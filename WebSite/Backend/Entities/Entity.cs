using System;

namespace RetroShark.Application.Backend.Entities
{
    [Serializable]
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>> where TKey : IComparable
    {
        public virtual TKey Id { get; set; }

        public virtual bool Equals(Entity<TKey> comparableObj)
        {
            if (comparableObj == null)
            {
                return false;
            }

            return Id.CompareTo(comparableObj.Id) == 0;
        }

        public override bool Equals(object comparableObj)
        {
            return Equals(comparableObj as Entity<TKey>);
        }

        public override int GetHashCode()
        {
            return Cinchcast.Framework.HashHelper.GetHashCode(Id);
        }
    }
}
