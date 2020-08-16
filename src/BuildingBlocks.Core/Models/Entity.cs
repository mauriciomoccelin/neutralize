﻿using System;

namespace BuildingBlocks.Models
{
    public abstract class Entity<TEntity, TId>
        where TEntity : class
        where TId : struct
    {
        private static TEntity empty;
        public TId Id { get; set; }

        protected Entity() { }
        protected Entity(TId id) { Id = id; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<TEntity, TId>;

            if (ReferenceEquals(this, compareTo)) return true;
            return !(compareTo is null) && Id.Equals(compareTo.Id);
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

        public bool IsEmpty() => Equals(empty);
        public override string ToString() => GetType().Name + " [Id=" + Id + "]";
        public override int GetHashCode() => (GetType().GetHashCode() * 96) + Id.GetHashCode();
        public static bool operator !=(Entity<TEntity, TId> a, Entity<TEntity, TId> b) => !(a == b);
        public static TEntity Empty() => empty ??= Activator.CreateInstance(typeof(TEntity), true) as TEntity;
    }
}