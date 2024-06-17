using System.Net.Http.Json;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class ShippingCalculator : WorkflowActivity<ShippingInfo, ShippingResult>
    {
        private readonly HttpClient _httpClient;
        public ShippingCalculator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<ShippingResult> RunAsync(WorkflowActivityContext context, ShippingInfo shippingInfo)
        {
            var response = await _httpClient.PostAsJsonAsync("/calculatecost", shippingInfo);
            var result = await response.Content.ReadFromJsonAsync<ShippingResult>();
            return result;
        }
    }
}