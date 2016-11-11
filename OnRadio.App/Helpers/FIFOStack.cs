using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OnRadio.App.Helpers
{
    public class FIFOStack<T>
    {
        public List<T> _items = new List<T>();
        private int _capacity;  // save only last few radios

        public void SetCapacity(int capacity)
        {
            _capacity = capacity;
        }

        public void Push(T item)
        {
            if (_items.Count > 0 && _items.Contains(item))
            {
                _items.Remove(item);
            }
            if (_items.Count >= 5)
            {
                _items.RemoveAt(0);
            }
            _items.Add(item);
        }

        // Return the most recent radio from the history
        public T Peek()
        {
            if (_items.Count > 0)
            {
                return _items[_items.Count - 1];
            }
            else
                return default(T);
        }

        // Returns the list of all radios from the most recent one.
        public List<T> GetFromFirstToLastRecent()
        {
            if (_items.Count > 0)
            {
                return _items;
            }
            else
                return null;
        }

        public void DebugPrint()
        {
            foreach (var item in _items)
            {
                Debug.WriteLine(item.ToString());
            }
        }
    }
}
