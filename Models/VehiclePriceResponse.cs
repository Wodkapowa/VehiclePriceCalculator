namespace VehiclePriceCalculator.Models;

public class VehiclePriceResponse
{
    public required PriceDetail BasePrice { get; set; }
    public required PriceDetail AdditionalEquipment { get; set; }
    public required PriceDetail TotalPrice { get; set; }
}

public class PriceDetail
{
    public decimal Net { get; set; }
    public decimal Gross { get; set; }
}
