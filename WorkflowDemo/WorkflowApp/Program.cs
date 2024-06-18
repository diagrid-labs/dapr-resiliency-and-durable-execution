using Dapr.Client;
using Dapr.Workflow;
using WorkflowApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<HttpClient>(DaprClient.CreateInvokeHttpClient(appId: "shipping"));
builder.Services.AddDaprClient();
builder.Services.AddDaprWorkflowClient();
builder.Services.AddDaprWorkflow(options =>{
    options.RegisterWorkflow<ValidateOrderWorkflow>();
    options.RegisterActivity<UpdateInventory>();
    options.RegisterActivity<UndoUpdateInventory>();
    options.RegisterActivity<ShippingCalculator>();
});
var app = builder.Build();

app.MapPost("/validateOrder", async (
    Order order,
    DaprWorkflowClient daprWorkflowClient
    ) => {
        Console.WriteLine($"Validating order {order.Id} for.");
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

        return Results.Ok(productInventory);
});

app.Run();
