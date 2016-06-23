using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HopeAndAnchor.Shared.Model;
using HopeAndAnchor.Shared.View;

namespace HopeAndAnchor.TileMap.View
{
	public class TileMapDisplay : MonoBehaviour
	{
		[SerializeField] private ThumbStick ThumbStick;
		[SerializeField] private TiledSurface TiledSurface;
		[SerializeField] private Transform Player;
		[SerializeField] private int Speed;
		[SerializeField] private int TileSize = 2;

		static private int NOISE_MULTIPLIER = 3;
		static private int SCALE = 8;

		private MarchingSquaresModel model;
		private GridSquare[] grid;
		private Bounds screenBounds;
		private Vector3 playerPosition;
		private Vector3 squareOffset;
		private int gridWidth;
		private int gridHeight;
		private bool isGridInitialised;
		private float squareSize;
		private float squareWorldPositionX;
		private float squareWorldPositionY;


		public void Initialise (MarchingSquaresModel model)
		{
			this.model = model;
			this.playerPosition = new Vector3(30, 35, 0);
			this.squareOffset = Vector3.zero;

			squareSize = TileSize;
			gridWidth = Mathf.CeilToInt (model.ScreenWidth / squareSize) + 2;
			gridHeight = Mathf.CeilToInt (model.ScreenHeight / squareSize) + 2;

			screenBounds = new Bounds(Player.position, new Vector3(model.ScreenWidth, model.ScreenHeight, 0));
			GenerateGrid();
			GenerateMesh ();
			MarchingSquares();
			TiledSurface.Draw();
		}

		#region Setup
		private void GenerateGrid ()
		{
			grid = new GridSquare[gridWidth * gridHeight];

			for (int y = 0; y < gridHeight; y++)
			{
				for (int x = 0; x < gridWidth; x++)
				{
					int squareIndex = (y * gridWidth) + x;
					GridSquare square = new GridSquare ((x * squareSize), (y * squareSize), squareSize);
					square.leftNeighbour = (x == 0) ? null : grid [squareIndex - 1];
					square.bottomNeighbour = (y == 0) ? null : grid [squareIndex - gridWidth];
					grid [squareIndex] = square;
				}
			}
			isGridInitialised = true;
		}

		private void GenerateMesh ()
		{
			TiledSurface.Initialise(grid.Length);
		}

		#endregion

		#region Update Tilemap

		void Update()
		{
			if(isGridInitialised)
			{
				TiledSurface.Reset();
				UpdatePosition();
				MarchingSquares();
				TiledSurface.Draw();
			}
		}
			
		private void UpdatePosition()
		{
			playerPosition.x += Time.deltaTime * Speed * ThumbStick.HorizontalAxis;
			playerPosition.y += Time.deltaTime * Speed * ThumbStick.VerticalAxis;

			if(playerPosition.x < 0) playerPosition.x = 0;
			if(playerPosition.y < 0) playerPosition.y = 0;

			int currentSquareX = Mathf.FloorToInt(playerPosition.x/squareSize);

			squareWorldPositionX = currentSquareX * squareSize;
			squareWorldPositionY = Mathf.FloorToInt(playerPosition.y/squareSize) * squareSize;

			squareOffset.x = playerPosition.x - squareWorldPositionX;
			squareOffset.y = playerPosition.y - squareWorldPositionY;
		}

		private void MarchingSquares ()
		{

			for (int gridIndex = 0; gridIndex < grid.Length; gridIndex++)
			{
				GridSquare square = grid [gridIndex];
				SetCornerValues(square);
				int caseId = model.GetSquareConfiguration(square, TiledSurface.Threshold);
				TiledSurface.DrawCaseAtPoint(caseId, square, squareOffset);
			}
		}

		private void SetCornerValues (GridSquare square)
		{
			square.bottomLeftValue = 0;
			square.bottomRightValue = 0;
			square.topLeftValue = 0;
			square.topRightValue = 0;

			if (square.bottomNeighbour != null)
			{
				square.bottomRightValue = square.bottomNeighbour.topRightValue;
				square.bottomLeftValue = square.bottomNeighbour.topLeftValue;
			}
			if (square.leftNeighbour != null)
			{
				square.topLeftValue = square.leftNeighbour.topRightValue;

				if (square.bottomNeighbour == null)
					square.bottomLeftValue = square.leftNeighbour.bottomRightValue;
			}

			if (square.leftNeighbour == null && square.bottomNeighbour == null)
				square.bottomLeftValue += GetValueForPoint (square.bottomLeftCorner);
			if (square.bottomNeighbour == null)
				square.bottomRightValue += GetValueForPoint (square.bottomRightCorner);
			if (square.leftNeighbour == null)
				square.topLeftValue += GetValueForPoint (square.topLeftCorner);
			square.topRightValue += GetValueForPoint (square.topRightCorner);

		}

		private float GetValueForPoint (Vector2 point)
		{
			return Mathf.PerlinNoise((point.x + squareWorldPositionX)/SCALE, (point.y + squareWorldPositionY)/SCALE) * NOISE_MULTIPLIER;
		}
		#endregion
	}
}