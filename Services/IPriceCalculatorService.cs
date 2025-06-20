using VehiclePriceCalculator.Models;

namespace VehiclePriceCalculator.Services
{
    public interface IPriceCalculatorService
    {
        VehiclePriceResponse CalculatePrice(VehiclePriceRequest request);
    }
}
