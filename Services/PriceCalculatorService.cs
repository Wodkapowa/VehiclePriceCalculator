using System;
using VehiclePriceCalculator.Models;

namespace VehiclePriceCalculator.Services
{
    public class PriceCalculatorService : IPriceCalculatorService
    {
        public VehiclePriceResponse CalculatePrice(VehiclePriceRequest request)
        {
            var vat = request.VatRate / 100m;

            var basePrice = ConvertToNetAndGross(request.BaseNet, request.BaseGross, vat, required: true);
            var equipmentPrice = ConvertToNetAndGross(request.EquipmentNet, request.EquipmentGross, vat, required: false);

            var totalNet = basePrice.Net + equipmentPrice.Net;
            var totalGross = basePrice.Gross + equipmentPrice.Gross;

            return new VehiclePriceResponse
            {
                BasePrice = basePrice,
                AdditionalEquipment = equipmentPrice,
                TotalPrice = new PriceDetail { Net = totalNet, Gross = totalGross }
            };
        }

        private PriceDetail ConvertToNetAndGross(decimal? net, decimal? gross, decimal vatRate, bool required)
        {
            net ??= 0;
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

            return new PriceDetail { Net = 0, Gross = 0 };
        }
    }
}
