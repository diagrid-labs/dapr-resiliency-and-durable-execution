dapr uninstall
dapr init
dotnet build ResiliencyDemo/AppA/AppA.csproj
dotnet build ResiliencyDemo/AppB/AppB.csproj
dotnet build WorkflowDemo/ShippingApp/ShippingApp.csproj
dotnet build WorkflowDemo/WorkflowApp/WorkflowApp.csproj