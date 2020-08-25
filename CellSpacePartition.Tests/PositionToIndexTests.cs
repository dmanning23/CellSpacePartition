using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace CellSpacePartitionLib.Tests
{
	[TestFixture]
	public class PositionToIndexTests
	{
		[TestCase(0f, 0f, 10, 10, 10, 0f, 0f, 0)]
		[TestCase(0f, 0f, 10, 10, 10, 5f, 5f, 0)]
		[TestCase(0f, 0f, 10, 10, 10, 10f, 0f, 1)]
		[TestCase(0f, 0f, 10, 10, 10, 99.9f, 99.9f, 99)]
		[TestCase(0f, 0f, 10, 10, 10, -1, 0f, -1)]
		[TestCase(0f, 0f, 10, 10, 10, 0,-1f, -1)]
		[TestCase(0f, 0f, 10, 10, 10, -1, -1, -1)]
		[TestCase(0f, 0f, 10, 10, 10, 100, 0, -1)]
		[TestCase(0f, 0f, 10, 10, 10, 0, 100, -1)]
		[TestCase(0f, 0f, 10, 10, 10, 100, 100, -1)]
		[TestCase(0f, 0f, 10, 10, 10, -100, -100, -1)]
		[TestCase(-10f, 0f, 10, 10, 10, -1, 0f, 0)]
		[TestCase(0f, -10f, 10, 10, 10, 0, -1f, 0)]
		[TestCase(10f, 0f, 10, 10, 10, 100, 0, 9)]
		[TestCase(10f, 10f, 10, 10, 10, 100, 100, 99)]

		public void TestPositionToIndex(
			float originX,
			float originY,
			int cellSize,
			int numColumns,
			int numRows,
			float itemPositionX,
			float itemPositionY,
			int expectedIndex)
		{
			var partition = new TestCellSpacePartition(new Vector2(originX, originY), cellSize, numColumns, numRows);

			var position = new Vector2(itemPositionX, itemPositionY);

			partition.TestPositionToIndex(position).ShouldBe(expectedIndex);
		}

		[TestCase(0f, 0f, 10, 10, 10, 0f, 0f, 0)]
		[TestCase(0f, 0f, 10, 10, 10, 5f, 5f, 0)]
		[TestCase(0f, 0f, 10, 10, 10, 10f, 0f, 1)]
		[TestCase(0f, 0f, 10, 10, 10, 99.9f, 99.9f, 9)]
		[TestCase(0f, 0f, 10, 10, 10, -1, 0f, -1)]
		[TestCase(0f, 0f, 10, 10, 10, 0, -1f, 0)]
		[TestCase(0f, 0f, 10, 10, 10, -1, -1, -1)]
		[TestCase(0f, 0f, 10, 10, 10, 100, 0, 10)]
		[TestCase(0f, 0f, 10, 10, 10, 0, 100, 0)]
		[TestCase(0f, 0f, 10, 10, 10, 100, 100, 10)]
		[TestCase(0f, 0f, 10, 10, 10, -100, -100, -10)]
		[TestCase(-10f, 0f, 10, 10, 10, -1, 0f, 0)]
		[TestCase(0f, -10f, 10, 10, 10, 0, -1f, 0)]
		[TestCase(10f, 0f, 10, 10, 10, 100, 0, 9)]
		[TestCase(10f, 10f, 10, 10, 10, 100, 100, 9)]
		public void TestColumnIndex(
			float originX,
			float originY,
			int cellSize,
			int numColumns,
			int numRows,
			float itemPositionX,
			float itemPositionY,
			int expectedColumnIndex)
		{
			var partition = new TestCellSpacePartition(new Vector2(originX, originY), cellSize, numColumns, numRows);

			var position = new Vector2(itemPositionX, itemPositionY);

			partition.TestGetColumnIndex(position).ShouldBe(expectedColumnIndex);
		}

		[Test]
		public void PositionToIndex0()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = Vector2.Zero;
			partition.TestPositionToIndex(pos).ShouldBe(0);
		}

		[Test]
		public void PositionToIndex1()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(11.0f, 5.0f);
			Assert.AreEqual(1, partition.TestPositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex10()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(0.0f, 15.0f);
			Assert.AreEqual(10, partition.TestPositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex12()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(25.0f, 15.0f);
			Assert.AreEqual(12, partition.TestPositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex99()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(99.9f, 99.9f);
			partition.TestPositionToIndex(pos).ShouldBe(99);
		}

		[Test]
		public void PositionToIndexOutOfBoundsX()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(100.1f, 100.0f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexOutOfBoundsY()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(100.0f, 100.1f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexOutOfBoundsBoth()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(100.1f, 100.1f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexWayOutOfBounds()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(200.0f, 200.0f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexOutOfBoundsNegX()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(-100.1f, 100.0f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexOutOfBoundsNegY()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(100.0f, -100.1f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexOutOfBoundsNegBoth()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(-100.1f, -100.1f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}

		[Test]
		public void PositionToIndexWayOutOfNegBounds()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);
			Vector2 pos = new Vector2(-200.0f, -200.0f);
			partition.TestPositionToIndex(pos).ShouldBe(-1);
		}
	}
}
