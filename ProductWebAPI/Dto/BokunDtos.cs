using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProductWebAPI.Dto
{
    // Matches the Bokun adapter HTTP response shape returned by the Bokun service.
    // Includes both the "new" simple shape (availableUnits, unitPrice, currency) and
    // the legacy Bokun fields (availabilityCount, pricesByRate) for compatibility.
    public class BokunAvailabilityDto
    {
        public DateTime Date { get; set; }

        // Bokun returns "availableUnits"
        public int AvailableUnits { get; set; }

        // Bokun returns both "price" and "unitPrice"
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }

        public string Currency { get; set; }
    }



    public class BokunPriceByRate
    {
        [JsonPropertyName("pricePerCategoryUnit")]
        public List<BokunPricePerCategoryUnit>? PricePerCategoryUnit { get; set; }
    }

    public class BokunPricePerCategoryUnit
    {
        [JsonPropertyName("amount")]
        public BokunAmount? Amount { get; set; }
    }

    public class BokunAmount
    {
        // Match the shape used in the adapter: {.value, .currency} or similar
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
    }
}