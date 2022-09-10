using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSubstitute;

namespace Neutralize.Kafka.Test;

public class KafkaFixture : IDisposable
{
    public AutoMocker Mocker { get; set; }

    public KafkaFixture()
    {
        Mocker = new AutoMocker();
    }

    public IConsumer<Ignore, string> GenereteKafkaConsumer()
    {
        return Substitute.For<IConsumer<Ignore, string>>();
    }

    public IKafkaMonitorConsumerService GenereteKafkaMonitorConsumerService()
    {
        Mocker = new AutoMocker();
        return Mocker.CreateInstance<KafkaMonitorConsumerService>();
    }

    public IServiceCollection GenereteServiceCollection()
    {
        return new ServiceCollection();
    }

    public ConsumeResult<Ignore, string> GenereteConsumeResult<T>(T model)
    {
        return new ConsumeResult<Ignore, string>
        {
            Offset = 0,
            Partition = 0,
            Topic = typeof(T).Name,
            Message = new Message<Ignore, string>
            {
                Value = JsonConvert.SerializeObject(
                    model,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.None,
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                    }
                )
            }
        };
    }

    public IDictionary<string, Type> GenereteHandlers<T>()
    {
        Type type = typeof(T);
        var dictonary = new Dictionary<string, Type>();

        dictonary.Add(type.Name, type);

        return dictonary;
    }


    public void Dispose()
    {
    }
}
