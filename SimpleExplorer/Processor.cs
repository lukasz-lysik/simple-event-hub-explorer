using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Diagnostics;

namespace SimpleExplorer
{
    public class Processor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                var sb = new StringBuilder();
                sb.AppendLine($"Partition Id: {context.PartitionId}");
                sb.AppendLine("Properties:");
                foreach(var p in eventData.Properties)
                {
                    sb.AppendLine($"  {p.Key}: {p.Value}");
                }
                sb.AppendLine($"Message: {data}");
                Trace.TraceInformation(sb.ToString());
            }

            return context.CheckpointAsync();
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error) 
        {
            Trace.TraceError(error.Message);
            return Task.CompletedTask;
        }
    }
}
