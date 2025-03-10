using Dapr.Workflow;

namespace WorkflowApp
{
    public class GetShippingServices : WorkflowActivity<GetShippingServicesRequest, GetShippingServicesResult>
    {
        private readonly ILogger<GetShippingServices> _logger;
        public GetShippingServices(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetShippingServices>();
        }

        public override async Task<GetShippingServicesResult> RunAsync(WorkflowActivityContext context, GetShippingServicesRequest request)
        {
            _logger.LogInformation("Retrieving shipping services for order {Order}.", request.Order.Id);

            return await Task.FromResult(new GetShippingServicesResult(
            [
                "Shipping Service A",
                "Shipping Service B",
                "Shipping Service C"
            ]));
        }
    }
}