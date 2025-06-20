using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehiclePriceCalculator.Controllers;
using VehiclePriceCalculator.Models;
using Microsoft.Extensions.Logging.Abstractions;
using VehiclePriceCalculator.Services;

namespace VehiclePriceCalculator.Tests;

[TestClass]
public sealed class PriceCalculatorControllerTests
{
    private IPriceCalculatorService priceCalculatorService;

    [TestInitialize]
    public void Setup()
    {
        priceCalculatorService = new PriceCalculatorService();
    }

    [TestMethod]
    public void Calculate_ReturnsCorrectNetAndGrossTotals()
    {
        // Arrange
        var request = new VehiclePriceRequest
        {
            BaseNet = 10000m,
            BaseGross = null,
            EquipmentNet = null,
            EquipmentGross = 122m,
            VatRate = 22m
        };

        decimal expectedBaseNet = 10000m;
        decimal expectedBaseGross = 12200m;
        decimal expectedEquipmentNet = 100m; // 122 / 1.22
        decimal expectedEquipmentGross = 122m;
        decimal expectedTotalNet = 10100m;
        decimal expectedTotalGross = 12322m;

        var logger = NullLogger<PriceCalculatorController>.Instance;
        var controller = new PriceCalculatorController(logger, priceCalculatorService);

        // Act
        var result = controller.Calculate(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        var response = (result.Result as OkObjectResult)?.Value as VehiclePriceResponse;
        Assert.IsNotNull(response);

        Assert.AreEqual(expectedBaseNet, response.BasePrice.Net, 0.01m);
        Assert.AreEqual(expectedBaseGross, response.BasePrice.Gross, 0.01m);
        Assert.AreEqual(expectedEquipmentNet, response.AdditionalEquipment.Net, 0.01m);
        Assert.AreEqual(expectedEquipmentGross, response.AdditionalEquipment.Gross, 0.01m);
        Assert.AreEqual(expectedTotalNet, response.TotalPrice.Net, 0.01m);
        Assert.AreEqual(expectedTotalGross, response.TotalPrice.Gross, 0.01m);
    }
    [TestMethod]
    public void Calculate_WithOnlyBaseGross_ReturnsCorrectValues()
    {
        var request = new VehiclePriceRequest
        {
            BaseNet = null,
            BaseGross = 12200m,
            EquipmentNet = null,
            EquipmentGross = null,
            VatRate = 22m
        };

        var logger = NullLogger<PriceCalculatorController>.Instance;
        var controller = new PriceCalculatorController(logger, priceCalculatorService);

        var result = controller.Calculate(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        var response = (result.Result as OkObjectResult)?.Value as VehiclePriceResponse;
        Assert.IsNotNull(response);

        Assert.AreEqual(10000m, response.BasePrice.Net, 0.01m);
        Assert.AreEqual(12200m, response.BasePrice.Gross, 0.01m);
        Assert.AreEqual(0m, response.AdditionalEquipment.Net, 0.01m);
        Assert.AreEqual(0m, response.AdditionalEquipment.Gross, 0.01m);
    }
    [TestMethod]
    public void Calculate_WithoutBasePrice_ThrowsBadRequest()
    {
        var request = new VehiclePriceRequest
        {
            BaseNet = null,
            BaseGross = null,
            EquipmentNet = null,
            EquipmentGross = 100m,
            VatRate = 22m
        };

        var logger = NullLogger<PriceCalculatorController>.Instance;
        var controller = new PriceCalculatorController(logger, priceCalculatorService);

        var result = controller.Calculate(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

        var badRequest = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest.Value.ToString());
        StringAssert.Contains(badRequest.Value.ToString(), "baseNet");
    }
    [TestMethod]
    public void Calculate_OnlyEquipmentNetProvided_ReturnsCorrectTotals()
    {
        var request = new VehiclePriceRequest
        {
            BaseNet = 5000m,
            BaseGross = null,
            EquipmentNet = 1000m,
            EquipmentGross = null,
            VatRate = 20m
        };

        var logger = NullLogger<PriceCalculatorController>.Instance;
        var controller = new PriceCalculatorController(logger, priceCalculatorService);

        var result = controller.Calculate(request);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        var response = (result.Result as OkObjectResult)?.Value as VehiclePriceResponse;
        Assert.IsNotNull(response);

        Assert.AreEqual(5000m, response.BasePrice.Net, 0.01m);
        Assert.AreEqual(6000m, response.BasePrice.Gross, 0.01m);
        Assert.AreEqual(1000m, response.AdditionalEquipment.Net, 0.01m);
        Assert.AreEqual(1200m, response.AdditionalEquipment.Gross, 0.01m);
        Assert.AreEqual(6000m, response.BasePrice.Gross, 0.01m);
        Assert.AreEqual(1200m, response.AdditionalEquipment.Gross, 0.01m);
        Assert.AreEqual(7200m, response.TotalPrice.Gross, 0.01m);
    }
}