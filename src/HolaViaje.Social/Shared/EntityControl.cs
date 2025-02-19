namespace HolaViaje.Social.Shared;

public record EntityControl
{
    public DateTime CreateAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}