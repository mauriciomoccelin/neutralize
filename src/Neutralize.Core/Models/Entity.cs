namespace Neutralize.Models
{
    public abstract class Entity<TId> : IEntity<TId> where TId: struct 
    {
        public TId Id { get; protected set; }

        protected Entity() { }
        protected Entity(TId id) { Id = id; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<TId>;

            if (ReferenceEquals(this, compareTo)) return true;
            return !(compareTo is null) && Id.Equals(compareTo.Id);
        }

        public static bool operator ==(
            Entity<TId> a,
            Entity<TId> b
        )
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Equals(b);
        }
        
        public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);
        public override string ToString() => GetType().Name + " [Id=" + Id + "]";
        public override int GetHashCode() => (GetType().GetHashCode() * 96) + Id.GetHashCode();
    }
}
