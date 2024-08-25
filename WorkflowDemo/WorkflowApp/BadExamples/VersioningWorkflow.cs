using Dapr.Workflow;

namespace WorkflowApp
{
    public class VersioningWorkflow1 : Workflow<string, int>
    {
        public override async Task<int> RunAsync(WorkflowContext context, string orderItem)
        {
            int resultA = await context.CallActivityAsync<int>(
                "ActivityA",
                orderItem);

            int resultB = await context.CallActivityAsync<int>(
                "ActivityB",
                orderItem);

            return resultA + resultB;
        }
    }

    public class VersioningWorkflow2 : Workflow<string, int>
    {
        public override async Task<int> RunAsync(WorkflowContext context, string orderItem)
        {
            int resultA = await context.CallActivityAsync<int>(
                "ActivityA",
                orderItem);

            // The input for this activity has changed from orderItem (string) to resultA (int).
            // If there are in-flight workflows that were started with the previous version
            // of this workflow, but have not yet completed, these will fail when they reach
            // this point since the parameters do not match with the persisted state.
            int resultB = await context.CallActivityAsync<int>(
                "ActivityB",
                resultA);

            return resultA + resultB;
        }
    }
}