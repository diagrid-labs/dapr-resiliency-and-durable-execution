# Dapr Resiliency and Durable Execution

This repo contains Dapr applications to demonstrate Dapr resiliency policies and durable execution with Dapr workflow.

> Running the CodeTours in this repo is recommended since this gives more context about:
>
> - failure and resiliency in general
> - the way Dapr provides resiliency
> - what durable execution is
> - how the Dapr Workflow ensures durable execution

**TOC**

- [Running the Failure & Resiliency CodeTour](#running-the-failure--resiliency-codetour)
- [ResiliencyDemo](#resiliencydemo)
- [Running the Durable Execution & Workflow CodeTour](#running-the-durable-execution--workflow-codetour)
- [WorkflowDemo](#workflowdemo)

## Prerequisites

Ensure you have these installed on your machine:

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Clone the [dapr-resiliency-and-durable-execution repo](https://github.com/diagrid-labs/dapr-resiliency-and-durable-execution) to your local machine.
- [VSCode](https://code.visualstudio.com/) Although other IDEs can be used to view the code, some VSCode specific extensions are used (such as [CodeTour](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour)) to help guide you through the codebase.

Open the cloned repo in VSCode and accept the suggested VSCode extensions.

## Running the Failure & Resiliency CodeTour

Using the CodeTour panel in the VSCode explorer, start the *2 - Failure & Resiliency* CodeTour:

![CodeTour Failure & Resiliency](./images/codetour-failure-resiliency.png)

### ResiliencyDemo

The ResiliencyDemo consists of two applications, AppA and AppB, and a state store.

Communication between AppA and AppB can be done using HTTP or Pub/Sub.

**Service invocation**

```mermaid
graph LR
    A{{AppA}}
    B{{AppB}}
    State[(KV Store)]
    A --HTTP--> B
    B --> State
```

**Pub/sub**

```mermaid
graph LR
    A{{AppA}}
    B{{AppB}}
    MB[Message Broker]
    State[(KV Store)]
    A .-> MB .-> B
    B --> State
```

### Running the ResiliencyDemo apps locally

1. Navigate to the ResiliencyDemo folder in the terminal:

    ```bash
    cd ResiliencyDemo
    ```

2. Run the ResiliencyDemo apps using the Dapr CLI:

    ```bash
    dapr run -f .
    ```

3. Open the [local.http](./ResiliencyDemo/local.http) file in the VSCode editor and execute the HTTP requests to the ResiliencyDemo apps.

## Running the Durable Execution & Workflow CodeTour

Using the CodeTour panel in the VSCode explorer, start the *3 - Durable Execution & Workflow* CodeTour:

![CodeTour Durable Execution & Workflow](./images/codetour-durable-execution.png)

### WorkflowDemo

**ValidateOrderWorkflow**

```mermaid
graph LR
    Start((Start))
    subgraph Data Store
        KV[(KV Store)]
    end
    subgraph Shipping App
        App{{ShippingApp}}
    end
    subgraph ValidateOrderWorkflow
        A1(UpdateInventory)
        Stock{Sufficient\nStock?}
        A2(ShippingCalculator)
        subgraph Compensation
            ShipIssue{ShippingCalculator\nissue?}
            A3(UndoUpdateInventory)
        end
    end
    End((End))

    Start -- Order --> A1
    A1 -- 2 --> Stock
    A1 -- 1 --> KV
    Stock -- Yes --> A2
    Stock -- No --> End
    A2 -- 1 HTTP --> App
    A2 -- 2 --> ShipIssue
    ShipIssue -- Yes --> A3
    ShipIssue -- No --> End
    A3 -- 1 --> KV
    A3 -- 2 --> End
```

### Running the WorkflowDemo locally

1. Navigate to the WorkflowDemo folder in the terminal:

    ```bash
    cd WorkflowDemo
    ```

2. Run the WorkflowDemo apps using the Dapr CLI:

    ```bash
    dapr run -f .
    ```

3. Open the [local.http](./WorkflowDemo/local.http) file in the VSCode editor and execute the HTTP requests to the WorkflowDemo apps.