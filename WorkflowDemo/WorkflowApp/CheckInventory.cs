using Dapr.Client;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class CheckInventory : WorkflowActivity<OrderItem, InventoryResult>
    {
        private readonly DaprClient _daprClient;

        public CheckInventory(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        
        public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        {
            var productInventory = await _daprClient.GetStateAsync<ProductInventory>("inventory", orderItem.ProductId);
            if (productInventory != null)
            {
                if (productInventory.Quantity >= orderItem.Quantity)
                {
                    return new InventoryResult(IsKnownProduct: true, InStock: true);
                }
                else
                {
                    return new InventoryResult(IsKnownProduct: true, InStock: false);
                }
                
            }

            return new InventoryResult(IsKnownProduct: false, InStock: false);
        }
    }
}