using Dapr.Client;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class SaveOrderActivity : WorkflowActivity<OrderItem, InventoryResult>
    {
        private readonly DaprClient _daprClient;
        private const string StateStoreComponentName = "inventory";

        public SaveOrderActivity(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        {
            // Beware of non-idempotent operations in an activity!
            // For instance, can you save a record to a database twice without side effects?
            // Dapr Workflow guarantees at-least-once execution of activities.
            // So activities might be executed more than once.
            await _daprClient.SaveStateAsync(
                StateStoreComponentName,
                orderItem.ProductId,
                orderItem.Quantity);

            return new InventoryResult(IsKnownProduct: true, IsSufficientStock: true);
        }

        // public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        // {
        //     var inventory = await _daprClient.GetStateAsync<ProductInventory>(
        //         StateStoreComponentName,
        //         orderItem.ProductId);
        //     if (inventory == null)
        //     {
        //         await _daprClient.SaveStateAsync(
        //             StateStoreComponentName,
        //             orderItem.ProductId,
        //             orderItem.Quantity);
        //     }

        //     return new InventoryResult(IsKnownProduct: true, IsSufficientStock: true);
        // }
    }
}