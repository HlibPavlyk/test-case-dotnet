using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NodaTime;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;

namespace TransactionApi.Infrastructure.DataServices;

public class TimeZoneService : ITimeZoneService
{
    private readonly HttpClient _httpClient;

    public TimeZoneService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTimeZoneIdFromCoordinatesAsync(string coordinates)
    {
        try
        {
            var parts = coordinates.Split(", ");
            if (parts.Length != 2 )
            {
                throw new ArgumentException("Invalid coordinates format. Please provide coordinates in the format 'latitude,longitude'.");
            }

            var requestUri = $"https://timeapi.io/api/TimeZone/coordinate?latitude={parts[0]}&longitude={parts[1]}";

            var response = await _httpClient.GetStringAsync(requestUri);
            var json = JObject.Parse(response);

            if (json["timeZone"] == null)
            {
                throw new Exception("Failed to retrieve time zone information");
            }

            return json["timeZone"].ToString();
        }
        catch (HttpRequestException httpRequestException)
        {
            throw new Exception("Error occurred while making the request to the Time API.", httpRequestException);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }

    public DateTime ConvertLocalTimeToOtherByTimeZoneIdAsync(DateTime originLocalTime, string originTimeZoneId,
        string destinationTimeZoneId)
    {
        try
        {
            var localDateTime = LocalDateTime.FromDateTime(originLocalTime);
            var originZone = DateTimeZoneProviders.Tzdb[originTimeZoneId];
            var destinationZone = DateTimeZoneProviders.Tzdb[destinationTimeZoneId];

            var originZonedDateTime = originZone.AtLeniently(localDateTime);
            var originInstant = originZonedDateTime.ToInstant();
            var destinationZonedDateTime = originInstant.InZone(destinationZone);
            var destinationLocalDateTime = destinationZonedDateTime.LocalDateTime;

            return destinationLocalDateTime.ToDateTimeUnspecified();
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while converting time.", ex);
        }
    }
}