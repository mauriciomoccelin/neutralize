﻿using System;
using System.Collections.Generic;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neutralize.Kafka.Consumers;
using Neutralize.Kafka.Productors;

namespace Neutralize.Kafka
{
    public static class KafkaRegister
    {
        public static IServiceCollection AddKafka(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IKafkaConfiguration> option
        )
        {
            var kafkaConfiguration = KafkaConfiguration.Create(
                configuration["kafka:group"],
                Convert.ToByte(configuration["kafka:flushTimeout"]),
                configuration["kafka:bootstrapServers"],
                configuration["kafka:topicFailureDelivery"],
                configuration["kafka:topicSuccessDelivery"]
            );

            option.Invoke(kafkaConfiguration);

            services.AddSingleton<IKafkaConfiguration>(sp => kafkaConfiguration);

            services.AddMediatR(Assembly.Load("Neutralize.Kafka"));
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            services.AddSingleton<IReportDelivery, ReportDelivery>();

            services.AddHostedService<KafkaMonitorConsumerService>();
            return services;
        }
    }
}
