namespace TransactionApi.Application.Abstractions;

public interface ITimeZoneService
{
    Task<string> GetTimeZoneIdFromCoordinatesAsync(string coordinates);
    DateTime ConvertLocalTimeToOtherByTimeZoneIdAsync(DateTime originLocalTime, string originTimeZoneId, string destinationTimeZoneId);
}