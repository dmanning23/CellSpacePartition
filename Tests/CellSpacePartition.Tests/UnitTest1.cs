using System;
using NUnit;
using NUnit.Framework;
using CellSpacePartition;
using Microsoft.Xna.Framework;
using Moq;
using RectangleFLib;

namespace CellSpacePartition.Tests
{
	[TestFixture]
	public class UnitTest1
	{
		[Test]
		public void CorrectCellSizeX()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Assert.AreEqual(10.0f, tester.CellSize.X);
		}

		[Test]
		public void CorrectCellSizeY()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Assert.AreEqual(10.0f, tester.CellSize.Y);
		}

		[Test]
		public void CorrectCellSizeY2()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 20);
			Assert.AreEqual(5.0f, tester.CellSize.Y);
		}

		[Test]
		public void CellSetupPosX()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Assert.AreEqual(10.0f, tester.Cells[21].BBox.Left);
		}

		[Test]
		public void CellSetupPosY()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Assert.AreEqual(20.0f, tester.Cells[21].BBox.Top);
		}

		[Test]
		public void CellSetupSizeX()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 20);
			Assert.AreEqual(10.0f, tester.Cells[21].BBox.Width);
		}

		[Test]
		public void CellSetupSizeY()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 20);
			Assert.AreEqual(5.0f, tester.Cells[21].BBox.Height);
		}

		[Test]
		public void CorrectNumCells()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Assert.AreEqual(100, tester.Cells.Count);
		}

		[Test]
		public void PositionToIndex0()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = Vector2.Zero;
			Assert.AreEqual(0, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex1()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(11.0f, 5.0f);
			Assert.AreEqual(1, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex10()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(0.0f, 15.0f);
			Assert.AreEqual(10, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex12()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(25.0f, 15.0f);
			Assert.AreEqual(12, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndex99()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(100.0f, 100.0f);
			Assert.AreEqual(99, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndexOutOfBoundsX()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(100.1f, 100.0f);
			Assert.AreEqual(99, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndexOutOfBoundsY()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(100.0f, 100.1f);
			Assert.AreEqual(99, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndexOutOfBoundsBoth()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(100.1f, 100.1f);
			Assert.AreEqual(99, tester.PositionToIndex(pos));
		}

		[Test]
		public void PositionToIndexWayOutOfBounds()
		{
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);
			Vector2 pos = new Vector2(200.0f, 200.0f);
			Assert.AreEqual(99, tester.PositionToIndex(pos));
		}

		[Test]
		public void AddEntity()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//create a fake item
			var entity = new Mock<IMovingEntity>();
			entity.Setup(x => x.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			tester.Add(entity.Object);

			//make sure it went in the correct spot
			Assert.AreEqual(1, tester.Cells[12].Items.Count);
		}

		[Test]
		public void UpdateEntityRemoveOld()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//create a fake item
			var entity = new Mock<IMovingEntity>();
			entity.Setup(x => x.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			tester.Add(entity.Object);

			//make sure it went in the correct spot
			Assert.AreEqual(1, tester.Cells[12].Items.Count);

			//move the entity
			entity.Setup(x => x.OldPosition).Returns(new Vector2(25.0f, 15.0f));
			entity.Setup(x => x.Position).Returns(new Vector2(35.0f, 15.0f));
			tester.Update(entity.Object);

			//make sure it removed the old object and added the new spot
			Assert.AreEqual(0, tester.Cells[12].Items.Count);
		}

		[Test]
		public void UpdateEntityAddToNewSpot()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//create a fake item
			var entity = new Mock<IMovingEntity>();
			entity.Setup(x => x.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			tester.Add(entity.Object);

			//make sure it went in the correct spot
			Assert.AreEqual(1, tester.Cells[12].Items.Count);

			//move the entity
			entity.Setup(x => x.OldPosition).Returns(new Vector2(25.0f, 15.0f));
			entity.Setup(x => x.Position).Returns(new Vector2(35.0f, 15.0f));
			tester.Update(entity.Object);

			//make sure it removed the old object and added the new spot
			Assert.AreEqual(0, tester.Cells[12].Items.Count);
			Assert.AreEqual(1, tester.Cells[13].Items.Count);
		}

		[Test]
		public void CreateQueryBoxLeft()
		{
			RectangleF box = CellSpacePartition.CreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(20.0f, box.Left);
		}

		[Test]
		public void CreateQueryBoxRight()
		{
			RectangleF box = CellSpacePartition.CreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(30.0f, box.Right);
		}

		[Test]
		public void CreateQueryBoxTop()
		{
			RectangleF box = CellSpacePartition.CreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(10.0f, box.Top);
		}

		[Test]
		public void CreateQueryBoxBottom()
		{
			RectangleF box = CellSpacePartition.CreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(20.0f, box.Bottom);
		}

		[Test]
		public void CalcNeighbor()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//create a fake item
			var entity = new Mock<IMovingEntity>();
			entity.Setup(x => x.Position).Returns(new Vector2(25.0f, 15.0f));
			tester.Add(entity.Object);

			//add a second item
			var entity1 = new Mock<IMovingEntity>();
			entity1.Setup(x => x.Position).Returns(new Vector2(35.0f, 15.0f));
			tester.Add(entity1.Object);

			//calculate the neighbors
			tester.CalculateNeighbors(new Vector2(25.0f, 15.0f), 10.0f);
			Assert.AreEqual(2, tester.Neighbors.Count);
		}

		[Test]
		public void CalcNeighbor1()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//add a second item
			var entity1 = new Mock<IMovingEntity>();
			entity1.Setup(x => x.Position).Returns(new Vector2(35.0f, 15.0f));
			tester.Add(entity1.Object);

			//add a third item
			var entity2 = new Mock<IMovingEntity>();
			entity2.Setup(x => x.Position).Returns(new Vector2(36.0f, 15.0f));
			tester.Add(entity2.Object);

			//calculate the neighbors
			tester.CalculateNeighbors(new Vector2(25.0f, 15.0f), 10.0f);
			Assert.AreEqual(1, tester.Neighbors.Count);
		}

		[Test]
		public void CalcNeighbor2()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//add a second item
			var entity1 = new Mock<IMovingEntity>();
			entity1.Setup(x => x.Position).Returns(new Vector2(35.0f, 15.0f));
			tester.Add(entity1.Object);

			//add a third item
			var entity2 = new Mock<IMovingEntity>();
			entity2.Setup(x => x.Position).Returns(new Vector2(30.0f, 15.0f));
			tester.Add(entity2.Object);

			//calculate the neighbors
			tester.CalculateNeighbors(new Vector2(25.0f, 15.0f), 10.0f);
			Assert.AreEqual(2, tester.Neighbors.Count);
		}

		[Test]
		public void CalcNeighbor3()
		{
			//create the thing
			CellSpacePartition tester = new CellSpacePartition(new Vector2(100.0f, 100.0f), 10, 10);

			//add a second item
			var entity1 = new Mock<IMovingEntity>();
			entity1.Setup(x => x.Position).Returns(new Vector2(35.0f, 15.0f));
			tester.Add(entity1.Object);

			//add a third item
			var entity2 = new Mock<IMovingEntity>();
			entity2.Setup(x => x.Position).Returns(new Vector2(30.0f, 15.0f));
			tester.Add(entity2.Object);

			//add a third item
			var entity3 = new Mock<IMovingEntity>();
			entity3.Setup(x => x.Position).Returns(new Vector2(30.0f, 20.0f));
			tester.Add(entity3.Object);

			//calculate the neighbors
			tester.CalculateNeighbors(new Vector2(25.0f, 15.0f), 10.0f);
			Assert.AreEqual(3, tester.Neighbors.Count);
		}
	}
}
