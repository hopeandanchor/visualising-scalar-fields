using UnityEngine;
using System.Collections;

namespace HopeAndAnchor.Shared.Model
{
	public class MarchingSquaresModel
	{

		public float ScreenWidth { private set; get; }
		public float ScreenHeight { private set; get; }

		public Vector3[][] CaseVertices { private set; get; }
		public int[][] CaseTriangles { private set; get; }

		private bool useAlternate5And10;

		public MarchingSquaresModel (bool useAlternate5And10)
		{
			this.useAlternate5And10 = useAlternate5And10;
			SetScreenBounds ();
			GenerateCases ();
		}

		void SetScreenBounds ()
		{
			ScreenHeight = Camera.main.orthographicSize*2;
			ScreenWidth = ScreenHeight * Camera.main.aspect;
		}

		public int GetSquareConfiguration (GridSquare square, float threshold)
		{
			int caseId = 0;
			if (square.bottomLeftValue >= threshold)
			{
				caseId |= 1;
			}
			if (square.bottomRightValue >= threshold)
			{
				caseId |= 2;
			}
			if (square.topRightValue >= threshold)
			{
				caseId |= 4;
			}
			if (square.topLeftValue >= threshold)
			{
				caseId |= 8;
			}
			return caseId;
		}

		#region Case Generation

		/**
		 * Written in this way to be easy to understand. 
		 * If you use this in a project, you might want to condense this down - it's just static data.
		*/
		private void GenerateCases ()
		{
			CaseVertices = new Vector3[16][];
			CaseTriangles = new int[16][];

			GenerateCase0 ();
			GenerateCase1 ();
			GenerateCase2 ();
			GenerateCase3 ();
			GenerateCase4 ();
			GenerateCase5 ();
			GenerateCase6 ();
			GenerateCase7 ();
			GenerateCase8 ();
			GenerateCase9 ();
			GenerateCase10 ();
			GenerateCase11 ();
			GenerateCase12 ();
			GenerateCase13 ();
			GenerateCase14 ();
			GenerateCase15 ();
		}

		private void GenerateCase0 ()
		{
			CaseVertices [0] = new Vector3[]{ };
			CaseTriangles [0] = new int[]{ };
		}

