var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

app.MapPost("/calculateCost", (
    ShippingInfo shippingInfo) => {
    Console.WriteLine($"Getting shipping info for {shippingInfo.Country}.");
    Thread.Sleep(8000); //😱
    ShippingResult shippingResult = new(
        IsShippingAvailable: true,
        Cost: Math.Round(Convert.ToDecimal(new Random().NextDouble()) * 100, 2));

    return Results.Ok(shippingResult);
});

app.Run();

public record ShippingInfo(string Country);
public record ShippingResult(bool IsShippingAvailable, decimal Cost);
