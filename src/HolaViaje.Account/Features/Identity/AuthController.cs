using HolaViaje.Account.Extensions;
using HolaViaje.Account.Features.Identity.Events;
using HolaViaje.Account.Features.Identity.Models;
using HolaViaje.Infrastructure.Messaging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace HolaViaje.Account.Features.Identity;

[Route("[controller]")]
[ApiController]
public class AuthController(
          UserManager<ApplicationUser> _userManager,
          SignInManager<ApplicationUser> _signInManager,
          //IOpenIddictApplicationManager _applicationManager,
          IOpenIddictScopeManager _scopeManager,
          IEventBus _eventBus) : ControllerBase
{
    [HttpPost("~/connect/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Publish event
                var userInfo = await _userManager.FindByNameAsync(model.UserName);

                if (userInfo is not null)
                {
                    await _eventBus.Publish(new UserRegistered { AccountId = userInfo.Id, Email = userInfo.Email });
                }

                return Ok();
            }

            AddErrors(result);
        }

        return BadRequest(ModelState);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    //[HttpPost("~/connect/client/register")]
    //public async Task<IActionResult> RegisterClient([FromBody] RegisterClientModel model)
    //{
    //    try
    //    {
    //        await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
    //        {
    //            ClientId = model.ClientId,
    //            ClientSecret = model.ClientSecret,
    //            DisplayName = model.ClientName,
    //            Permissions =
    //            {
    //                Permissions.Endpoints.Token,
    //                Permissions.Endpoints.Authorization,

    //                Permissions.GrantTypes.ClientCredentials,
    //                Permissions.GrantTypes.RefreshToken,

    //                Permissions.Prefixes.Scope + "Resourse",
    //                Permissions.Prefixes.Scope + Scopes.OfflineAccess,

    //            }
    //        });

    //        return Ok(model);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.ToString());
    //    }
    //}

    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve the user principal stored in the authentication cookie.
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        // If the user principal can't be extracted, redirect the user to the login page.
        if (!result.Succeeded)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                });
        }

        // Create a new claims principal
        var claims = new List<Claim>
            {
                // 'subject' claim which is required
                new Claim(Claims.Subject, result.Principal.Identity.Name),
                new Claim(Claims.Username, result.Principal.Identity.Name),
                new Claim(Claims.Audience, "Resourse"),
            };

        var email = result.Principal.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Email);
        if (email is not null)
        {
            claims.Add(new Claim(Claims.Email, email.Value));
        }

        var identity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        // Getting scopes from oidc request parameters (TokenViewModel) and adding in Identity 
        identity.SetScopes(new[]
        {
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.OfflineAccess,
                    Scopes.Roles
                }.Concat(request.GetScopes()));

        identity.SetDestinations(GetDestinations);

        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Signing in with the OpenIddict authentiction scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Token()
    {
        try
        {
            var oidcRequest = HttpContext.GetOpenIddictServerRequest() ??
                     throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

            if (oidcRequest.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Subject (sub) is a required field, we use the client id as the subject identifier here.
                identity.AddClaim(Claims.Subject, oidcRequest.ClientId ?? throw new InvalidOperationException());


                // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
                //identity.AddClaim("some-claim", "some-value", Destinations.AccessToken);
                identity.AddClaim(new Claim(Claims.Audience, "Resourse"));

                // Setting scopes, resources and destinations
                identity.SetScopes(oidcRequest.GetScopes());
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
                identity.SetDestinations(GetDestinations);

                claimsPrincipal = new ClaimsPrincipal(identity);

            }
            else if (oidcRequest.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the authorization code.
                claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))?.Principal ?? throw new InvalidOperationException();
            }
            else if (oidcRequest.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                if (authenticateResult.Succeeded && authenticateResult.Principal != null)
                {
                    claimsPrincipal = authenticateResult.Principal;
                }
                else if (authenticateResult.Failure is not null)
                {
                    var failureMessage = authenticateResult.Failure.Message;
                    var failureException = authenticateResult.Failure.InnerException;
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidRequest,
                        ErrorDescription = failureMessage + failureException
                    });
                }
            }
            else if (oidcRequest.IsPasswordGrantType())
            {
                ClaimsIdentity identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
                ApplicationUser? user = await _userManager.FindByNameAsync(oidcRequest.Username);

                if (user is null)
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "User does not exist"
                    });
                }

                // Check that the user can sign in and is not locked out.
                // If two-factor authentication is supported, it would also be appropriate to check that 2FA is enabled for the user
                if (!await _signInManager.CanSignInAsync(user) || (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)))
                {
                    // Return bad request is the user can't sign in
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "The specified user cannot sign in."
                    });
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.PasswordSignInAsync(user.UserName, oidcRequest.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    if (result.IsNotAllowed)
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "User not allowed to login. Please confirm your email"
                        });
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "User requires 2F authentication"
                        });
                    }

                    if (result.IsLockedOut)
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "User is locked out"
                        });
                    }
                    else
                    {
                        return BadRequest(new OpenIddictResponse
                        {
                            Error = Errors.InvalidGrant,
                            ErrorDescription = "Username or password is incorrect"
                        });
                    }
                }

                // The user is now validated, so reset lockout counts, if necessary
                if (_userManager.SupportsUserLockout)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                // Getting scopes from oidc request parameters (TokenViewModel) and adding in Identity 
                identity.SetScopes(new[]
                {
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.OfflineAccess,
                    Scopes.Roles
                }.Concat(oidcRequest.GetScopes()));

                // Checking in OpenIddictScopes tables for matching resources
                // Adding in Identity
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());


                // sub claims is mendatory
                identity.AddClaim(new Claim(Claims.Subject, user.Id.ToString()));
                // Add Custom claims
                identity.AddClaim(new Claim(Claims.Audience, "Resourse"));
                identity.AddClaim(new Claim(Claims.Name, user.UserName));

                // Setting destinations of claims i.e. identity token or access token
                identity.SetDestinations(GetDestinations);

                claimsPrincipal = new ClaimsPrincipal(identity);

            }
            else
            {
                return BadRequest(new
                {
                    error = Errors.UnsupportedGrantType,
                    error_description = "The specified grant type is not supported."
                });
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            var signInResult = SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return signInResult;
        }
        catch (Exception)
        {
            return BadRequest(new OpenIddictResponse()
            {
                Error = Errors.ServerError,
                ErrorDescription = "Invalid login attempt"
            });
        }
    }

    [HttpPost("~/connect/endsession")]
    public async Task<IActionResult> EndSession()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve the user principal stored in the authentication cookie.
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        // If the user principal can't be extracted, redirect the user to the login page.
        if (!result.Succeeded)
        {
            return Ok(
                new
                {
                    result.Failure?.Message
                }
            );
        }

        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        await _signInManager.SignOutAsync();
        // Signout with the OpenIddict authentiction scheme
        return SignOut(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo")]
    public async Task<IActionResult> UserInfo()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (authenticateResult.Succeeded && authenticateResult.Principal != null)
        {
            return Ok(new
            {
                Name = authenticateResult.Principal.GetClaim(Claims.Name),
            });
        }
        else if (authenticateResult.Failure is not null)
        {
            var failureMessage = authenticateResult.Failure.Message;
            var failureException = authenticateResult.Failure.InnerException;
            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.InvalidRequest,
                ErrorDescription = failureMessage + failureException
            });
        }

        return BadRequest(new OpenIddictResponse
        {
            Error = Errors.InvalidRequest,
            ErrorDescription = "Unkonwn error"
        });
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        return claim.Type switch
        {
            Claims.Name or
            Claims.PreferredUsername or
            Claims.Email or
            Claims.Role or
            Claims.Subject
               => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            "AspNet.Identity.SecurityStamp" => Array.Empty<string>(),

            "secret_value" => Array.Empty<string>(),

            _ => new[] { Destinations.AccessToken },
        };

    }
}
