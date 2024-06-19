using Dapr.Workflow;

namespace WorkflowApp
{
    public class ValidateOrderWorkflow : Workflow<Order, OrderValidationResult>
    {
        public override async Task<OrderValidationResult> RunAsync(WorkflowContext context, Order order)
        {
            var inventoryResult = await context.CallActivityAsync<InventoryResult>(
                nameof(UpdateInventory),
                order.OrderItem);

            ShippingResult shippingResult = new(IsShippingAvailable: false, 0);

            if (inventoryResult.IsSufficientStock)
            {
                try
                {
                    shippingResult = await context.CallActivityAsync<ShippingResult>(
                        nameof(ShippingCalculator),
                        order.ShippingInfo);
                }
                catch (WorkflowTaskFailedException ex)
                {
                    Console.WriteLine($"Shipping calculator activity failed: {ex.Message}");
                    var undoResult = await context.CallActivityAsync<InventoryResult>(
                        nameof(UndoUpdateInventory),
                        order.OrderItem);
                }
            }

            return new OrderValidationResult(inventoryResult, shippingResult);
        }
    }
}