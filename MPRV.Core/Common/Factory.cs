using System;

namespace MPRV.Common
{
	public class Factory<T>
		where T : new()
	{
		#region Singleton Members
		
		#region Constructors
		
		static Factory()
		{
		}
		
		#endregion
		
		#region Public Properties
		
		public static T Current
		{
			get
			{
				if (_current == null)
				{
					lock (_syncRoot)
					{
						if (_current == null)
						{
							_current = new T();
						}
					}
				}
				
				return _current;
			}
		}
		
		#endregion
		
		#region Private Fields
		
		private static object _syncRoot = new object();
		private static T _current;
		
		#endregion
		
		#endregion
		
		#region Instance Members
		
		#endregion
	}
}

