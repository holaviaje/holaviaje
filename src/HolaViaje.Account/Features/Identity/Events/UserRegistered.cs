﻿namespace HolaViaje.Account.Features.Identity.Events;

public class UserRegistered
{
    public long AccountId { get; set; }
    public string Email { get; set; } = string.Empty;
}
