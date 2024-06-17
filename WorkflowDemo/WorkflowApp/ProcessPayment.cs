using Dapr.Workflow;

namespace WorkflowApp;

public class ProcessPayment : WorkflowActivity<OrderItem, PaymentResult>
{
    public override Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem input)
    {
        throw new NotImplementedException();
    }
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
}