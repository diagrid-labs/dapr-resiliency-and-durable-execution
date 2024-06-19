using Dapr.Workflow;

namespace WorkflowApp
{
    public class ValidateOrderWorkflowWithRetry : Workflow<Order, OrderValidationResult>
    {
        public override async Task<OrderValidationResult> RunAsync(WorkflowContext context, Order order)
        {
            var inventoryResult = await context.CallActivityAsync<InventoryResult>(
                nameof(UpdateInventory),
                order.OrderItem);

            ShippingResult shippingResult = new(IsShippingAvailable: false, 0);

            if (inventoryResult.IsSufficientStock)
            {
                var taskOptions = new WorkflowTaskOptions(
                    new WorkflowRetryPolicy(
                        maxNumberOfAttempts: 5,
                        firstRetryInterval: TimeSpan.FromSeconds(1),
                        retryTimeout: TimeSpan.FromSeconds(2)));

                shippingResult = await context.CallActivityAsync<ShippingResult>(
                    nameof(ShippingCalculator),
                    order.ShippingInfo,
                    taskOptions);

            }

            return new OrderValidationResult(inventoryResult, shippingResult);
        }
    }
}