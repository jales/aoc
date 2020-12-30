using System.Diagnostics.CodeAnalysis;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public class PriorityQueue<T> : IReadOnlyCollection<T>
    {
        private readonly LinkedList<T> _items = new();
        private readonly Comparer<T> _comparer;

        public PriorityQueue() : this(Comparer<T>.Default)
        {
        }

        public PriorityQueue(Comparison<T> comparer)
        {
            _comparer = Comparer<T>.Create(comparer);
        }

        public PriorityQueue(Comparer<T> comparer)
        {
            _comparer = comparer;
        }

        public void Add(T item)
        {
            Enqueue(item);
        }

        public void Enqueue(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var node = _items.First;

            while (node != null)
            {
                if (_comparer.Compare(item, node.Value) <= 0)
                {
                    _items.AddBefore(node, item);
                    return;
                }

                node = node.Next;
            }

            _items.AddLast(item);
        }

        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue empty.");
            }

            var removed = _items.First!.Value;

            _items.RemoveFirst();

            return removed;
        }

        public bool TryDequeue([MaybeNullWhen(false)] out T result)
        {
            if (Count == 0)
            {
                result = default!;
                return false;
            }

            result = _items.First!.Value;

            _items.RemoveFirst();

            return true;
        }

        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue empty.");
            }

            return _items.First!.Value;
        }

        public bool TryPeek([MaybeNullWhen(false)] out T result)
        {
            if (Count == 0)
            {
                result = default!;
                return false;
            }

            result = _items.First!.Value;
            return true;
        }


        public int Count => _items.Count;

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public void Clear() => _items.Clear();

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public bool Contains(T item) => _items.Contains(item);

        public T[] ToArray() => _items.ToArray();

    }
}
