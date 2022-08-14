using System;
using MediatR;

namespace Neutralize.Kafka.Test;

public class Notification_Fake : INotification
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public class InvalidNotification_Fake
{
    public Guid Id { get; set; } = Guid.NewGuid();
}