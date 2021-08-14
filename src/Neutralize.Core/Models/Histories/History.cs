using System;

namespace Neutralize.Models.Histories
{
    public class History : Entity<Guid>
    {
        public Guid? AggregateId { get; private set; }

        public string Data { get; private set; }
        public long UserId { get; private set; }
        public string Command { get; private set; }
        public TimeSpan Runtime { get; private set; }
        public DateTime CreateOn { get; private set; }
        public string Entity { get; private set; }

        private History() { }

        private History(
            string data,
            long userId,
            string command,
            TimeSpan runtime,
            DateTime createOn,
            string entity,
            Guid? aggregateId
        )
        {
            UserId = userId;
            Runtime = runtime;
            CreateOn = createOn;
            Data = data ?? string.Empty;
            Command = command ?? string.Empty;
            Entity = entity ?? string.Empty;
            AggregateId = aggregateId;
        }

        public History CalcRuntime()
        {
            Runtime = (DateTime.Now - CreateOn);
            return this;
        }

        public class Factory
        {
            public static History Criar(
                string data,
                long userId,
                string command,
                DateTime creationTime,
                string fullEntityName,
                Guid? aggregateId = null
            )
            {
                return new History(
                    data, userId, command, default, creationTime, fullEntityName, aggregateId
                ).CalcRuntime();
            }
        }
    }
}
