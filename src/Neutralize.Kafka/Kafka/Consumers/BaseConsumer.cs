using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Neutralize.Kafka.Consumers
{
    public abstract class BaseConsumer : IDisposable
    {
        static ConsumerConfig conf = new ConsumerConfig
        { 
            GroupId = "life",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        IDictionary<string, Type> handlers = new Dictionary<string, Type>();
        

        protected Task ConsumerWrapper(string topic, string groupId)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                consumer.Subscribe(topic);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

                try
                {
                    while (!cts.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cts.Token);
                            var type = handlers.First(k => k.Key.Equals(consumeResult.Topic));
                            var result = JsonConvert.DeserializeObject(consumeResult.Message.Value, type.Value);
                            
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }

                    return Task.CompletedTask;
                }
                catch (OperationCanceledException exception)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("KAFKA: Consumer Error:");
                    Console.ResetColor();
                    Console.WriteLine(exception.Message);
                    
                    consumer.Close();
                    return Task.CompletedTask;
                }
            }
        }
        
        public void Dispose() => GC.SuppressFinalize(this);
    }
}