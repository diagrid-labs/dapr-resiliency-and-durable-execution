namespace WorkflowApp
{
    public record Order(string Id, OrderItem OrderItem, ShippingInfo ShippingInfo);
    public record OrderItem(string ProductId, string ProductName, int Quantity, decimal TotalPrice);
    public record ShippingInfo(string Country);
    public record OrderValidationResult(InventoryResult InventoryResult, ShippingResult ShippingResult);
    public record ShippingResult(bool IsShippingAvailable, decimal Cost);
    public record InventoryResult(bool IsKnownProduct, bool InStock);
    public record ProductInventory(string ProductId, int Quantity);
}