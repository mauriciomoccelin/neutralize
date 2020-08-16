namespace Neutralize.Models
{
    public abstract class Entity : IEntity
    {
        public long Id { get; protected set; }

        protected Entity() { }
        protected Entity(long id) { Id = id; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            return !(compareTo is null) && Id.Equals(compareTo.Id);
        }

        public static bool operator ==(
            Entity a,
            Entity b
        )
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Equals(b);
        }
        
        public static bool operator !=(Entity a, Entity b) => !(a == b);
        public override string ToString() => GetType().Name + " [Id=" + Id + "]";
        public override int GetHashCode() => (GetType().GetHashCode() * 96) + Id.GetHashCode();
    }
}
