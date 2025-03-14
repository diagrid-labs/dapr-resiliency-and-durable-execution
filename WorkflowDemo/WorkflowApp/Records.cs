public record Order(string Id, OrderItem OrderItem, ContactInfo ContactInfo);
public record OrderItem(string ProductId, string ProductName, int Quantity, decimal TotalPrice);
public record ContactInfo(string Name, string Country);
public record OrderValidationResult(InventoryResult InventoryResult, RegisterShipmentResult? RegisterShipmentResult);
public record GetShippingProvidersRequest(Order Order);
public record GetShippingProvidersResult(string[] ShippingProviders);
public record ShippingCostRequest(string ShippingService, Order Order);
public record ShippingCostResult(string ShippingService, bool IsShippingAvailable, decimal Cost);
public record RegisterShipmentRequest(Order Order, string ShippingService);
public record RegisterShipmentResult(string ShippingService, string Id, bool IsRegistered);
public record InventoryResult(bool IsKnownProduct, bool IsSufficientStock);
public record ProductInventory(string ProductId, int Quantity);