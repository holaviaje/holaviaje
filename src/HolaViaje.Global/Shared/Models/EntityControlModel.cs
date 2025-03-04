﻿namespace HolaViaje.Global.Shared.Models;

public record struct EntityControlModel
{
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}