var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

app.MapPost("/calculateCost", (
    ShippingCostRequest shippingRequest) => {
    Console.WriteLine($"{shippingRequest.ShippingService}: Getting shipping costs for order {shippingRequest.Order.Id}.");
    Thread.Sleep(3000); //😱
    ShippingCostResult shippingResult = new(
        shippingRequest.ShippingService,
        IsShippingAvailable: true,
        Cost: Math.Round(Convert.ToDecimal(new Random().NextDouble()) * 100, 2));

    return Results.Ok(shippingResult);
});

app.MapPost("/registerShipment", (
    RegisterShipmentRequest shipmentRequest) => {
    Console.WriteLine($"{shipmentRequest.ShippingService}: Registering shipment for order {shipmentRequest.Order.Id}.");
    Thread.Sleep(4000); //😱
    RegisterShipmentResult shipmentResult = new(shipmentRequest.ShippingService, Guid.NewGuid().ToString(), IsRegistered: true);

    return Results.Ok(shipmentResult);
});

app.Run();
