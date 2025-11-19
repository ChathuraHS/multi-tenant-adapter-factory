using System.Text.Json.Serialization;

namespace BokunAdapter.Dto
{
    public class BokunPriceByRate
    {
        [JsonPropertyName("activityRateId")]
        public long ActivityRateId { get; set; }

        [JsonPropertyName("pricePerCategoryUnit")]
        public List<PricePerCategoryUnit> PricePerCategoryUnit { get; set; }
    }

    public class PricePerCategoryUnit
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("amount")]
        public Amount Amount { get; set; }

        [JsonPropertyName("minParticipantsRequired")]
        public int MinParticipantsRequired { get; set; }
    }

    public class Amount
    {
        [JsonPropertyName("amount")]
        public decimal Value { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

}
