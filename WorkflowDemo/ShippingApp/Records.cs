public record Order(string Id, OrderItem OrderItem, ContactInfo ContactInfo);
public record OrderItem(string ProductId, string ProductName, int Quantity, decimal TotalPrice);
public record ContactInfo(string Name, string Country);
public record ShippingCostRequest(string ShippingService, Order Order);
public record ShippingCostResult(string ShippingService, bool IsShippingAvailable, decimal Cost);
public record RegisterShipmentRequest(Order Order, string ShippingService);
public record RegisterShipmentResult(string ShippingService, string Id, bool IsRegistered);