		private void GenerateCase1 ()
		{
			CaseVertices [1] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 1, 0),
				new Vector3 (1, 0, 0)
			};
			CaseTriangles [1] = new int[] {
				0, 1, 2
			};
			
		}

		private void GenerateCase2 ()
		{
			CaseVertices [2] = new Vector3[] {
				new Vector3 (1, 0, 0),
				new Vector3 (2, 1, 0),
				new Vector3 (2, 0, 0)
			};
			CaseTriangles [2] = new int[] {
				0, 1, 2
			};
			
		}

		private void GenerateCase3 ()
		{
			CaseVertices [3] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 1, 0),
				new Vector3 (2, 1, 0),
				new Vector3 (2, 0, 0)
			};
			CaseTriangles [3] = new int[] {
				0, 1, 2,
				0, 2, 3

			};
			
		}

		private void GenerateCase4 ()
		{
			CaseVertices [4] = new Vector3[] {
				new Vector3 (1, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (2, 1, 0)
			};
			CaseTriangles [4] = new int[] {
				0, 1, 2

			};
			
		}

		private void GenerateCase5 ()
		{
			if(useAlternate5And10)
			{
				CaseVertices [5] = new Vector3[] {
					new Vector3 (0, 1, 0),
					new Vector3 (0, 2, 0),

					new Vector3 (1, 2, 0),

					new Vector3 (2, 1, 0),
					new Vector3 (2, 0, 0),
					new Vector3 (1, 0, 0)
				};
				CaseTriangles [5] = new int[] {
					0, 1, 5,
					1, 4, 5,
					1, 2, 4,
					2, 3, 4

				};
			}
			else
			{
				CaseVertices [5] = new Vector3[] {
					new Vector3 (0, 0, 0),
					new Vector3 (0, 1, 0),
					new Vector3 (1, 0, 0),

					new Vector3 (1, 2, 0),
					new Vector3 (2, 2, 0),
					new Vector3 (2, 1, 0)
				};
				CaseTriangles [5] = new int[] {
					0, 1, 2,
					3, 4, 5

				};
			}
			
		}

		private void GenerateCase6 ()
		{
			CaseVertices [6] = new Vector3[] {
				new Vector3 (1, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (1, 0, 0),
				new Vector3 (2, 0, 0)
			};
			CaseTriangles [6] = new int[] {
				0, 1, 2,
				2, 1, 3
			};
			
		}

		private void GenerateCase7 ()
		{
			CaseVertices [7] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 1, 0),
				new Vector3 (1, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (2, 0, 0)
			};
			CaseTriangles [7] = new int[] {
				0, 1, 2,
				2, 3, 0,
				0, 3, 4
			};
			
		}

		private void GenerateCase8 ()
		{
			CaseVertices [8] = new Vector3[] {
				new Vector3 (0, 1, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (1, 2, 0)
			};
			CaseTriangles [8] = new int[] {
				0, 1, 2

			};
			
		}

		private void GenerateCase9 ()
		{
			CaseVertices [9] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (1, 2, 0),
				new Vector3 (1, 0, 0)
			};

			CaseTriangles [9] = new int[] {
				0, 1, 2,
				0, 2, 3,

			};
			
		}

		private void GenerateCase10 ()
		{
			if(useAlternate5And10)
			{
				CaseVertices [10] = new Vector3[] {
					new Vector3 (0, 0, 0),
					new Vector3 (0, 1, 0),
					new Vector3 (1, 2, 0),
					new Vector3 (2, 2, 0),
					new Vector3 (2, 1, 0),
					new Vector3 (1, 0, 0)
				};

				CaseTriangles [10] = new int[] {
					0, 1, 2,
					0, 2, 3,
					0, 3, 4,
					0, 4, 5

				};
			}
			else
			{
				CaseVertices [10] = new Vector3[] {
					new Vector3 (0, 1, 0),
					new Vector3 (0, 2, 0),
					new Vector3 (1, 2, 0),
					new Vector3 (1, 0, 0),
					new Vector3 (2, 1, 0),
					new Vector3 (2, 0, 0)
				};

				CaseTriangles [10] = new int[] {
					0, 1, 2,
					3, 4, 5,

				};

			}
			
		}

		private void GenerateCase11 ()
		{
			CaseVertices [11] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (1, 2, 0),
				new Vector3 (2, 1, 0),
				new Vector3 (2, 0, 0)
			};

			CaseTriangles [11] = new int[] {
				0, 1, 4,
				1, 2, 4,
				2, 3, 4

			};
			
		}

		private void GenerateCase12 ()
		{
			CaseVertices [12] = new Vector3[] {
				new Vector3 (0, 1, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (2, 1, 0)
			};

			CaseTriangles [12] = new int[] {
				0, 1, 2,
				0, 2, 3

			};
			
		}

		private void GenerateCase13 ()
		{
			CaseVertices [13] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (2, 1, 0),
				new Vector3 (1, 0, 0)
			};

			CaseTriangles [13] = new int[] {
				0, 1, 2,
				0, 2, 3,
				0, 3, 4,

			};
			
		}

		private void GenerateCase14 ()
		{
			CaseVertices [14] = new Vector3[] {
				new Vector3 (0, 1, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (2, 0, 0),
				new Vector3 (1, 0, 0)
			};

			CaseTriangles [14] = new int[] {
				1, 2, 3,
				0, 1, 3,
				0, 3, 4,

			};
			
		}

		private void GenerateCase15 ()
		{
			CaseVertices [15] = new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (0, 2, 0),
				new Vector3 (2, 2, 0),
				new Vector3 (2, 0, 0)
			};

			CaseTriangles [15] = new int[] {
				0, 1, 2,
				0, 2, 3

			};
			
		}

		#endregion
	}
}