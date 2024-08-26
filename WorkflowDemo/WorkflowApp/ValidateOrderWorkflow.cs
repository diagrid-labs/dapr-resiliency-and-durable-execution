using Dapr.Workflow;

namespace WorkflowApp
{
    public class ValidateOrderWorkflow : Workflow<Order, OrderValidationResult>
    {
        public override async Task<OrderValidationResult> RunAsync(WorkflowContext context, Order order)
        {
            RegisterShipmentResult? registerShipmentResult = default;

            var inventoryResult = await context.CallActivityAsync<InventoryResult>(
                nameof(UpdateInventory),
                order.OrderItem);

            if (inventoryResult.IsSufficientStock)
            {
                string[] shippingServices = [
                    "Shipping Service A",
                    "Shipping Service B",
                    "Shipping Service C"];
                List<Task<ShippingCostResult>> shippingCostResultTasks = [];

                foreach (var shippingService in shippingServices)
                {
                    ShippingCostRequest shippingRequest = new(shippingService, order);
                    shippingCostResultTasks.Add(context.CallActivityAsync<ShippingCostResult>(
                        nameof(GetShippingCost),
                        shippingRequest));
                }

                ShippingCostResult[] shippingCostResults = await Task.WhenAll(shippingCostResultTasks);
                ShippingCostResult cheapestShippingService = shippingCostResults.MinBy(result => result.Cost);

                try
                {
                    RegisterShipmentRequest registerShipmentRequest = new(order, cheapestShippingService.ShippingService);
                    registerShipmentResult = await context.CallActivityAsync<RegisterShipmentResult>(
                        nameof(RegisterShipment),
                        registerShipmentRequest);
                }
                catch (WorkflowTaskFailedException ex)
                {
                    Console.WriteLine($"RegisterShipment activity failed: {ex.Message}");
                    var undoResult = await context.CallActivityAsync<InventoryResult>(
                        nameof(UndoUpdateInventory),
                        order.OrderItem);
                }
            }

            return new OrderValidationResult(inventoryResult, registerShipmentResult);
        }
    }
}