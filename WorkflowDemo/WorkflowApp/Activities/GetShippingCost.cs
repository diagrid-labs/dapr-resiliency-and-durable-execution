using Dapr.Workflow;

namespace WorkflowApp
{
    public class GetShippingCost : WorkflowActivity<ShippingCostRequest, ShippingCostResult>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetShippingCost> _logger;
        public GetShippingCost(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<GetShippingCost>();
        }

        public override async Task<ShippingCostResult> RunAsync(WorkflowActivityContext context, ShippingCostRequest shippingRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/calculateCost", shippingRequest);
            var result = await response.Content.ReadFromJsonAsync<ShippingCostResult>();

            _logger.LogInformation("{ShippingService}: Cost is {Cost} for order {Order}.", result.ShippingService, result.Cost, shippingRequest.Order.Id);

            return result;
        }
    }
}