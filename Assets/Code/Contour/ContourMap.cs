using UnityEngine;
using System.Collections;
using HopeAndAnchor.Contour.View;
using HopeAndAnchor.Shared.Model;
using HopeAndAnchor.Shared.View;

namespace HopeAndAnchor.Contour
{
	public class ContourMap : MonoBehaviour
	{
		[SerializeField] private ContourMapDisplay ContourMapDisplay;
		[SerializeField] private TriangleConfigDemo GridSquareCaseDisplay;
		[SerializeField] private bool UseAlternative5And10Cases;

		private MarchingSquaresModel model;

		void Awake()
		{
			Application.targetFrameRate = 60;

			model = new MarchingSquaresModel(UseAlternative5And10Cases);
			ContourMapDisplay.Initialise(model);
			GridSquareCaseDisplay.Initialise(model);
		}
	}
}
