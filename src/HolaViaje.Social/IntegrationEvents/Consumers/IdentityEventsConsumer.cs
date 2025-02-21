using HolaViaje.Account.Features.Identity.Events;
using HolaViaje.Social.Features.Profiles;
using MassTransit;

namespace HolaViaje.Social.IntegrationEvents.Consumers;

public class IdentityEventsConsumer(IUserProfileApplication profileApplication,
    ILogger<UserProfileApplication> logger) : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var result = await profileApplication.CreateAsync(context.Message.AccountId);

        result.Switch(
            success => logger.LogInformation("User profile created for account {AccountId}", context.Message.AccountId),
            error => logger.LogError("Failed to create user profile for account {AccountId}. Error: {Error}",
                context.Message.AccountId, error.Message));
    }
}