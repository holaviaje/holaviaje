namespace HolaViaje.Catalog.Shared.Models;

public record struct CancellationPolicyModel(CancellationPolicyType Policy, int DaysToCancel, int RefundPercentage) { }