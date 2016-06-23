using UnityEngine;
using System.Collections;
using HopeAndAnchor.MetaBalls.View;
using HopeAndAnchor.Shared.Model;
using HopeAndAnchor.Shared.View;

namespace HopeAndAnchor.MetaBalls
{
	public class Metaballs : MonoBehaviour
	{
		[SerializeField] private MetaballsDisplay MetaballsDisplay;
		[SerializeField] private bool UseAlternative5And10Cases;

		private MarchingSquaresModel model;

		void Awake()
		{
			Application.targetFrameRate = 60;

			model = new MarchingSquaresModel(UseAlternative5And10Cases);
			MetaballsDisplay.Initialise(model);
		}
	}
}
