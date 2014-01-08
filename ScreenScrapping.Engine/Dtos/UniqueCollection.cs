using System.Collections;
using System.Collections.Generic;

namespace ScreenScrapping.Engine.Dtos
{
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

        public void Add(T item)
        {
            T eligibleItem;
            if (!Contains(item, out eligibleItem))
            {
                _items.Add(eligibleItem);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool Contains(T item)
        {
            T eligibleItem;
            return Contains(item, out eligibleItem);
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

        protected virtual T ConvertToEligibleFormat(T originalItem)
        {
            return originalItem;
        }
    }
}