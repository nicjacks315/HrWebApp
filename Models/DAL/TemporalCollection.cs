using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS.HR.Models
{
    public class TemporalCollection<T> : ICollection<T> where T : TemporalEntity
    {
        private ICollection<T> _items;

        public List<T> Effective(DateTime asOfDate)
        {
            return this.Where(e => e.DateEffective <= asOfDate).OrderByDescending(e => e.DateEffective).ToList();
        }

        public List<T> Effective()
        {
            return Effective(DateTime.Now);
        }

        public List<T> EffectiveOrOldest(DateTime asOfDate)
        {
            var effective = Effective(asOfDate);
            if (effective.Count == 0)
            {
                return this.OrderBy(e => e.DateEffective).ToList();
            }
            return effective;
        }

        public List<T> EffectiveOrOldest()
        {
            return EffectiveOrOldest(DateTime.Now);
        }

        public TemporalCollection()
        {
            // Default to using a List<T>. <!----jk HashSet
            _items = new HashSet<T>();
        }

        protected TemporalCollection(ICollection<T> collection)
        {
            // Let derived classes specify the exact type of ICollection<T> to wrap.
            _items = collection;
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}