using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neutralize.Kafka.Consumers;
using Neutralize.Kafka.Productors;

namespace Neutralize.Kafka
{
    public static class KafkaAbstraction
    {
        /// <summary>
        /// /// Register for producer only
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafka(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var kafkaConfiguration = GetKafkaConfiguration(configuration);
            
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            services.AddSingleton<IReportDelivery, ReportDelivery>();
            services.AddSingleton<IKafkaConfiguration>(sp => kafkaConfiguration);

            return services;
        }

        /// <summary>
        /// Register for producer and consumer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="option"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafka(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IKafkaConfiguration> option,
            params Assembly[] args
        )
        {
            var kafkaConfiguration = GetKafkaConfiguration(configuration);
            
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            services.AddSingleton<IReportDelivery, ReportDelivery>();
            services.AddSingleton<IKafkaConfiguration>(sp => kafkaConfiguration);
            
            services.AddMediatR(args);
            option?.Invoke(kafkaConfiguration);
            services.AddHostedService<KafkaMonitorConsumerService>();

            return services;
        }

        private static KafkaConfiguration GetKafkaConfiguration(IConfiguration configuration)
        {
            var kafkaConfiguration = KafkaConfiguration.Create(
                configuration["kafka:group"],
                Convert.ToByte(configuration["kafka:flushTimeout"]),
                configuration["kafka:bootstrapServers"],
                configuration["kafka:topicFailureDelivery"],
                configuration["kafka:topicSuccessDelivery"]
            );
            return kafkaConfiguration;
        }
    }
}
