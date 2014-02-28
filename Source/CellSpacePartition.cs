using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RectangleFLib;
using BasicPrimitiveBuddy;

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
	public class CellSpacePartition<T> where T : IMovingEntity
	{
		#region Members

		/// <summary>
		/// the required amount of cells in the space
		/// </summary>
		public List<Cell<T>> Cells { get; private set; }

		/// <summary>
		/// the width and height of the world space the entities inhabit
		/// </summary>
		public Vector2 WorldSize { get; private set; }

		/// <summary>
		/// the number of cells the space is going to be divided up into
		/// </summary>
		public Point NumCells { get; private set; }

		/// <summary>
		/// The size of each individual cell
		/// </summary>
		public Vector2 CellSize { get; private set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="worldSize">size of 2D space</param>
		/// <param name="cellsX">number of divisions horizontally</param>
		/// <param name="cellsY">and vertically</param>
		public CellSpacePartition(Vector2 worldSize, int cellsX, int cellsY)
		{
			Cells = new List<Cell<T>>();
			WorldSize = worldSize;
			NumCells = new Point(cellsX, cellsY);

			//calculate bounds of each cell
			CellSize = new Vector2((WorldSize.X / NumCells.X), (WorldSize.Y / NumCells.Y));

			//create the cells
			for (int y = 0; y < NumCells.Y; ++y)
			{
				for (int x = 0; x < NumCells.X; ++x)
				{
					float left = x * CellSize.X;
					float top = y * CellSize.Y;

					Cells.Add(new Cell<T>(new RectangleF(left, top, CellSize.X, CellSize.Y)));
				}
			}
		}

		/// <summary>
		/// Given a 2D vector representing a position within the game world, 
		/// this method calculates an index into its appropriate cell
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public int PositionToIndex(Vector2 pos)
		{
			int idx = (int)(NumCells.X * pos.X / WorldSize.X) +
					  ((int)(NumCells.Y * pos.Y / WorldSize.Y) * NumCells.X);

			//if the entity's position is equal to vector2d(m_dSpaceWidth, m_dSpaceHeight)
			//then the index will overshoot. We need to check for this and adjust
			if (idx > Cells.Count - 1)
			{
				idx = Cells.Count - 1;
			}

			return idx;
		}

		/// <summary>
		/// Used to add the entitys to the appropriate cell
		/// </summary>
		/// <param name="ent"></param>
		/// <param name="pos"></param>
		public void Add(T ent)
		{
			int sz = Cells.Count;
			int idx = PositionToIndex(ent.Position);

			Cells[idx].Items.Add(ent);
		}

		/// <summary>
		/// Checks to see if an entity has moved cells. 
		/// If so the data structure is updated accordingly
		/// </summary>
		/// <param name="ent"></param>
		public void Update(T ent)
		{
			//if the index for the old pos and the new pos are not equal then the entity has moved to another cell.
			int OldIdx = PositionToIndex(ent.OldPosition);
			int NewIdx = PositionToIndex(ent.Position);

			if (NewIdx == OldIdx)
			{
				return;
			}

			//the entity has moved into another cell so delete from current cell and add to new one
			Cells[OldIdx].Items.Remove(ent);
			Cells[NewIdx].Items.Add(ent);
		}

		/// <summary>
		///  This must be called to create the vector of neighbors.
		///  This method examines each cell within range of the target
		///  If the cells contain entities then they are tested to see if they are situated within the target's neighborhood region. 
		///  If they are they are added to neighbor list
		/// </summary>
		/// <param name="TargetPos"></param>
		/// <param name="QueryRadius"></param>
		public List<T> CalculateNeighbors(Vector2 targetPos, float queryRadius)
		{
			//create the list of neighbors
			List<T> neighbors = new List<T>();

			RectangleF queryBox = CreateQueryBox(targetPos, queryRadius);

			//iterate through each cell and test to see if its bounding box overlaps with the query box. 
			//If it does and it also contains entities then make further proximity tests.
			float radiusSquared = (queryRadius * queryRadius);
			for (int i = 0; i < Cells.Count; i++)
			{
				//test to see if this cell contains members and if it overlaps the query box
				if ((0 < Cells[i].Items.Count) && Cells[i].BBox.Intersects(queryBox))
				{
					//add any entities found within query radius to the neighbor list
					for (int j = 0; j < Cells[i].Items.Count; j++)
					{
						float distToDude = Vector2.DistanceSquared(Cells[i].Items[j].Position, targetPos);
						if (distToDude <= radiusSquared)
						{
							neighbors.Add(Cells[i].Items[j]);
						}
					}
				}
			}

			return neighbors;
		}

		/// <summary>
		/// create the query box that is the bounding box of the target's query area
		/// </summary>
		/// <param name="TargetPos"></param>
		/// <param name="QueryRadius"></param>
		/// <returns></returns>
		public static RectangleF CreateQueryBox(Vector2 targetPos, float queryRadius)
		{
			float twoRadius = queryRadius * MathHelper.PiOver2;
			float halfRadius = twoRadius * 0.5f;

			Vector2 upLeft = targetPos - new Vector2(halfRadius, halfRadius);
			return new RectangleF(upLeft.X, upLeft.Y, twoRadius, twoRadius);
		}

		/// <summary>
		/// empties the cells of entities
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < Cells.Count; i++)
			{
				Cells[i].Items.Clear();
			}
		}

		#region Debug Rendering

		/// <summary>
		/// call this to render the cell edges
		/// </summary>
		/// <param name="primitive"></param>
		public void RenderCells(IBasicPrimitive primitive)
		{
			foreach (var cell in Cells)
			{
				cell.RenderCell(primitive, Color.White);
			}
		}

		public void RenderCellIntersections(IBasicPrimitive primitive, Vector2 targetPos, float queryRadius, Color color)
		{
			RectangleF queryBox = CreateQueryBox(targetPos, queryRadius);

			//iterate through each cell and test to see if its bounding box overlaps with the query box. 
			for (int i = 0; i < Cells.Count; i++)
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