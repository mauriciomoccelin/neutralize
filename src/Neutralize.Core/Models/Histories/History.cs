using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Neutralize.Models.Histories
{
    public class History : Entity<Guid>
    {
        public Guid? AggregateId { get; private set; }

        public string Hash { get; private set; }
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

        public History CalcHash()
        {
            var shManaged = SHA1.Create();
            var content = string.Join(Data, Id, @"^Mk+7Nd\N~{KG_aG");
            var bytes = Encoding.UTF8.GetBytes(content);
            var hash = shManaged.ComputeHash(bytes);

            Hash = string.Concat(hash.Select(b => b.ToString("x2")));
            return this;
        }

        public static class Factory
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
                var history = new History(
                    data, userId, command, default, creationTime, fullEntityName, aggregateId
                );

                return history.CalcRuntime().CalcHash();
            }
        }
    }
}
