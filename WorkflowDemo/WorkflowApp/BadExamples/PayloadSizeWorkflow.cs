using Dapr.Workflow;

namespace WorkflowApp
{
    public class LargePayloadSizeWorkflow : Workflow<string, LargeDocument>
    {
        public override async Task<LargeDocument> RunAsync(WorkflowContext context, string id)
        {
            var document = await context.CallActivityAsync<LargeDocument>(
                "GetDocument",
                id);

            var updatedDocument = await context.CallActivityAsync<LargeDocument>(
                "UpdateDocument",
                document);

            // More activities to process the updated document...

            return updatedDocument;
        }
    }


    public class SmallPayloadSizeWorkflow : Workflow<string, string>
    {
        public override async Task<string> RunAsync(WorkflowContext context, string id)
        {
            var documentId = await context.CallActivityAsync<string>(
                "GetAndUpdateDocument",
                id);

            // More activities to process the updated document...

            return documentId;
        }
    }

    public record LargeDocument(string Id, object Data);
}