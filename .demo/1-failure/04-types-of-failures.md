# Not all failures are the same

## Transient failures

Transient failures are temporary and usually resolve themselves without human intervention.

## Permanent failures

Permanent failures are the opposite of transient failures and they do require human intervention to be resolved.

## We need different approaches to handle different types of failures

- [Retries](https://learn.microsoft.com/azure/architecture/patterns/retry) are useful to handle transient failures.
- [Circuit breakers](https://learn.microsoft.com/azure/architecture/patterns/circuit-breaker) are useful to handle permanent failures.
