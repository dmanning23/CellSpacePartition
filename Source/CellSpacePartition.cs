using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CellSpacePartition
{
	/// <summary>
	/// class to divide a 2D space into a grid of cells each of which may contain a number of entities. 
	/// Once created and initialized with entities, fast proximity querys can be made by calling the 
	/// CalculateNeighbors method with a position and proximity radius.
	/// 
	/// If an entity is capable of moving, and therefore capable of moving between cells, the Update 
	/// method should be called each update-cycle to sychronize the entity and the cell space it occupies
	/// </summary>
	public class CellSpacePartition
	{
		#region Members

		/// <summary>
		/// the required amount of cells in the space
		/// </summary>
		public List<Cell> Cells { get; private set; }

		/// <summary>
		/// this is used to store any valid neighbors when an agent searches its neighboring space
		/// </summary>
		public List<IMovingEntity> Neighbors { get; private set; }

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
			WorldSize = worldSize;
			NumCells = new Point(cellsX, cellsY);
			Neighbors = new List<IMovingEntity>();

			//calculate bounds of each cell
			CellSize = new Vector2((WorldSize.X / NumCells.X), (WorldSize.Y / NumCells.Y));

			//create the cells
			for (int y = 0; y < NumCells.Y; ++y)
			{
				for (int x = 0; x < NumCells.X; ++x)
				{
					float left = x * CellSize.X;
					float top = y * CellSize.Y;

					Cells.Add(new Cell(new Rectangle((int)left, (int)top, (int)CellSize.X, (int)CellSize.Y)));
				}
			}
		}

		/// <summary>
		/// Given a 2D vector representing a position within the game world, 
		/// this method calculates an index into its appropriate cell
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		int PositionToIndex(Vector2 pos)
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
		public void AddEntity(IMovingEntity ent)
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
		public void UpdateEntity(IMovingEntity ent)
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
		public void CalculateNeighbors(Vector2 TargetPos, float QueryRadius)
		{
			//clear out the list of neighbors
			Neighbors.Clear();

			//create the query box that is the bounding box of the target's query area
			float halfRadius = QueryRadius * 0.5f;
			Vector2 upLeft = TargetPos - new Vector2(halfRadius, halfRadius);
			Rectangle QueryBox = new Rectangle((int)upLeft.X, (int)upLeft.Y, (int)QueryRadius, (int)QueryRadius);

			//iterate through each cell and test to see if its bounding box overlaps with the query box. 
			//If it does and it also contains entities then make further proximity tests.
			for (int i = 0; i < Cells.Count; i++)
			{
				//test to see if this cell contains members and if it overlaps the query box
				if (Cells[i].BBox.Intersects(QueryBox) && (0 < Cells[i].Items.Count))
				{
					//add any entities found within query radius to the neighbor list
					for (int j = 0; j < Cells[i].Items.Count; j++)
					{
						if (Vector2.DistanceSquared(Cells[i].Items[j].Position, TargetPos) <
							(QueryRadius * QueryRadius))
						{
							Neighbors.Add(Cells[i].Items[j]);
						}
					}
				}
			}
		}

		/// <summary>
		/// empties the cells of entities
		/// </summary>
		public void EmptyCells()
		{
			for (int i = 0; i < Cells.Count; i++)
			{
				Cells[i].Items.Clear();
			}
		}

		/// <summary>
		/// call this to render the cell edges
		/// </summary>
		public void RenderCells()
		{
			//std::vector<Cell<entity> >::const_iterator curCell;
			//for (curCell=m_Cells.begin(); curCell!=m_Cells.end(); ++curCell)
			//{
			//  curCell->BBox.Render(false);
			//}
		}

		#endregion //Methods
	}
}