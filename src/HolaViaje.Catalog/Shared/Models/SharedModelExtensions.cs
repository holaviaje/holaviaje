using HolaViaje.Global.Helpers;

namespace HolaViaje.Catalog.Shared.Models;

public static class SharedModelExtensions
{
    public static CancellationPolicy FromModel(this CancellationPolicyModel model) => new CancellationPolicy(model.Policy, model.DaysToCancel, model.RefundPercentage);

    public static BookInfo FromModel(this BookInfoModel model) => new BookInfo(model.Phone, model.WhatsApp, model.Email);

    public static Duration FromModel(this DurationModel model) => new Duration()
    {
        Days = model.Days,
        Hours = model.Hours,
        Minutes = model.Minutes
    };

    public static IEnumerable<AdditionalInfo> FromModel(this IEnumerable<AdditionalInfoModel> models) => models.Select(m => new AdditionalInfo()
    {
        RecordId = ModelHelper.GetRecordId(m.RecordId),
        Description = m.Description
    });
}