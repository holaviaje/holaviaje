namespace HolaViaje.Catalog.Shared;

public record struct CancellationPolicy(CancellationPolicyType Policy, int DaysToCancel, int RefundPercentage) { }