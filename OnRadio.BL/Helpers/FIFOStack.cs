using System.Collections.Generic;
using System.Diagnostics;
using OnRadio.BL.Models;

namespace OnRadio.BL.Helpers
{
    public class FIFOStack<T> 
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Capacity { get; set; } = 5;// save only last few radios

        public void Push(T item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }

            if (Items.Count >= Capacity)
            {
                Items.RemoveAt(Items.Count - 1);
            }

            Items.Insert(0, item);
        }


        /// <summary>
        /// Return the most recent radio from the history
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (Items.Count > 0)
            {
                return Items[Items.Count - 1];
            }
            else
                return default(T);
        }

        /// <summary>
        /// Returns the list of all radios from the most recent one.
        /// </summary>
        /// <returns></returns>
        public List<T> Get()
        {
            return Items;
        }

        public void DebugPrint()
        {
            foreach (var item in Items)
            {
                Debug.WriteLine(item.ToString());
            }
        }
    }
}
