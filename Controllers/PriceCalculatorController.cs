using Microsoft.AspNetCore.Mvc;
using VehiclePriceCalculator.Models;

namespace VehiclePriceCalculator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceCalculatorController : ControllerBase
{
    [HttpPost("calculate")]
    public ActionResult<VehiclePriceResponse> Calculate([FromBody] VehiclePriceRequest request)
    {
        try
        {
            var basePrice = ConvertToNetAndGross(request.BaseNet, request.BaseGross, request.VatRate);
            var equipmentPrice = ConvertToNetAndGross(request.EquipmentNet, request.EquipmentGross, request.VatRate);

            var totalNet = basePrice.Net + equipmentPrice.Net;
            var totalGross = basePrice.Gross + equipmentPrice.Gross;

            var response = new VehiclePriceResponse
            {
                BasePrice = basePrice,
                AdditionalEquipment = equipmentPrice,
                TotalPrice = new PriceDetail { Net = totalNet, Gross = totalGross }
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private PriceDetail ConvertToNetAndGross(decimal? net, decimal? gross, decimal vatRate)
    {
        if (net.HasValue)
        {
            return new PriceDetail
            {
                Net = Math.Round(net.Value, 2),
                Gross = Math.Round(net.Value * (1 + vatRate), 2)
            };
        }
        else if (gross.HasValue)
        {
            return new PriceDetail
            {
                Net = Math.Round(gross.Value / (1 + vatRate), 2),
                Gross = Math.Round(gross.Value, 2)
            };
        }

        throw new ArgumentException("Either net or gross price must be provided.");
    }
}
