# Simulate process outage

Simulate a process outage during the execution of the workflow process.

1. Ensure you're in the WorkflowDemo folder.
2. Run the demo with Dapr multi-app run:
  `dapr run -f .`
3. Use the [local.http](../../WorkflowDemo/local.http) file to restock the in-memory inventory.
4. Use the same http files to start an instance of the workflow. Once started, stop the Dapr run process (CTRL+C in the terminal) within a few seconds. This stops both Workflow and Shipping apps from running.
5. Restart the Workflow and Shipping apps with the Dapr CLI: `dapr run -f .`
6. The workflow should pickup where it was and complete now.