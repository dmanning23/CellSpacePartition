using FakeItEasy;
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
	public class AddEntityTests
	{
		[Test]
		public void AddEntity()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			partition.Add(entity);

			//make sure it went in the correct spot
			partition.TestCells[12].Items.Count.ShouldBe(1);
		}

		[TestCase(-25f, 15f)]
		[TestCase(25f, -15f)]
		[TestCase(-25f, -15f)]
		[TestCase(250f, 15f)]
		[TestCase(25f, 150f)]
		[TestCase(250f, 150f)]
		public void AddEntityFloater(float x, float y)
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(x, y));

			//add the item
			partition.Add(entity);

			//make sure it went in the correct spot
			partition.TestCells[12].Items.Count.ShouldBe(0);
			partition.TestFloaters.Count.ShouldBe(1);
		}

		[Test]
		public void RemoveEntity()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));
			partition.Add(entity);

			//remove the item
			partition.Remove(entity);

			//make sure it went in the correct spot
			Assert.AreEqual(0, partition.TestCells[12].Items.Count);
		}

		[Test]
		public void UpdateEntityRemoveOld()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			partition.Add(entity);

			//make sure it went in the correct spot
			Assert.AreEqual(1, partition.TestCells[12].Items.Count);

			//move the entity
			A.CallTo(() => entity.OldPosition).Returns(new Vector2(25.0f, 15.0f));
			A.CallTo(() => entity.Position).Returns(new Vector2(35.0f, 15.0f));
			partition.Update(entity);

			//make sure it removed the old object and added the new spot
			Assert.AreEqual(0, partition.TestCells[12].Items.Count);
		}

		[Test]
		public void UpdateEntityAddToNewSpot()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			partition.Add(entity);

			//make sure it went in the correct spot
			Assert.AreEqual(1, partition.TestCells[12].Items.Count);

			//move the entity
			A.CallTo(() => entity.OldPosition).Returns(new Vector2(25.0f, 15.0f));
			A.CallTo(() => entity.Position).Returns(new Vector2(35.0f, 15.0f));
			partition.Update(entity);

			//make sure it removed the old object and added the new spot
			Assert.AreEqual(0, partition.TestCells[12].Items.Count);
			Assert.AreEqual(1, partition.TestCells[13].Items.Count);
		}

		[Test]
		public void UpdateEntityToFloater()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));

			//add the item
			partition.Add(entity);

			//make sure it went in the correct spot
			Assert.AreEqual(1, partition.TestCells[12].Items.Count);

			//move the entity
			A.CallTo(() => entity.OldPosition).Returns(new Vector2(25.0f, 15.0f));
			A.CallTo(() => entity.Position).Returns(new Vector2(-35.0f, 15.0f));
			partition.Update(entity);

			//make sure it removed the old object and added the new spot
			partition.TestCells[12].Items.Count.ShouldBe(0);
			partition.TestFloaters.Count.ShouldBe(1);
		}

		[Test]
		public void UpdateFloaterToNewSpot()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(-25.0f, 15.0f));

			//add the item
			partition.Add(entity);

			//make sure it went in the correct spot
			partition.TestFloaters.Count.ShouldBe(1);

			//move the entity
			A.CallTo(() => entity.OldPosition).Returns(new Vector2(-25.0f, 15.0f));
			A.CallTo(() => entity.Position).Returns(new Vector2(35.0f, 15.0f));
			partition.Update(entity);

			//make sure it removed the old object and added the new spot
			partition.TestFloaters.Count.ShouldBe(0);
			partition.TestCells[13].Items.Count.ShouldBe(1);
		}

		[Test]
		public void UpdateEntityNotAdded()
		{
			var partition = new TestCellSpacePartition(new Vector2(0, 0), 10, 10, 10);

			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));

			//move the entity
			A.CallTo(() => entity.OldPosition).Returns(new Vector2(25.0f, 15.0f));
			A.CallTo(() => entity.Position).Returns(new Vector2(35.0f, 15.0f));
			partition.Update(entity);

			//make sure it removed the old object and added the new spot
			Assert.AreEqual(0, partition.TestCells[12].Items.Count);
			Assert.AreEqual(1, partition.TestCells[13].Items.Count);
		}
	}
}
