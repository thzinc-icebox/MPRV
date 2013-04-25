using System;
using MPRV.Common.Reflection;
using System.Linq;
using MPRV.Common;

namespace MPRV.Model
{
	public abstract class WritableModel<TWritableModel>: ReadableModel, IWritableModel<TWritableModel>
		where TWritableModel : class, IReadableModel, new()
	{
		#region Constructors

		public WritableModel ()
		{
		}

		#endregion

		#region Public Methods

		public abstract bool Commit ();

		public override bool Populate(ReadableModelPopulator populator)
		{
			_original = new Lazy<TWritableModel>(() => {
				var original = new TWritableModel();
				original.Populate(populator);

				return original;
			});

			return base.Populate(populator);
		}

		#region IEquatable implementation

		public bool Equals(TWritableModel other)
		{
			return typeof(TWritableModel)
				.GetMembers<EquatableAttribute>(TypeExtensions.PROPERTY_AND_FIELD_BINDING_FLAGS, true)
				.Select(l => l.Key)
				.All(mi => this.GetMember(mi).GetValueOrDefault(v => v.Equals(other.GetMember(mi)), () => false));
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as TWritableModel);
		}

		public override int GetHashCode()
		{
			// TODO: reflect
			return base.GetHashCode();
		}

		#endregion

		#endregion

		#region Public Properties

		public bool IsChanged {
			get {
				return !this.Equals(_original.Value);
			}
		}

		#endregion

		#region Protected Methods

		#endregion

		#region Protected Properties

		protected Lazy<TWritableModel> _original {get; set;}

		#endregion
	}
}

