using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Workflow;

namespace WorkflowApp
{
    public class OrderWorkflow : Workflow<OrderItem, OrderResult>
    {
        public override Task<string> RunAsync(WorkflowContext context, OrderItem orderItem)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderResult
    {
    }

    public class OrderItem
    {
    }
}