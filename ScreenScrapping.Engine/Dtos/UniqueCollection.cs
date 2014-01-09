using System.Collections;
using System.Collections.Generic;

namespace ScreenScrapping.Engine.Dtos
{
    /// <summary>
    /// provides generic collection of unique items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UniqueCollection<T> : IEnumerable<T>
    {
        private readonly IList<T> _items;

        protected IEnumerable<T> Items
        {
            get { return _items; }
        }

        public UniqueCollection()
        {
            _items = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(T item)
        {
            T eligibleItem;
            if (!Contains(item, out eligibleItem))
            {
                _items.Add(eligibleItem);
            }
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public virtual bool Contains(T item)
        {
            T eligibleItem;
            return Contains(item, out eligibleItem);
        }

        protected virtual T ConvertToEligibleFormat(T originalItem)
        {
            return originalItem;
        }

        private bool Contains(T originalItem, out T eligibleItem)
        {
            eligibleItem = default(T);

            var convertedItem = ConvertToEligibleFormat(originalItem);
            if (!_items.Contains(convertedItem))
            {
                eligibleItem = convertedItem;
                return false;
            }
            return true;
        }        
    }    
}