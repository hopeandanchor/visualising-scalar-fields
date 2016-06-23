using System;
using UnityEngine;
using HopeAndAnchor.Shared.Model;

namespace HopeAndAnchor.Shared.View
{
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class Isosurface : MonoBehaviour
	{
		[SerializeField] private float Threshold = 1f;

		//Vertices are numbered from 0 to 2 (see the MarchingSquaresModel class)
		private const int MAX_VERTEX_VALUE = 2;

		private Mesh mesh;
		private Vector3[] vertices;
		private int[] triangles;
		private int vertexCount;
		private int triangleCount;
		private Vector3 point;
		private bool lerpEdges;


		public void Initialise (int numberVertices, int numberTriangles, bool lerpEdges = true)
		{
			this.lerpEdges = lerpEdges;
			vertices = new Vector3[numberVertices];
			triangles = new int[numberTriangles];

			vertexCount = 0;
			triangleCount = 0;

			mesh = new Mesh ();
			mesh.MarkDynamic ();
			GetComponent<MeshFilter> ().mesh = mesh;
		}

		public void Reset()
		{
			vertexCount = 0;
			triangleCount = 0;
		}

		public void AddVertices(Vector3[] newVertices, GridSquare square)
		{
			for (int vertexIndex = 0; vertexIndex < newVertices.Length; vertexIndex++)
			{
				point = newVertices [vertexIndex];
				if(lerpEdges)
					point = LerpMidEdgePoints (point, square);
				point.x = (point.x * (square.size / 2)) + square.bottomLeftCorner.x;
				point.y = (point.y * (square.size / 2)) + square.bottomLeftCorner.y;
				vertices [vertexCount + vertexIndex] = point;
			}
			vertexCount += newVertices.Length;

		}

		public Vector2 LerpMidEdgePoints (Vector2 point, GridSquare square)
		{
			if (point.x == 0 && point.y == 1)
			{
				//interpolate X=0, Y is mid point
				point.y = ((Threshold - square.bottomLeftValue) / (square.topLeftValue - square.bottomLeftValue)) * MAX_VERTEX_VALUE;
			}
			else if (point.x == 2 && point.y == 1)
			{
				//interpolate X=2, Y is mid point
				point.y = ((Threshold - square.bottomRightValue) / (square.topRightValue - square.bottomRightValue)) * MAX_VERTEX_VALUE;
			}
			else if (point.x == 1 && point.y == 0)
			{
				//interpolate X is midpoint, Y=0
				point.x = ((Threshold - square.bottomLeftValue) / (square.bottomRightValue - square.bottomLeftValue)) * MAX_VERTEX_VALUE;
			}
			else if (point.x == 1 && point.y == 2)
			{
				//interpolate X is midpoint, Y=2
				point.x = ((Threshold - square.topLeftValue) / (square.topRightValue - square.topLeftValue)) * MAX_VERTEX_VALUE;
			}
			return point;
		}


		public void AddTriangles(int[] newTriangles)
		{
			for (int triangleIndex = 0; triangleIndex < newTriangles.Length; triangleIndex++)
			{
				triangles [triangleCount + triangleIndex] = newTriangles [triangleIndex] + vertexCount;
			}
			triangleCount += newTriangles.Length;
		}

		public void DrawMesh()
		{
			Array.Clear (vertices, vertexCount, vertices.Length - vertexCount);
			Array.Clear (triangles, triangleCount, triangles.Length - triangleCount);

			mesh.Clear (false);
			mesh.vertices = vertices;
			mesh.triangles = triangles;
		}

		public float ThresholdValue
		{
			get
			{
				return Threshold;
			}
		}
	}
}


