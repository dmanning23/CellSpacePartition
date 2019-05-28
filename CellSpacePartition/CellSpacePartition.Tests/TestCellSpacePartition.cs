using Microsoft.Xna.Framework;
using RectangleFLib;
using System.Collections.Generic;

namespace CellSpacePartitionLib.Tests
{
	public class Mover : IMovingEntity
	{
		public virtual Vector2 OldPosition
		{
			get
			{
				return Vector2.Zero;
			}
		}

		public virtual Vector2 Position
		{
			get
			{
				return Vector2.Zero;
			}
		}
	}

	public class TestCellSpacePartition : CellSpacePartition<Mover>
	{
		public TestCellSpacePartition(Vector2 origin, int cellsize, int cellsX, int cellsY) : base(origin, cellsize, cellsX, cellsY)
		{
		}

		public List<Cell<Mover>> TestCells => Cells;

		public List<Mover> TestFloaters => Floaters;

		public RectangleF TestCreateQueryBox(Vector2 targetPos, float queryRadius)
		{
			return CreateQueryBox(targetPos, queryRadius);
		}

		public int TestPositionToIndex(Vector2 position)
		{
			return PositionToIndex(position);
		}

		public int TestGetColumnIndex(Vector2 position)
		{
			return GetColumnIndex(position);
		}
	}
}
