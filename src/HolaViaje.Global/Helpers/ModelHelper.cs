namespace HolaViaje.Global.Helpers;

public static class ModelHelper
{
    public static Guid GetRecordId(Guid? modeId) => modeId is null || modeId.Value == Guid.Empty ? Guid.NewGuid() : modeId.Value;
}
