using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System;
using System.Collections.Generic;

namespace CellSpacePartitionLib
{
	/// <summary>
	/// class to divide a 2D space into a grid of cells each of which may contain a number of entities. 
	/// Once created and initialized with entities, fast proximity querys can be made by calling the 
	/// CalculateNeighbors method with a position and proximity radius.
	/// 
	/// If an entity is capable of moving, and therefore capable of moving between cells, the Update 
	/// method should be called each update-cycle to sychronize the entity and the cell space it occupies
	/// </summary>
	public class CellSpacePartition<T> : IPartition<T> where T : IMovingEntity
	{
		#region Properties

		/// <summary>
		/// the required amount of cells in the space
		/// </summary>
		protected List<Cell<T>> Cells { get; set; }

		/// <summary>
		/// This is a list of dudes that fall outside the cellspace
		/// </summary>
		protected List<T> Floaters { get; set; }

		/// <summary>
		/// the number of cells the space is going to be divided up into
		/// </summary>
		protected Point NumCells { get; set; }

		public RectangleFLib.RectangleF CellSpace { get; protected set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="worldSize">size of 2D space</param>
		/// <param name="cellsX">number of divisions horizontally</param>
		/// <param name="cellsY">and vertically</param>
		public CellSpacePartition(Vector2 origin, int cellsize, int cellsX, int cellsY)
		{
			Cells = new List<Cell<T>>();

			NumCells = new Point(cellsX, cellsY);

			var worldSize = NumCells.ToVector2() * cellsize;
			CellSpace = new RectangleFLib.RectangleF(origin.X, origin.Y, worldSize.X, worldSize.Y);

			//create the cells
			for (var y = 0; y < NumCells.Y; ++y)
			{
				for (var x = 0; x < NumCells.X; ++x)
				{
					var left = origin.X + x * cellsize;
					var top = origin.Y + y * cellsize;

					Cells.Add(new Cell<T>(new RectangleFLib.RectangleF(left, top, cellsize, cellsize)));
				}
			}

			Floaters = new List<T>();
		}

		/// <summary>
		/// Given a 2D vector representing a position within the game world, 
		/// this method calculates an index into its appropriate cell
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		protected int PositionToIndex(Vector2 position)
		{
			var columnIndex = GetColumnIndex(position);
			var rowIndex = GetRowIndex(position);
			var cellIndex = GetColumnIndex(position) + (GetRowIndex(position) * NumCells.X);

			//if the entity's position is equal to vector2d(m_dSpaceWidth, m_dSpaceHeight)
			//then the index will overshoot. We need to check for this and adjust
			if (columnIndex < 0 || NumCells.X <= columnIndex || rowIndex < 0 || NumCells.Y <= rowIndex)
			{
				cellIndex = -1;
			}

			return cellIndex;
		}

		private int GetRowIndex(Vector2 position)
		{
			return (int)Math.Floor((NumCells.Y * (position.Y - CellSpace.Y)) / CellSpace.Height);
		}

		protected int GetColumnIndex(Vector2 position)
		{
			return (int)Math.Floor((NumCells.X * (position.X - CellSpace.X)) / CellSpace.Width);
		}

		/// <summary>
		/// Used to add the entitys to the appropriate cell
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			AddToCell(item, PositionToIndex(item.Position));
		}

		/// <summary>
		/// Used to remove the entitys from the appropriate cell
		/// </summary>
		/// <param name="item"></param>
		public void Remove(T item)
		{
			RemoveFromCell(item, PositionToIndex(item.Position));
		}

		protected void AddToCell(T item, int cellIndex)
		{
			if (0 <= cellIndex && cellIndex < Cells.Count)
			{
				Cells[cellIndex].Items.Add(item);
			}
			else
			{
				Floaters.Add(item);
			}
		}

		protected void RemoveFromCell(T item, int cellIndex)
		{
			if (0 <= cellIndex && cellIndex < Cells.Count)
			{
				Cells[cellIndex].Items.Remove(item);
			}
			else
			{
				Floaters.Remove(item);
			}
		}

		/// <summary>
		/// Checks to see if an entity has moved cells. 
		/// If so the data structure is updated accordingly
		/// </summary>
		/// <param name="item"></param>
		public void Update(T item)
		{
			//if the index for the old pos and the new pos are not equal then the entity has moved to another cell.
			var oldIdx = PositionToIndex(item.OldPosition);
			var newIdx = PositionToIndex(item.Position);

			if (newIdx == oldIdx)
			{
				return;
			}

			//the entity has moved into another cell so delete from current cell and add to new one
			RemoveFromCell(item, oldIdx);
			AddToCell(item, newIdx);
		}

