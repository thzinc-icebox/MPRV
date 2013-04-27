using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace MPRV.Model
{
	public class LazyList<TElement> : ICollection<TElement>
	{
		#region Constructors

		public LazyList()
			: this(Enumerable.Empty<TElement>())
		{
		}

		public LazyList(IEnumerable<TElement> source)
		{
			_backingCollection = new Lazy<List<TElement>>(() => new List<TElement>(source));
		}

		public LazyList(IEnumerable source)
			: this(source.Cast<TElement>())
		{
		}

		#endregion

		#region Events

		public event Action<TElement> OnAdd;

		public event Action<TElement> OnRemove;

		public event Action OnClear;

		#endregion

		#region ICollection implementation

		public void Add(TElement item)
		{
			if (OnAdd != null)
			{
				OnAdd(item);
			}

			_backingCollection.Value.Add(item);
		}

		public void Clear()
		{
			if (OnClear != null)
			{
				OnClear();
			}

			_backingCollection.Value.Clear();
		}

		public bool Contains(TElement item)
		{
			return _backingCollection.Value.Contains(item);
		}

		public void CopyTo(TElement[] array, int arrayIndex)
		{
			_backingCollection.Value.CopyTo(array, arrayIndex);
		}

		public bool Remove(TElement item)
		{
			if (OnRemove != null)
			{
				OnRemove(item);
			}

			return _backingCollection.Value.Remove(item);
		}

		public int Count
		{
			get
			{
				return _backingCollection.Value.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<TElement> GetEnumerator()
		{
			return _backingCollection.Value.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Protected Properties

		protected Lazy<List<TElement>> _backingCollection {get;set;}

		#endregion
	}
}

