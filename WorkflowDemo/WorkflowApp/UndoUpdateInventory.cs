using Dapr.Client;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class UndoUpdateInventory : WorkflowActivity<OrderItem, InventoryResult>
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<UpdateInventory> _logger;
        private const string StateStoreName = "inventory";

        public UndoUpdateInventory(DaprClient daprClient, ILoggerFactory loggerFactory)
        {
            _daprClient = daprClient;
            _logger = loggerFactory.CreateLogger<UpdateInventory>();
        }

        public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        {
            var productInventory = await _daprClient.GetStateAsync<ProductInventory>(StateStoreName, orderItem.ProductId);
            if (productInventory != null)
            {
                var updatedInventory = new ProductInventory(
                    productInventory.ProductId,
                    productInventory.Quantity + orderItem.Quantity);

                await _daprClient.SaveStateAsync(
                    StateStoreName,
                    productInventory.ProductId,
                    updatedInventory);

                _logger.LogInformation("Reverted inventory update for product {ProductId} to quantity {ProductQuantity}", updatedInventory.ProductId, updatedInventory.Quantity);

                return new InventoryResult(IsKnownProduct: true, IsSufficientStock: true);
            }

            return new InventoryResult(IsKnownProduct: false, IsSufficientStock: false);
        }
    }
}