namespace HolaViaje.Catalog.Shared;

public record CancellationPolicy(CancellationPolicyType Policy, int DaysToCancel, int RefundPercentage) { }