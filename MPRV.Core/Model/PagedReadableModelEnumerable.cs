using System;
using MPRV.Common;
using System.Collections.Generic;
using System.Collections;

namespace MPRV.Model
{
	public delegate IEnumerable<ReadableModelPopulator> PagedReadableModelEnumerableSelector(long page, long perPage, out long totalItems, out long totalPages);
	public class PagedReadableModelEnumerable<TReadableModel> : IPagedEnumerable<TReadableModel>
		where TReadableModel : IReadableModel, new()
	{
		#region Subclasses
		private class Enumerator : IEnumerator<TReadableModel>
		{
			#region Constructors
			public Enumerator(IEnumerable<ReadableModelPopulator> source)
			{
				_source = source.GetEnumerator();
			}
			#endregion
			#region IEnumerator implementation
			public bool MoveNext()
			{
				_current = default(TReadableModel);
				return _source.MoveNext();
			}

			public void Reset()
			{
				_source.Reset();
			}

			public TReadableModel Current
			{
				get
				{
					if (_current == null)
					{
						_current = new TReadableModel();
						_current.Populate(_source.Current);
					}
					return _current;
				}
			}

			object IEnumerator.Current
			{
				get { return this.Current;}
			}
			#endregion
			#region IDisposable implementation
			public void Dispose()
			{
				_source = null;
			}
			#endregion
			#region Private Fields
			private TReadableModel _current;
			private IEnumerator<ReadableModelPopulator> _source;
			#endregion
		}
		#endregion
		#region Constructors
		public PagedReadableModelEnumerable(PagedReadableModelEnumerableSelector selector)
		{
			_resetEnumerable = () => {
				_enumerable = new Lazy<IEnumerable<ReadableModelPopulator>>(() => selector(Page, PerPage, out _totalItems, out _totalPages));
			};

			_resetEnumerable();
		}
		#endregion
		#region Public Properties
		public long TotalItems{ get { return _totalItems; } }

		public long TotalPages { get { return _totalPages; } }

		public long Page { 
			get{return _page;}
			set {
				_page = value;
				_resetEnumerable();
			}
		}

		public long PerPage { 
			get{return _perPage;}
			set {
				_perPage = value;
				_resetEnumerable();
			}
		}

		public long Count { get; protected set; }
		#endregion
		#region Public Methods
		public IEnumerator<TReadableModel> GetEnumerator()
		{
			return new Enumerator(_enumerable.Value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
		#region Private Properties
		private Action _resetEnumerable;
		private Lazy<IEnumerable<ReadableModelPopulator>> _enumerable;
		private long _page;
		private long _perPage;
		private long _totalItems;
		private long _totalPages;
		#endregion
	}
}

