namespace HolaViaje.Catalog.Shared;

public enum CancellationPolicyType
{
    /// <summary>
    /// Free cancellation, no charge will be made if the user cancels 24h before.
    /// </summary>
    FreeCancellation,
    /// <summary>
    /// Partial refund, a percentage of the total amount will be refunded if the user cancels 24h before.
    /// </summary>
    PartialRefund,
    /// <summary>
    /// No refund, the user will not receive any refund if they cancel.
    /// </summary>
    NoRefund,
    /// <summary>
    /// Custom cancellation policy, the user will receive a refund based on the custom policy.
    /// </summary>
    Custom
}
