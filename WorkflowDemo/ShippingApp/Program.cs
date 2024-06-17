using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DaprClient>(new DaprClientBuilder().Build());
var app = builder.Build();

app.MapPost("/getcost", async (
    ShippingInfo shippingInfo) => {
    Console.WriteLine($"Getting shipping info for {shippingInfo.Country}.");
    var shippingResult = new ShippingResult(IsShippingAvailable: true, Cost: Math.Round(new Random().NextDouble() * 100, 2));

    return Results.Ok(shippingResult);
});

app.Run();

public record ShippingInfo(string Country);
public record ShippingResult(bool IsShippingAvailable, decimal Cost);
