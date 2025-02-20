using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HolaViaje.Social.Controllers;

public class ControllerCore : ControllerBase
{
    private long _userIdentity = 0;

    public long UserIdentity
    {
        get
        {
            if (_userIdentity == 0)
            {
                if (long.TryParse(HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    _userIdentity = userId;
                }
                else
                {
                    _userIdentity = 0;
                }
            }

            return _userIdentity;
        }
    }
}
