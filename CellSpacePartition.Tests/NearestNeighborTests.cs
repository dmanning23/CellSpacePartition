using FakeItEasy;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Shouldly;

namespace CellSpacePartitionLib.Tests
{
	[TestFixture]
	public class NearestNeighborTests
	{
		TestCellSpacePartition partition;

		[SetUp]
		public void Setup()
		{
			partition = new TestCellSpacePartition(Vector2.Zero, 10, 10, 10);
		}

		[Test]
		public void CalcNeighbor()
		{
			//create a fake item
			var entity = A.Fake<Mover>();
			A.CallTo(() => entity.Position).Returns(new Vector2(25.0f, 15.0f));
			A.CallTo(() => entity.Id).Returns(1);
			partition.Add(entity);

			//add a second item
			var entity1 = A.Fake<Mover>();
			A.CallTo(() => entity1.Position).Returns(new Vector2(25.0f, 14.0f));
			A.CallTo(() => entity1.Id).Returns(2);
			partition.Add(entity1);

			//calculate the neighbors
			var neighbor = partition.NearestNeighbor(new Vector2(25.0f, 13.0f), 10.0f);
			neighbor.Id.ShouldBe(2);
		}

		[Test]
		public void CalcNeighbor1()
		{
			//add a second item
			var entity1 = A.Fake<Mover>();
			A.CallTo(() => entity1.Position).Returns(new Vector2(35.0f, 15.0f));
			A.CallTo(() => entity1.Id).Returns(1);
			partition.Add(entity1);

			//add a third item
			var entity2 = A.Fake<Mover>();
			A.CallTo(() => entity2.Position).Returns(new Vector2(36f, 15.0f));
			A.CallTo(() => entity2.Id).Returns(2);
			partition.Add(entity2);

			//calculate the neighbors
			var neighbor = partition.NearestNeighbor(new Vector2(25.0f, 15.0f), 10.0f);
			neighbor.Id.ShouldBe(1);
		}

		[Test]
		public void CalcNeighbor2()
		{
			//add a second item
			var entity1 = A.Fake<Mover>();
			A.CallTo(() => entity1.Position).Returns(new Vector2(35.0f, 15.0f));
			A.CallTo(() => entity1.Id).Returns(1);
			partition.Add(entity1);

			//add a third item
			var entity2 = A.Fake<Mover>();
			A.CallTo(() => entity2.Position).Returns(new Vector2(30f, 15.0f));
			A.CallTo(() => entity2.Id).Returns(2);
			partition.Add(entity2);

			//calculate the neighbors
			var neighbor = partition.NearestNeighbor(new Vector2(25.0f, 15.0f), 10.0f);
			neighbor.Id.ShouldBe(2);
		}

		[Test]
		public void CalcNeighbor3()
		{
			//add a second item
			var entity1 = A.Fake<Mover>();
			A.CallTo(() => entity1.Position).Returns(new Vector2(35.0f, 15.0f));
			A.CallTo(() => entity1.Id).Returns(1);
			partition.Add(entity1);

			//add a third item
			var entity2 = A.Fake<Mover>();
			A.CallTo(() => entity2.Position).Returns(new Vector2(30f, 15.0f));
			A.CallTo(() => entity2.Id).Returns(2);
			partition.Add(entity2);

			//add a third item
			var entity3 = A.Fake<Mover>();
			A.CallTo(() => entity3.Position).Returns(new Vector2(30f, 20f));
			A.CallTo(() => entity3.Id).Returns(3);
			partition.Add(entity3);

			//calculate the neighbors
			var neighbor = partition.NearestNeighbor(new Vector2(25.0f, 15.0f), 10.0f);
			neighbor.Id.ShouldBe(2);
		}

		[Test]
		public void NoNeighbor()
		{
			//add a second item
			var entity1 = A.Fake<Mover>();
			A.CallTo(() => entity1.Position).Returns(new Vector2(35.0f, 15.0f));
			A.CallTo(() => entity1.Id).Returns(1);
			partition.Add(entity1);

			//add a third item
			var entity2 = A.Fake<Mover>();
			A.CallTo(() => entity2.Position).Returns(new Vector2(30f, 15.0f));
			A.CallTo(() => entity2.Id).Returns(2);
			partition.Add(entity2);

			//add a third item
			var entity3 = A.Fake<Mover>();
			A.CallTo(() => entity3.Position).Returns(new Vector2(30f, 20f));
			A.CallTo(() => entity3.Id).Returns(3);
			partition.Add(entity3);

			//calculate the neighbors
			var neighbor = partition.NearestNeighbor(new Vector2(25.0f, 15.0f), 4.0f);
			neighbor.ShouldBeNull();
		}
	}
}
