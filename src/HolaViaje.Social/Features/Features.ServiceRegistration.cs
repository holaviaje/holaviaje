﻿using HolaViaje.Social.Features.Profiles;

namespace HolaViaje.Social.Features;

public static class ServiceRegistration
{
    public static IServiceCollection AddSocialFeatures(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceRegistration).Assembly);
        services.AddProfilesFeature();
        return services;
    }
}