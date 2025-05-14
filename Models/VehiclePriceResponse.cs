namespace VehiclePriceCalculator.Models;

public class VehiclePriceResponse
{
    public PriceDetail BasePrice { get; set; }
    public PriceDetail AdditionalEquipment { get; set; }
    public PriceDetail TotalPrice { get; set; }
}

public class PriceDetail
{
    public decimal Net { get; set; }
    public decimal Gross { get; set; }
}
