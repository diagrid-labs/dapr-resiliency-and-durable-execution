using Dapr.Workflow;

namespace WorkflowApp
{
    public class CheckInventory : WorkflowActivity<OrderItem, InventoryResult>
    {
        public override Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem input)
        {
            throw new NotImplementedException();
        }
    }


}