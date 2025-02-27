namespace HolaViaje.Catalog.Shared.Models;

public record CancellationPolicyModel(CancellationPolicyType Policy, int DaysToCancel, int RefundPercentage) { }