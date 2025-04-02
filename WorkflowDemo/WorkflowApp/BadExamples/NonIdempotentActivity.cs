using Dapr.Workflow;

namespace WorkflowApp
{
    public class NonIdempotentActivity : WorkflowActivity<OrderItem, InventoryResult>
    {

        public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        {
            // Beware of non-idempotent operations in an activity!
            // For instance, can you save a record to a database twice without side effects?
            // Dapr Workflow guarantees at-least-once execution of activities.
            // So activities might be executed more than once.

            // Do a read operation to check if a record exists, before inserting it again.
            // Maybe the API you use in the activity natively supports idempotent operations.

            return new InventoryResult(IsKnownProduct: true, IsSufficientStock: true);
        }
    }
}