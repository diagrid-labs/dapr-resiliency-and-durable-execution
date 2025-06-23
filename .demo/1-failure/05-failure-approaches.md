# We need different approaches to handle different types of failures

**Transient failures**

- **[Retries](https://learn.microsoft.com/azure/architecture/patterns/retry)**: If a service is failing temporarily, try the operation again.

**Permanent failures**

- **Timeouts**: If a service is taking too long to respond, it's better to fail fast and return an error.
- **[Circuit breakers](https://learn.microsoft.com/azure/architecture/patterns/circuit-breaker)**: If a service is failing consistently, it's better to stop calling it, and occasionally try it again to check if it's back.
