using System;

namespace BuildingBlocks.Domain
{
    public class DomainNotification : Event
    {
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

        private DomainNotification() { }

        public static DomainNotification Create(string key, string value) =>
            new DomainNotification()
            {
                DomainNotificationId = Guid.NewGuid(),
                Version = 1,
                Key = key,
                Value = value
            };
    }
}
