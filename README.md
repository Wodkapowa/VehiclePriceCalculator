# 🚗 Vehicle Price Calculator API

A simple ASP.NET Core Web API for calculating the final vehicle price based on base price and optional equipment, supporting both net and gross input values. VAT is automatically calculated.

## 🧰 Features

- Accepts base price and equipment in net or gross form
- Calculates final net and gross prices with a provided VAT rate
- Includes clean, testable logic

## 🔧 Technologies

- .NET 9 (Preview)
- ASP.NET Core
- MS test

## ▶️ Getting Started

1. Clone the repo:
   bash
   git clone https://github.com/Wodkapowa/VehiclePriceCalculator.git
   cd VehiclePriceCalculator

2. dotnet run --project VehiclePriceCalculator


3. Send a sample request:

POST /api/pricecalculator/calculate
Content-Type: application/json

{
  "baseNet": 10000,
  "baseGross": 0,
  "equipmentNet": 0,
  "equipmentGross": 122,
  "vatRate": 22
}

4. 📂 Project Structure

VehiclePriceCalculator/
├── Controllers/
│   └── PriceCalculatorController.cs
├── Models/
│   ├── VehiclePriceRequest.cs
│   ├── VehiclePriceResponse.cs
│   └── PriceDetail.cs
├── Tests/
│   └── VehiclePriceCalculator.Tests/	
│       └── PriceCalculatorControllerTests.cs
