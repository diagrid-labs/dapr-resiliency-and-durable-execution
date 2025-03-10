using Dapr.Client;
using Dapr.Workflow;
using WorkflowApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<HttpClient>(DaprClient.CreateInvokeHttpClient(appId: "shipping"));
builder.Services.AddDaprClient();

builder.Services.AddDaprWorkflow(options =>{
    options.RegisterWorkflow<ValidateOrderWorkflow>();
    options.RegisterActivity<UpdateInventory>();
    options.RegisterActivity<UndoUpdateInventory>();
    options.RegisterActivity<GetShippingCost>();
    options.RegisterActivity<GetShippingServices>();
    options.RegisterActivity<RegisterShipment>();
});
var app = builder.Build();

app.MapPost("/validateOrder", async (
    Order order
    ) => {
        Console.WriteLine($"Validating order {order.Id} for.");
        
        await using var scope = app.Services.CreateAsyncScope();
        var daprWorkflowClient = scope.ServiceProvider.GetRequiredService<DaprWorkflowClient>();

        var instanceId = await daprWorkflowClient.ScheduleNewWorkflowAsync(
            nameof(ValidateOrderWorkflow),
            instanceId: order.Id,
            input: order);

        return Results.Accepted(instanceId);
});

app.MapPost("/inventory/restock", async (
    ProductInventory productInventory,
    DaprClient daprClient
    ) => {
        await daprClient.SaveStateAsync(
                "inventory",
                productInventory.ProductId,
                productInventory);

        return Results.Accepted();
});

app.MapGet("/inventory/{productId}", async (
    string productId,
    DaprClient daprClient
    ) => {
        var productInventory = await daprClient.GetStateAsync<ProductInventory>(
                "inventory",
                productId);

        if (productInventory == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(productInventory);
});

app.Run();
