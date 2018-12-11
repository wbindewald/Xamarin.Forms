using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Xamarin.Forms
{
	public sealed class ShellCollection<T> : IEnumerable<T>, IList<T>, INotifyCollectionChanged
	{
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged {
			add { ((INotifyCollectionChanged)Inner).CollectionChanged += value; }
			remove { ((INotifyCollectionChanged)Inner).CollectionChanged -= value; }
		}

		public int Count => Inner.Count;
		public bool IsReadOnly => Inner.IsReadOnly;
		internal IList<T> Inner { get; set; }

		public T this[int index]
		{
			get => Inner[index];
			set => Inner[index] = value;
		}

		public void Add(T item) => Inner.Add(item);

		public void Clear() => Inner.Clear();

		public bool Contains(T item) => Inner.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => Inner.CopyTo(array, arrayIndex);

		public IEnumerator<T> GetEnumerator() => Inner.GetEnumerator();

		public int IndexOf(T item) => Inner.IndexOf(item);

		public void Insert(int index, T item) => Inner.Insert(index, item);

		public bool Remove(T item) => Inner.Remove(item);

		public void RemoveAt(int index) => Inner.RemoveAt(index);

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inner).GetEnumerator();
	}
}