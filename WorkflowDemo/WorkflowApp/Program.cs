using Dapr.Client;
using Dapr.Workflow;
using WorkflowApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<HttpClient>(DaprClient.CreateInvokeHttpClient(appId: "shipping"));
builder.Services.AddDaprWorkflowClient();
builder.Services.AddDaprWorkflow(options =>{
    options.RegisterWorkflow<ValidateOrderWorkflow>();
    options.RegisterActivity<CheckInventory>();
    options.RegisterActivity<ShippingCalculator>();
});
var app = builder.Build();

app.MapPost("/validateOrder", async (
    Order order,
    DaprWorkflowClient daprWorkflowClient
    ) => {
        var instanceId = await daprWorkflowClient.ScheduleNewWorkflowAsync(
            nameof(ValidateOrderWorkflow),
            instanceId: order.Id,
            input: order);

        return Results.Accepted(instanceId);
});

app.Run();
