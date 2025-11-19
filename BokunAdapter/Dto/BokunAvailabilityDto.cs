using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BokunAdapter.Dto
{
    public class BokunAvailabilityDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("date")]
        [JsonConverter(typeof(UnixMillisecondsToDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonPropertyName("availabilityCount")]
        public int AvailabilityCount { get; set; }

        [JsonPropertyName("unlimitedAvailability")]
        public bool UnlimitedAvailability { get; set; }

        [JsonPropertyName("soldOut")]
        public bool SoldOut { get; set; }

        [JsonPropertyName("pricesByRate")]
        public List<BokunPriceByRate> PricesByRate { get; set; }
    }

    public class BokunPrice
    {
        [JsonPropertyName("rateId")]
        public long RateId { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public class UnixMillisecondsToDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var ms = reader.GetInt64();
                return DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                // Try parse as ISO 8601 string
                var str = reader.GetString();
                if (DateTime.TryParse(str, out var dt))
                    return dt;

                return default; // fallback
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }

            throw new JsonException($"Unexpected token parsing DateTime. TokenType: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
        }
    }

}
