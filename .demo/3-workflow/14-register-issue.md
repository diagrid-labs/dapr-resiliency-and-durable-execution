# Register shipment issue

Simulate an issue with the `registerShipment` endpoint:

1. Comment out the `return Results.Ok(shipmentResult)` line.

2. Uncomment the `return Results.StatusCode(500)` line.

3. Ensure that you're in the WorkflowDemo folder and run the applications via the Dapr CLI: `dapr run -f .`.
