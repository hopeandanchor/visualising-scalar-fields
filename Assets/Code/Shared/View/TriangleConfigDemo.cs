using UnityEngine;
using System.Collections;
using HopeAndAnchor.Shared.Model;

namespace HopeAndAnchor.Shared.View
{
	public class TriangleConfigDemo : MonoBehaviour {

		[SerializeField] private bool ShowDebugLines;

		private const int RESOLUTION = 8;
		private const float PADDING = 0.5f;
		private MarchingSquaresModel Model;
		private float squareSize;
		private bool isInitialised;

		public void Initialise (MarchingSquaresModel model)
		{
			this.Model = model;

			Mesh mesh = new Mesh();
			mesh.MarkDynamic ();
			GetComponent<MeshFilter> ().mesh = mesh;

			int[] triangles = new int[102];
			Vector3[] vertices = new Vector3[64];

			int currentCase = 0;
			int vertexCount =0;
			int triangleCount = 0;
			squareSize = Model.ScreenWidth / RESOLUTION;

			for (int row = 2; row >= 0; row--)
			{
				for (int column = 0; column < 6; column++)
				{
					if (currentCase >= 16)
					{
						break;
					}

					float currentX = (column * squareSize) + (squareSize / 2) + (PADDING * column);
					float currentY = (row * squareSize) + (squareSize / 2) + (PADDING * row);

					Vector3[] squareVertices = Model.CaseVertices [currentCase];
					int[] squareTriangles = Model.CaseTriangles [currentCase];

					for (int vertexIndex = 0; vertexIndex < squareVertices.Length; vertexIndex++)
					{
						Vector3 point = squareVertices [vertexIndex];
						point.x = (point.x * (squareSize / 2)) + currentX;
						point.y = (point.y * (squareSize / 2)) + currentY;
						vertices [vertexCount + vertexIndex] = point;
					}

					for (int triangleIndex = 0; triangleIndex < squareTriangles.Length; triangleIndex++)
					{
						triangles [triangleCount + triangleIndex] = squareTriangles [triangleIndex] + vertexCount;
					}

					currentCase++;
					vertexCount += squareVertices.Length;
					triangleCount += squareTriangles.Length;
				}
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			isInitialised = true;
		}

		#region Debug

		void OnDrawGizmos ()
		{
			if (ShowDebugLines && isInitialised)
			{
				for (int x = 0; x < 6; x++)
				{
					Debug.DrawLine (new Vector3 ((x * (squareSize + PADDING)) + (squareSize/2), 0, transform.position.z), new Vector3 ((x * (squareSize + PADDING)) + (squareSize/2), Model.ScreenHeight, transform.position.z), Color.cyan, 1f);
					Debug.DrawLine (new Vector3 ((x * (squareSize + PADDING)) + (squareSize*3/2), 0, transform.position.z), new Vector3 ((x * (squareSize + PADDING)) + (squareSize*3/2), Model.ScreenHeight, transform.position.z), Color.cyan, 1f);
				}
				for (int y = 0; y < 3; y++)
				{
					Debug.DrawLine (new Vector3 (0, (y * (squareSize + PADDING)) + (squareSize/2), transform.position.z), new Vector3 (Model.ScreenWidth, (y * (squareSize + PADDING)) + (squareSize/2), transform.position.z), Color.cyan, 1f);
					Debug.DrawLine (new Vector3 (0, (y * (squareSize + PADDING)) + (squareSize*3/2), transform.position.z), new Vector3 (Model.ScreenWidth, (y * (squareSize + PADDING)) + (squareSize*3/2), transform.position.z), Color.cyan, 1f);
				}
			}
		}

		#endregion
	}
}