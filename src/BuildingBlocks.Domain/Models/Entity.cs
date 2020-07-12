using System;

namespace BuildingBlocks.Domain.Models
{
    public abstract class Entity<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        private static TEntity empty;
        public TId Id { get; protected set; }

        protected Entity() { }
        protected Entity(TId id) { Id = id; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<TEntity, TId>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(
            Entity<TEntity, TId> a,
            Entity<TEntity, TId> b
        )
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TEntity, TId> a, Entity<TEntity, TId> b) => !(a == b);

        public override int GetHashCode() => (GetType().GetHashCode() * 96) + Id.GetHashCode();

        public override string ToString() => GetType().Name + " [Id=" + Id + "]";

        public static TEntity Empty() => empty = empty ?? Activator.CreateInstance(typeof(TEntity), true) as TEntity;

        public bool IsEmpty() => Equals(empty);
    }
}
