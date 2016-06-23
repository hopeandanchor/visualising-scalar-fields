using UnityEngine;
using System.Collections;
using HopeAndAnchor.Shared.Model;
using HopeAndAnchor.Shared.View;

namespace HopeAndAnchor.MetaBalls.View
{
	public class MetaballsDisplay : MonoBehaviour
	{
		[SerializeField] private int GridResolution;
		[SerializeField] private int NumberCircles = 5;
		[SerializeField] private float minVelocity = 0.2f;
		[SerializeField] private float maxVelocity = 2f;
		[SerializeField] private float minRadius = 1.5f;
		[SerializeField] private float maxRadius = 0.7f;
		[SerializeField] private bool ShowDebugLines;
		[SerializeField] private Isosurface isosurface;

		private MarchingSquaresModel model;
		private Circle[] circles;
		private GridSquare[] grid;

		private int gridWidth;
		private int gridHeight;
		private bool isGridInitialised;

		#region Initialisation

		public void Initialise (MarchingSquaresModel model)
		{
			this.model = model;
			GenerateCircles ();
			GenerateMesh ();
			GenerateGrid ();
		}

		private void GenerateCircles ()
		{
			circles = new Circle[NumberCircles];

			for (int i = 0; i < NumberCircles; i++)
			{
				Circle circle = new Circle ();
				circle.radius = Random.Range (minRadius, maxRadius);
				circle.position = new Vector2 (Random.Range (0, model.ScreenWidth), Random.Range (0, model.ScreenHeight));
				circle.velocity = new Vector2 (Random.Range (minVelocity, maxVelocity), Random.Range (minVelocity, maxVelocity));
				if(circle.velocity.x > 1.2f) circle.velocity.x *= -1;
				if(circle.velocity.y > 1.2f) circle.velocity.y *= -1;
				circles [i] = circle;
			}
		}

		private void GenerateGrid ()
		{
			
			float squareSize = model.ScreenWidth / GridResolution;
			//Add 1 so the grid always covers the screen
			gridWidth = Mathf.CeilToInt (model.ScreenWidth / squareSize) + 1;
			gridHeight = Mathf.CeilToInt (model.ScreenHeight / squareSize) + 1;

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
			//Approximated a vertices/triangle length here. 
			//Triangle array length needs to be a multiple of 3
			int verticesLength = 3 * GridResolution * GridResolution; 
			verticesLength += 3 - (verticesLength % 3);
			isosurface.Initialise (verticesLength, verticesLength);

		}

		#endregion

		#region Marching Squares

		void Update ()
		{
			if (isGridInitialised)
			{
				MoveCircles ();
				MarchingSquares ();
			}
		}

		private void MoveCircles ()
		{
			for (int i = 0; i < NumberCircles; i++)
			{
				Circle circle = circles [i];
				circle.position += circle.velocity * Time.deltaTime;

				if ((circle.position.x > model.ScreenWidth && circle.velocity.x > 0) || (circle.position.x < 0 && circle.velocity.x < 0))
					circle.velocity.x = -circle.velocity.x;
				if ((circle.position.y > model.ScreenHeight && circle.velocity.y > 0) || (circle.position.y < 0 && circle.velocity.y < 0))
					circle.velocity.y = -circle.velocity.y;
			}
		}

		private void MarchingSquares ()
		{
			isosurface.Reset ();

			for (int gridIndex = 0; gridIndex < grid.Length; gridIndex++)
			{
				GridSquare square = grid [gridIndex];

				SetCornerValues (square);
				int caseIndex = model.GetSquareConfiguration (square, isosurface.ThresholdValue);

				Vector3[] squareVertices = model.CaseVertices [caseIndex];
				int[] squareTriangles = model.CaseTriangles [caseIndex];

				isosurface.AddTriangles (squareTriangles);
				isosurface.AddVertices (squareVertices, square);

			}
			isosurface.DrawMesh ();
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

			foreach (Circle circle in circles)
			{
				if (square.leftNeighbour == null && square.bottomNeighbour == null)
					square.bottomLeftValue += GetValueForPoint (square.bottomLeftCorner, circle);
				if (square.bottomNeighbour == null)
					square.bottomRightValue += GetValueForPoint (square.bottomRightCorner, circle);
				if (square.leftNeighbour == null)
					square.topLeftValue += GetValueForPoint (square.topLeftCorner, circle);
				square.topRightValue += GetValueForPoint (square.topRightCorner, circle);
			}
		}

		private float GetValueForPoint (Vector2 point, Circle circle)
		{
			float xDist = point.x - circle.position.x;
			float yDist = point.y - circle.position.y;
			return (circle.radius * circle.radius) / ((xDist * xDist) + (yDist * yDist));
		}

		#endregion

		#region Debug

		void OnDrawGizmos ()
		{
			if (isGridInitialised && ShowDebugLines)
			{
				for (int x = 0; x < gridWidth; x++)
				{
					Debug.DrawLine (new Vector3 ((x * grid [0].size), 0, 0), new Vector3 ((x * grid [0].size), model.ScreenHeight, 0), Color.cyan, 1f);
				}
				for (int y = 0; y < gridHeight; y++)
				{
					Debug.DrawLine (new Vector3 (0, (y * grid [0].size), 0), new Vector3 (model.ScreenWidth, (y * grid [0].size), 0), Color.cyan, 1f);
				}

				Gizmos.color = Color.red;
				foreach (Circle circle in circles)
					Gizmos.DrawWireSphere (circle.position, circle.radius);
			}
		}

		#endregion
	}
}