// These are namespaces being imported so you can use types like ControllerBase, ILogger, etc.
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehiclePriceCalculator.Models;
using VehiclePriceCalculator.Services;
using static VehiclePriceCalculator.Controllers.PriceCalculatorController;

namespace VehiclePriceCalculator.Controllers
{
    // This attribute marks the class as a Web API controller.
    [ApiController]

    // This sets the base route for all endpoints in this controller: "api/pricecalculator"
    [Route("api/[controller]")]

    // ✅ CLASS DEFINITION
    // This defines the controller class which inherits from ControllerBase
    public class PriceCalculatorController : ControllerBase
    {
        // ✅ PRIVATE FIELD
        // This is a private, read-only field to hold the injected logger instance
        private readonly ILogger<PriceCalculatorController> _logger;
        private readonly IPriceCalculatorService _priceCalculatorService;

        // ✅ CONSTRUCTOR
        // This is the constructor for the controller class.
        // It uses Dependency Injection to receive a logger instance.
        public PriceCalculatorController(ILogger<PriceCalculatorController> logger, IPriceCalculatorService priceCalculatorService)
        {
            _logger = logger;
            _priceCalculatorService = priceCalculatorService;
        }

        // ✅ ACTION METHOD
        // This method responds to HTTP POST requests at "api/pricecalculator/calculate"
        [HttpPost("calculate")]
        public ActionResult<VehiclePriceResponse> Calculate([FromBody] VehiclePriceRequest request)
        {
            // Logging the input data
            _logger.LogInformation("Received price calculation request: BaseNet={BaseNet}, BaseGross={BaseGross}, EquipmentNet={EquipmentNet}, EquipmentGross={EquipmentGross}, VatRate={VatRate}",
                request.BaseNet, request.BaseGross, request.EquipmentNet, request.EquipmentGross, request.VatRate);

            try
            {
                var response = _priceCalculatorService.CalculatePrice(request);

                _logger.LogInformation("Price calculation succeeded: TotalNet={TotalNet}, TotalGross={TotalGross}, Label={Label}",
                 response.TotalPrice.Net, response.TotalPrice.Gross, response.Label);

                return Ok(response); // Return HTTP 200 with response data
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Price calculation failed due to invalid input");
                return BadRequest(new { error = ex.Message }); // Return HTTP 400 with error
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during price calculation");
                return StatusCode(500, new { error = "Internal server error" }); // Return HTTP 500
            }
        }

        

            // ✅ PRIVATE HELPER METHOD
            // This private method helps calculate both net and gross prices based on available data
            private PriceDetail ConvertToNetAndGross(decimal? net, decimal? gross, decimal vatRate, bool required)
        {
            net ??= 0; // Use 0 if null
            gross ??= 0;

            if (net > 0)
            {
                return new PriceDetail
                {
                    Net = Math.Round(net.Value, 2),
                    Gross = Math.Round(net.Value * (1 + vatRate), 2)
                };
            }
            else if (gross > 0)
            {
                return new PriceDetail
                {
                    Net = Math.Round(gross.Value / (1 + vatRate), 2),
                    Gross = Math.Round(gross.Value, 2)
                };
            }

            if (required)
            {
                throw new ArgumentException("Either a non-zero baseNet or baseGross must be provided.");
            }

            return new PriceDetail { Net = 0, Gross = 0 }; // Default return when optional
        }
    }
}
