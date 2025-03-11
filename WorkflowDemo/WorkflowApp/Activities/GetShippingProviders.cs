using Dapr.Workflow;

namespace WorkflowApp
{
    public class GetShippingProviders : WorkflowActivity<GetShippingProvidersRequest, GetShippingProvidersResult>
    {
        private readonly ILogger<GetShippingProviders> _logger;
        public GetShippingProviders(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetShippingProviders>();
        }

        public override async Task<GetShippingProvidersResult> RunAsync(WorkflowActivityContext context, GetShippingProvidersRequest request)
        {
            _logger.LogInformation("Retrieving shipping services for order {Order}.", request.Order.Id);

            return await Task.FromResult(new GetShippingProvidersResult(
            [
                "Shipping Service A",
                "Shipping Service B",
                "Shipping Service C"
            ]));
        }
    }
}