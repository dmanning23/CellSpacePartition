using FakeItEasy;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using RectangleFLib;
using Shouldly;

namespace CellSpacePartitionLib.Tests
{
	[TestFixture]
	public class QueryBoxTests
	{
		TestCellSpacePartition partition;

		[SetUp]
		public void Setup()
		{
			partition = new TestCellSpacePartition(Vector2.Zero, 10, 10, 10);
		}

		

		[Test]
		public void CreateQueryBoxLeft()
		{
			RectangleF box = partition.TestCreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(15f, box.Left);
		}

		[Test]
		public void CreateQueryBoxRight()
		{
			RectangleF box = partition.TestCreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(35f, box.Right);
		}

		[Test]
		public void CreateQueryBoxTop()
		{
			RectangleF box = partition.TestCreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(5f, box.Top);
		}

		[Test]
		public void CreateQueryBoxBottom()
		{
			RectangleF box = partition.TestCreateQueryBox(new Vector2(25.0f, 15.0f), 10.0f);

			Assert.AreEqual(25f, box.Bottom);
		}
	}
}
