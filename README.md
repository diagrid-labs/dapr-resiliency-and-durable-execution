# Dapr Resiliency and Durable Execution

This repo contains Dapr applications to demonstrate Dapr resiliency policies and durable execution with Dapr workflow.

> Running the CodeTours in this repo is recommended since this gives more context about:
>
> - failure and resiliency in general
> - the way Dapr provides resiliency
> - what durable execution is
> - how the Dapr Workflow ensures durable execution

**TOC**

- [Running the Dapr Resiliency CodeTour](#running-the-dapr-resiliency-codetour)
- [ResiliencyDemo](#resiliencydemo)
- [Dapr Reliability Advisor in Conductor Free](#dapr-reliability-advisor-in-conductor-free)
- [Running the Durable Execution & Workflow CodeTour](#running-the-durable-execution--workflow-codetour)
- [WorkflowDemo](#workflowdemo)
- [Resources](#resources)

## Prerequisites

Ensure you have these installed on your machine:

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [VSCode](https://code.visualstudio.com/) - Although other IDEs can be used to view the code, some VSCode specific extensions are used (such as [CodeTour](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour)) to help guide you through the codebase.

Clone the [dapr-resiliency-and-durable-execution repo](https://github.com/diagrid-labs/dapr-resiliency-and-durable-execution) to your local machine.

Open the cloned repo in VSCode and accept the suggested VSCode extensions.

## Running the Dapr Resiliency CodeTour

Using the CodeTour panel in the VSCode explorer, start the *3 - Dapr Resiliency* CodeTour:

![CodeTour Failure & Resiliency](./images/codetour-dapr-resiliency.png)

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

### Dapr Reliability Advisor in Conductor Free

[Diagrid Conductor Free](https://www.diagrid.io/conductor) is a free tool developers can use to visualize, troubleshoot, and optimize Dapr workloads on Kubernetes. It includes an Advisor that provides recommendations on how to improve the reliability of your Dapr applications.

![Conductor Advisor](./images/conductor1.png)

![Reliability details](./images/conductor2.png)

## Running the Durable Execution & Workflow CodeTour

Using the CodeTour panel in the VSCode explorer, start the *4 - Durable Execution & Workflow* CodeTour:

![CodeTour Durable Execution & Workflow](./images/codetour-durable-execution.png)

### WorkflowDemo

```mermaid
graph LR
    U[User]
    KV[(KV Store)]
    WApp{{WorkflowApp}}
    SApp{{ShippingApp}}
    U --> WApp
    WApp --> SApp
    WApp --> KV
```

```mermaid
sequenceDiagram
    actor U as User
    participant DC as DaprClient
    participant W as ValidateOrderWorkflow
    participant UI as UpdateInventory
    participant GSC as GetShippingCost
    participant RS as RegisterShipment
    participant UU as UndoUpdateInventory
    participant KV as KV Store
    U ->> DC: ScheduleNewWorkflowAsync
    DC ->> W: Schedules instance
    DC -->> U: Accepted
    W ->> UI: CallActivity
    UI ->> KV: GetState
    KV -->> UI: Response
    UI ->> KV: SaveState
    UI -->> W: Response
    alt IsSuffientStock
        loop All Shipper Services
            W ->> GSC: CallActivity
            GSC -->> W: Response
        end
        W ->> W: GetCheapestShipper
        W ->> RS: CallActivity
        RS -->> W: Response
        alt Exception from RegisterShipment
            W ->> UU: CallActivity
            UU ->> KV: GetState
            UU ->> KV: UpdateState
            UU ->> W: Response
        end
    end
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

## Resources

- [Diagrid blog: An in-depth guide to Dapr workflow patterns in .NET](https://www.diagrid.io/blog/in-depth-guide-to-dapr-workflow-patterns)
- [Diagrid Conductor Free](https://www.diagrid.io/conductor)
- [Dapr Docs: Workflow](https://docs.dapr.io/developing-applications/building-blocks/workflow/workflow-overview/)
- [Dapr Docs: Resiliency](https://docs.dapr.io/operations/resiliency/)