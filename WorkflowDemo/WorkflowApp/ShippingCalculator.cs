using Dapr.Workflow;

namespace WorkflowApp
{
    public class ShippingCalculator : WorkflowActivity<ShippingInfo, ShippingResult>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShippingCalculator> _logger;
        public ShippingCalculator(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<ShippingCalculator>();
        }

        public override async Task<ShippingResult> RunAsync(WorkflowActivityContext context, ShippingInfo shippingInfo)
        {
            var response = await _httpClient.PostAsJsonAsync("/calculateCost", shippingInfo);
            var result = await response.Content.ReadFromJsonAsync<ShippingResult>();

            _logger.LogInformation("Calculated shipping cost for country {Country}.", shippingInfo.Country);

            return result;
        }
    }
}