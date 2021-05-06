using System;
using System.ComponentModel;

namespace WFA.ECS.Framework.Core.Framework.Interception.Interfaces
{
	/// <summary>
	/// Daniel Cazzulino's hack to hide methods defined on <see cref="object"/> for IntelliSense on Fluent Interfaces
	/// </summary>
	public interface IHideBaseTypes
	{
		/// <inheritdoc/>	
		[EditorBrowsable(EditorBrowsableState.Never)]
		Type GetType();

		/// <inheritdoc/>	
		[EditorBrowsable(EditorBrowsableState.Never)]
		int GetHashCode();

		/// <inheritdoc/>	
		[EditorBrowsable(EditorBrowsableState.Never)]
		string ToString();

		/// <inheritdoc/>	
		[EditorBrowsable(EditorBrowsableState.Never)]
		bool Equals(object other);
	}
}
