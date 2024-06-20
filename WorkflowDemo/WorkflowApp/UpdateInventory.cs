using Dapr.Client;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class UpdateInventory : WorkflowActivity<OrderItem, InventoryResult>
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger _logger;
        private const string StateStoreComponentName = "inventory";

        public UpdateInventory(DaprClient daprClient, ILoggerFactory loggerFactory)
        {
            _daprClient = daprClient;
            _logger = loggerFactory.CreateLogger<UndoUpdateInventory>();
        }
        
        public override async Task<InventoryResult> RunAsync(WorkflowActivityContext context, OrderItem orderItem)
        {
            var productInventory = await _daprClient.GetStateAsync<ProductInventory>(StateStoreComponentName, orderItem.ProductId);
            if (productInventory != null)
            {
                if (productInventory.Quantity >= orderItem.Quantity)
                {
                    var updatedInventory = new ProductInventory(
                        productInventory.ProductId,
                        productInventory.Quantity - orderItem.Quantity);

                    await _daprClient.SaveStateAsync(
                        StateStoreComponentName,
                        productInventory.ProductId,
                        updatedInventory);

                    _logger.LogInformation("Reduced inventory for product {ProductId} with quantity {ProductQuantity}", updatedInventory.ProductId, orderItem.Quantity);

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