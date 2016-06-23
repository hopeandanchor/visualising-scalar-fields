using System;
using UnityEngine;
using HopeAndAnchor.Shared.Model;

namespace HopeAndAnchor.TileMap.View
{
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class TiledSurface : MonoBehaviour
	{
		[SerializeField] private int TextureWidth = 220;
		[SerializeField] private int TextureHeight = 220;
		[SerializeField] private int TileWidth = 55;
		[SerializeField] private int TileHeight = 55;
		[SerializeField] private float ThresholdValue = 1;

		private Mesh mesh;
		private Vector2[] uvs;
		private Vector3[] vertices;
		private int[] triangles;

		private int vertexCount;
		private int uvCount;

		private Vector3 currentVertex;
		private Vector2 bottomLeftSquareVertex;

		private Texture2D texture;

		static private Vector3[] tileVertices = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (1, 1, 0)
		};

		static private int[] tileTriangles = new int[] {
			0, 2, 1,
			2, 3, 1
		};

		public void Initialise (int numberSquares)
		{
			uvs = new Vector2[numberSquares * 4];
			triangles = new int[numberSquares * 6];
			vertices = new Vector3[numberSquares * 4];

			SetTriangles();

			vertexCount = 0;
			uvCount = 0;

			mesh = new Mesh ();
			mesh.MarkDynamic ();
			GetComponent<MeshFilter> ().mesh = mesh;
		}


		private void SetTriangles()
		{
			int triangleCount = 0;

			for(int v = 0; v < vertices.Length; v+=4)
			{
				triangles [triangleCount] = tileTriangles [0] + v;
				triangles [triangleCount + 1] = tileTriangles [1] + v;
				triangles [triangleCount + 2] = tileTriangles [2] + v;
				triangles [triangleCount + 3] = tileTriangles [3] + v;
				triangles [triangleCount + 4] = tileTriangles [4] + v;
				triangles [triangleCount + 5] = tileTriangles [5] + v;
				triangleCount += 6;
			}
		}

		public void Reset()
		{
			vertexCount = 0;
			uvCount = 0;
		}

		public void DrawCaseAtPoint (int caseId, GridSquare square, Vector3 offset)
		{

			SetVertices(square, offset);
			SetUVs(caseId);
			vertexCount += 4;
			uvCount += 4;

		}

		private void SetVertices(GridSquare square, Vector3 offset)
		{
			bottomLeftSquareVertex = square.bottomLeftCorner;
			for (int vertexIndex = 0; vertexIndex < tileVertices.Length; vertexIndex++)
			{
				currentVertex = tileVertices [vertexIndex];
				currentVertex.x = (currentVertex.x * square.size) + bottomLeftSquareVertex.x;
				currentVertex.y = (currentVertex.y * square.size) + bottomLeftSquareVertex.y;

				currentVertex.x -= offset.x;
				currentVertex.y -= offset.y;

				vertices [vertexCount + vertexIndex] = currentVertex;
			}
		}

		private void SetUVs(int caseId)
		{
			float leftCornerX = ((float)(TileWidth * (caseId%4))) / TextureWidth;
			float rightCornerX = ((float)(TileWidth * ((caseId%4) + 1))) / TextureWidth;
			int tileRow = Mathf.FloorToInt(caseId/4);
			float bottomCornerY = ((float)(TileHeight * tileRow)) / TextureHeight;
			float topCornerY = (float)(TileHeight * (tileRow + 1)) / TextureHeight;
			//Inside the tile by 0.01% to make sure we're within the bounds 

			uvs [uvCount].x = leftCornerX + 0.01f;
			uvs [uvCount].y = bottomCornerY + 0.01f;

			uvs [uvCount + 1].x = rightCornerX - 0.01f;
			uvs [uvCount + 1].y = bottomCornerY + 0.01f;

			uvs [uvCount + 2].x = leftCornerX + 0.01f;
			uvs [uvCount + 2].y = topCornerY - 0.01f;

			uvs [uvCount + 3].x = rightCornerX - 0.01f;
			uvs [uvCount + 3].y = topCornerY - 0.01f;
		}

		public void Draw ()
		{
			Array.Clear (vertices, vertexCount, vertices.Length - vertexCount);
			Array.Clear (uvs, uvCount, uvs.Length - uvCount);

			mesh.Clear (false);
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uvs;
		}

		public float Threshold
		{
			get
			{
				return ThresholdValue;
			}
		}

		public float TileSize
		{
			get
			{
				return TileWidth;
			}
		}
	}
}

