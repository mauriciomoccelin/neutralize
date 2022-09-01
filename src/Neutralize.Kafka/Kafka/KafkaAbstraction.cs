using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Neutralize.Kafka
{
    public static class KafkaAbstraction
    {
        /// <summary>
        /// /// Register factory for producer and consumer only
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafka(
            this IServiceCollection services,
            Action<IKafkaConfiguration> options
        )
        {
            var kafkaConfiguration = KafkaConfiguration.Create();

            AddKafkaFactory(services, kafkaConfiguration);

            options?.Invoke(kafkaConfiguration);

            return services;
        }

        /// <summary>
        /// Register factory for producer, consumer and monitor consumer handlers 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafka(
            this IServiceCollection services,
            Action<IKafkaConfiguration> options,
            params Assembly[] args
        )
        {
            var kafkaConfiguration = KafkaConfiguration.Create(); ;

            AddKafkaFactory(services, kafkaConfiguration);

            services.AddMediatR(args);
            options?.Invoke(kafkaConfiguration);
            services.AddHostedService<KafkaMonitorConsumerService>();

            return services;
        }

        private static void AddKafkaFactory(IServiceCollection services, KafkaConfiguration kafkaConfiguration)
        {
            services.AddSingleton<IKafkaFactory, KafkaFactory>();
            services.AddSingleton<IKafkaConfiguration>(sp => kafkaConfiguration);
        }
    }
}
