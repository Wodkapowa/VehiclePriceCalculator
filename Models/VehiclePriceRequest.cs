namespace VehiclePriceCalculator.Models;

public class VehiclePriceRequest
{
    public decimal? BaseNet { get; set; }
    public decimal? BaseGross { get; set; }
    public decimal? EquipmentNet { get; set; }
    public decimal? EquipmentGross { get; set; }
    public decimal VatRate { get; set; } // npr. 22%
}
