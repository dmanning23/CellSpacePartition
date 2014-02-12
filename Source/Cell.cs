using Microsoft.Xna.Framework;
using BasicPrimitiveBuddy;
using System.Collections.Generic;
using RectangleFLib;

namespace CellSpacePartitionLib
{
	/// <summary>
	/// defines a cell containing a list of pointers to entities
	/// </summary>
	public class Cell<T> where T : IMovingEntity
	{
		#region Members 

		/// <summary>
		/// all the entities inhabiting this cell
		/// </summary>
		public List<T> Items { get; private set; }

		/// <summary>
		/// the cell's bounding box
		/// </summary>
		public RectangleF BBox { get; private set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="topleft"></param>
		/// <param name="botright"></param>
		public Cell(RectangleF area)
		{
			Items = new List<T>();
			BBox = area;
		}

		/// <summary>
		/// call this to render the cell edges
		/// </summary>
		public void RenderCell(IBasicPrimitive primitive)
		{
			primitive.Rectangle(BBox, Color.White);
		}

		#endregion //Methods
	}
}