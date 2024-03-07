using System;
using System.Collections.Generic;
using System.Threading;

namespace BinanceOptionsApp.Helpers
{
    public class ThreadSafeQueue<T> : IDisposable where T : class
    {
        private readonly List<T> list = new List<T>();
        private AutoResetEvent filledEvent = new AutoResetEvent(false);
        public void Clear()
        {
            lock (list)
            {
                list.Clear();
            }
        }
        public void Enqueue(T item)
        {
            lock (list)
            {
                list.Add(item);
            }
            filledEvent.Set();
        }
        public void Enqueue(IEnumerable<T> items)
        {
            if (items != null)
            {
                lock (list)
                {
                    list.AddRange(items);
                }
                filledEvent.Set();
            }
        }
        public T[] Dequeue(int msTimeout)
        {
            filledEvent.WaitOne(msTimeout);
            lock (list)
            {
                var result = list.ToArray();
                list.Clear();
                return result;
            }
        }
        public void Dispose()
        {
            if (filledEvent != null)
            {
                filledEvent.Dispose();
                filledEvent = null;
            }
            GC.SuppressFinalize(this);
        }
    }

}
