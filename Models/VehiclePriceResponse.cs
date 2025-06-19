using System.Text.Json.Serialization;

namespace VehiclePriceCalculator.Models;

public class VehiclePriceResponse
{
    public required PriceDetail BasePrice { get; set; }
    public required PriceDetail AdditionalEquipment { get; set; }
    public required PriceDetail TotalPrice { get; set; }
    public PriceCategory Label => GetLabelFromPrice(TotalPrice.Gross);

    private static PriceCategory GetLabelFromPrice(decimal gross)
    {
        if (gross < 15000)
            return PriceCategory.Budget;
        if (gross < 30000)
            return PriceCategory.Affordable;
        return PriceCategory.Expensive;
    }
}

public class PriceDetail
{
    public decimal Net { get; set; }
    public decimal Gross { get; set; }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PriceCategory
{
    [JsonPropertyName("Budget")]
    Budget,

    [JsonPropertyName("Affordable")]
    Affordable,

    [JsonPropertyName("Expensive")]
    Expensive
}
