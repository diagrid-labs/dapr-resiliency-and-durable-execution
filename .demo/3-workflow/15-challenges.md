# Workflow challenges

- **Deterministic**; Multiple executions with the same input must result in the same output.
- **Idempotent**; Multiple executions do not result in negative side effects.
- **Workflow Versioning**; Be careful of breaking changes in the workflow.
- **Workflow & Activity Payloads**; Keep payloads small due to frequent (de)serialization.

[Source](https://medium.com/@cgillum/common-pitfalls-with-durable-execution-frameworks-like-durable-functions-or-temporal-eaf635d4a8bb)
