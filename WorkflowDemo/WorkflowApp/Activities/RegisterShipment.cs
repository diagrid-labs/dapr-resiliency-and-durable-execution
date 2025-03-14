using System.Net;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class RegisterShipment : WorkflowActivity<RegisterShipmentRequest, RegisterShipmentResult>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetShippingCost> _logger;
        public RegisterShipment(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<GetShippingCost>();
        }

        public override async Task<RegisterShipmentResult> RunAsync(WorkflowActivityContext context, RegisterShipmentRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/registerShipment", request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Failed to register shipment for order {Order} {Message}.", request.Order.Id, response.ReasonPhrase);
                throw new Exception($"Failed to register shipment. Reason: {response.ReasonPhrase}.");
            }
            var result = await response.Content.ReadFromJsonAsync<RegisterShipmentResult>();

            _logger.LogInformation("{ShippingService}: Registered shipment for order {Order}.", request.ShippingService, request.Order.Id);

            return result;
        }
    }
}