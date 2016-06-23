using UnityEngine;

namespace HopeAndAnchor.Shared.Model
{
	public class GridSquare
	{
		public Vector2 bottomLeftCorner { private set; get; }
		public Vector2 bottomRightCorner { private set; get; }
		public Vector2 topLeftCorner { private set; get; }
		public Vector2 topRightCorner { private set; get; }

		public float bottomLeftValue;
		public float bottomRightValue;
		public float topLeftValue;
		public float topRightValue;
		public float size;

		public GridSquare bottomNeighbour;
		public GridSquare leftNeighbour;

		public GridSquare (float x, float y, float squareSize)
		{
			this.size = squareSize;
			bottomLeftCorner = new Vector2 (x, y);
			bottomRightCorner = new Vector2 (x + size, y);
			topLeftCorner = new Vector2 (x, y + size);
			topRightCorner = new Vector2 (x + size, y + size);
		}

	}
}
