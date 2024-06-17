using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class ValidateOrderWorkflow : Workflow<Order, OrderValidationResult>
    {
        public override async Task<OrderValidationResult> RunAsync(WorkflowContext context, Order order)
        {
            var inventoryResult = await context.CallActivityAsync<InventoryResult>(
                nameof(CheckInventory),
                order.OrderItem);
            
            var shippingResult = await context.CallActivityAsync<ShippingResult>(
                nameof(ShippingCalculator),
                order.ShippingInfo);
            
            return new OrderValidationResult(inventoryResult, shippingResult);
        }
    }
}