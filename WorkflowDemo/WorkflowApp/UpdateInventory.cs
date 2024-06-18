using Dapr.Client;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class UndoUpdateInventory : WorkflowActivity<OrderItem, InventoryResult>
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger _logger;
        private const string StateStoreName = "inventory";

        public UndoUpdateInventory(DaprClient daprClient, ILoggerFactory loggerFactory)
        {
            _daprClient = daprClient;
            _logger = loggerFactory.CreateLogger<UndoUpdateInventory>();
        }
        
        public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        {
            var productInventory = await _daprClient.GetStateAsync<ProductInventory>(StateStoreName, orderItem.ProductId);
            if (productInventory != null)
            {
                if (productInventory.Quantity >= orderItem.Quantity)
                {
                    var updatedInventory = new ProductInventory(
                        productInventory.ProductId,
                        productInventory.Quantity - orderItem.Quantity);

                    await _daprClient.SaveStateAsync(
                        StateStoreName,
                        productInventory.ProductId,
                        updatedInventory);

                    _logger.LogInformation("Updated inventory for product {ProductId} with quantity {ProductQuantity}", productInventory.ProductId, productInventory);

                    return new InventoryResult(IsKnownProduct: true, IsSufficientStock: true);
                    
                }
                else
                {
                    _logger.LogInformation("Insufficient inventory for product {ProductId} with quantity {ProductQuantity}", productInventory.ProductId, productInventory);

                    return new InventoryResult(IsKnownProduct: true, IsSufficientStock: false);
                }
                
            }

            _logger.LogInformation("Unknown inventory productId {ProductId}.", orderItem.ProductId);

            return new InventoryResult(IsKnownProduct: false, IsSufficientStock: false);
        }
    }
}