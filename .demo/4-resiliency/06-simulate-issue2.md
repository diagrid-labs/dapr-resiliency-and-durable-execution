# Simulate an issue with the state store 

Let's start both apps, but make the state store temporarily unavailable.

1. Navigate to the ResiliencyDemo folder and run AppA and AppB with multi-app run:

   ```bash
   dapr run -f .
   ```

2. Open a new terminal and stop the `dapr_redis` container:

 ```bash
 docker stop dapr_redis
 ```

3. Make a new request to AppA via the `local.http` file.
4. Wait a few seconds and start the `dapr_redis` container:

   ```bash
   docker start dapr_redis
   ```
