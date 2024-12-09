# Simulate issue with AppB 

Let's start AppA, but not AppB. Make a request to AppA, wait a couple of seconds and then start AppB.

1. Navigate to AppA folder:

   ```bash
   cd AppA
   ```

2. Run AppA with the Dapr CLI:

   ```bash
   dapr run --app-id app-a --app-port 5045 --dapr-http-port 3500 --resources-path "../Resources/" -- dotnet run
   ```

3. First make a request to AppA, then open a new terminal and navigate to AppB:

   ```bash
   cd AppB
   ```

4. Run AppB with the Dapr CLI:

   ```bash
   dapr run --app-id app-b --app-port 5047 --dapr-http-port 3502 --resources-path "../Resources/" -- dotnet run
   ```
