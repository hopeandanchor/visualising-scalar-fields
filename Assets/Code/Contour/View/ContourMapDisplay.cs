using UnityEngine;
using System.Collections;
using HopeAndAnchor.Shared.Model;
using HopeAndAnchor.Shared.View;

namespace HopeAndAnchor.Contour.View
{
	public class ContourMapDisplay : MonoBehaviour
	{
		
		[SerializeField] private int GridResolution;
		[SerializeField] private Isosurface[] Meshes;
		[SerializeField] private bool SmoothEdges = true;


		private MarchingSquaresModel model;
		private int gridWidth;
		private int gridHeight;
		private GridSquare[] grid;
		private Vector3[] vertices;
		private Vector3 centrePoint;
		private Mesh mesh;
		private int[] triangles;

		public void Initialise (MarchingSquaresModel model)
		{
			this.model = model;
			this.centrePoint = new Vector3(model.ScreenWidth/2, model.ScreenHeight/2, 0);
			GenerateGrid ();
			GenerateMesh ();
			MarchingSquares ();
		}

		void GenerateGrid ()
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
					SetCornerValues(square);
					grid [squareIndex] = square;
				}
			}
		}

		void GenerateMesh ()
		{
			// Being a bit lazy with the array size here.
			// Needs to be enough vertices/triangles to cover
			// any combination of squares. 6 is the max number of vertices needed for each square
			// and triangle array size needs to be a multiple of 3, so being lazy and using 6.
			int verticesLength = 6 * GridResolution * GridResolution; 
			foreach (Isosurface mesh in Meshes)
			{
				mesh.Initialise (verticesLength, verticesLength, SmoothEdges);
			}
		}

		void MarchingSquares ()
		{
			foreach (Isosurface mesh in Meshes)
			{
				mesh.Reset ();
			}

			for (int gridIndex = 0; gridIndex < grid.Length; gridIndex++)
			{
				GridSquare square = grid [gridIndex];
				foreach (Isosurface mesh in Meshes)
				{
					int caseIndex = model.GetSquareConfiguration (square, mesh.ThresholdValue);

					Vector3[] squareVertices = model.CaseVertices [caseIndex];
					int[] squareTriangles = model.CaseTriangles [caseIndex];

					mesh.AddTriangles (squareTriangles);
					mesh.AddVertices (squareVertices, square);
				}
			}
			foreach (Isosurface mesh in Meshes)
			{
				mesh.DrawMesh ();
			}

		}

		void SetCornerValues (GridSquare square)
		{
			square.bottomLeftValue = GetValueForPoint (square.bottomLeftCorner);
			square.bottomRightValue = GetValueForPoint (square.bottomRightCorner);
			square.topLeftValue = GetValueForPoint (square.topLeftCorner);
			square.topRightValue = GetValueForPoint (square.topRightCorner);

		}

		float GetValueForPoint (Vector2 point)
		{
			float xDist = point.x - centrePoint.x;
			float yDist = point.y - centrePoint.y;
			float distanceFromCentre = Mathf.Sqrt((xDist * xDist) + (yDist * yDist));
			int tan45 = 1;
			return (model.ScreenHeight/2) - (distanceFromCentre / tan45);
		}
	}
}