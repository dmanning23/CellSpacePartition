using CellSpacePartitionLib.Tests;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSpacePartitionLib.Tests
{
	[TestFixture]
	public class ConstructionTests
	{
		[TestCase(0, 0)]
		[TestCase(-100, -100)]
		[TestCase(100, 100)]
		public void XPosition(float x, float expectedValue)
		{
			var partition = new TestCellSpacePartition(new Vector2(x, 0), 0, 0, 0);
			partition.CellSpace.X.ShouldBe(expectedValue);
		}

		[TestCase(0, 0)]
		[TestCase(-100, -100)]
		[TestCase(100, 100)]
		public void YPosition(float y, float expectedValue)
		{
			var partition = new TestCellSpacePartition(new Vector2(0, y), 0, 0, 0);
			partition.CellSpace.Y.ShouldBe(expectedValue);
		}

		[TestCase(0, 0, 0)]
		[TestCase(10, 10, 100)]
		[TestCase(10, 20, 200)]
		public void TestWidth(int cellSize, int numColumns, float expectedValue)
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), cellSize, numColumns, 0);
			partition.CellSpace.Width.ShouldBe(expectedValue);
		}

		[TestCase(0, 0, 0)]
		[TestCase(10, 10, 100)]
		[TestCase(10, 20, 200)]
		public void TestHeight(int cellSize, int numRows, float expectedValue)
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), cellSize, 0, numRows);
			partition.CellSpace.Height.ShouldBe(expectedValue);
		}
	}
}
