using Microsoft.Xna.Framework;

namespace CellSpacePartition
{
	/// <summary>
	/// A simple moving entity with position and old position
	/// </summary>
	public interface IMovingEntity
	{
		Vector2 Position { get; }

		Vector2 OldPosition { get; }
	}
}