		/// <summary>
		///  This must be called to create the vector of neighbors.
		///  This method examines each cell within range of the target
		///  If the cells contain entities then they are tested to see if they are situated within the target's neighborhood region. 
		///  If they are they are added to neighbor list
		/// </summary>
		/// <param name="targetPos"></param>
		/// <param name="queryRadius"></param>
		public List<T> CalculateNeighbors(Vector2 targetPos, float queryRadius, bool fastQuery = false)
		{
			//create the list of neighbors
			var neighbors = new List<T>();

			var queryBox = CreateQueryBox(targetPos, queryRadius);

			//iterate through each cell and test to see if its bounding box overlaps with the query box. 
			//If it does and it also contains entities then make further proximity tests.
			var radiusSquared = (queryRadius * queryRadius);
			for (var i = 0; i < Cells.Count; i++)
			{
				//test to see if this cell contains members and if it overlaps the query box
				if ((0 < Cells[i].Items.Count) && Cells[i].BBox.Intersects(queryBox))
				{
					//If we are doing a fast query, just dump all the dudes in this cell
					if (fastQuery)
					{
						neighbors.AddRange(Cells[i].Items);
					}
					else
					{
						//add any entities found within query radius to the neighbor list
						for (var j = 0; j < Cells[i].Items.Count; j++)
						{
							var distToDude = Vector2.DistanceSquared(Cells[i].Items[j].Position, targetPos);
							if (distToDude <= radiusSquared)
							{
								neighbors.Add(Cells[i].Items[j]);
							}
						}
					}
				}
			}

			//check the floaters too
			for (var i = 0; i < Floaters.Count; i++)
			{
				var distToDude = Vector2.DistanceSquared(Floaters[i].Position, targetPos);
				if (distToDude <= radiusSquared)
				{
					neighbors.Add(Floaters[i]);
				}
			}

			return neighbors;
		}

		/// <summary>
		/// create the query box that is the bounding box of the target's query area
		/// </summary>
		/// <param name="targetPos"></param>
		/// <param name="queryRadius"></param>
		/// <returns></returns>
		protected RectangleFLib.RectangleF CreateQueryBox(Vector2 targetPos, float queryRadius)
		{
			var upLeft = targetPos - new Vector2(queryRadius, queryRadius);
			var radius2 = queryRadius * 2f;
			return new RectangleFLib.RectangleF(upLeft.X, upLeft.Y, radius2, radius2);
		}

		/// <summary>
		/// empties the cells of entities
		/// </summary>
		public void Clear()
		{
			for (var i = 0; i < Cells.Count; i++)
			{
				Cells[i].Items.Clear();
			}
			Floaters.Clear();
		}

		public T NearestNeighbor(Vector2 targetPos, float queryRadius)
		{
			//get all the dudes in range
			var inRange = CalculateNeighbors(targetPos, queryRadius);

			float closestDistance = 0f;
			T closest = default(T);

			//set the "closest" to the first available
			if (inRange.Count >= 1)
			{
				closest = inRange[0];
				closestDistance = DistanceSquared(targetPos, inRange[0]);
			}

			//loop through the rest and see if there are any closer
			if (inRange.Count >= 2)
			{
				for (int i = 1; i < inRange.Count; i++)
				{
					var distance = DistanceSquared(targetPos, inRange[i]);
					if (distance < closestDistance)
					{
						closest = inRange[i];
						closestDistance = distance;
					}
				}
			}

			return closest;
		}

		private float DistanceSquared(Vector2 position, IMovingEntity dude)
		{
			return (dude.Position - position).LengthSquared();
		}

		#region Debug Rendering

		/// <summary>
		/// call this to render the cell edges
		/// </summary>
		/// <param name="primitive"></param>
		public void RenderCells(IPrimitive primitive)
		{
			foreach (var cell in Cells)
			{
				cell.RenderCell(primitive, Color.White);
			}
		}

		public void RenderCellIntersections(IPrimitive primitive, Vector2 targetPos, float queryRadius, Color color)
		{
			var queryBox = CreateQueryBox(targetPos, queryRadius);

			//iterate through each cell and test to see if its bounding box overlaps with the query box. 
			for (var i = 0; i < Cells.Count; i++)
			{
				//test to see if this cell contains members and if it overlaps the query box
				if (Cells[i].BBox.Intersects(queryBox))
				{
					//draw the cell
					Cells[i].RenderCell(primitive, color);
				}
			}
		}

		#endregion //Debug Rendering

		#endregion //Methods
	}
}