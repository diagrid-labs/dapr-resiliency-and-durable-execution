wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash -s 1.14.1
dapr uninstall
dapr init
dotnet build ResiliencyDemo/AppA/AppA.csproj
dotnet build ResiliencyDemo/AppA/AppB.csproj
dotnet build WorkflowDemo/ShippingApp/ShippingApp.csproj
dotnet build WorkflowDemo/WorkflowApp/WorkflowApp.csproj