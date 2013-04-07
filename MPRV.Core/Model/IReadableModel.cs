using System;

namespace MPRV.Model
{
	public interface IReadableModel : IModel
	{
		bool IsPopulated { get; }

		bool Populate (ReadableModelPopulator populator);
	}

	public delegate bool ReadableModelPopulator (IReadableModel model);

	public static class IReadableModelExtensions
	{
		public static bool IsInstantiatedAndPopulated (this IReadableModel readableModel)
		{
			return readableModel != null && readableModel.IsPopulated;
		}
	}
}

