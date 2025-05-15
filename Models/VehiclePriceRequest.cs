using System.ComponentModel;

namespace VehiclePriceCalculator.Models;

public class VehiclePriceRequest
{
    [DefaultValue(10000)]
    public decimal? BaseNet { get; set; }
    public decimal? BaseGross { get; set; }
    public decimal? EquipmentNet { get; set; }
    [DefaultValue(122)]
    public decimal? EquipmentGross { get; set; }
    [DefaultValue(22)]
    public decimal VatRate { get; set; } // npr. 22%
}
