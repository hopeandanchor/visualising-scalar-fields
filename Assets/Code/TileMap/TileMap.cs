using UnityEngine;
using System.Collections;
using HopeAndAnchor.TileMap.View;
using HopeAndAnchor.Shared.Model;
using HopeAndAnchor.Shared.View;

namespace HopeAndAnchor.TileMap
{
	public class TileMap : MonoBehaviour
	{
		[SerializeField] private TileMapDisplay TileMapDisplay;
		[SerializeField] private bool UseAlternative5And10Cases;

		private MarchingSquaresModel model;

		void Awake()
		{
			Application.targetFrameRate = 60;

			model = new MarchingSquaresModel(UseAlternative5And10Cases);
			TileMapDisplay.Initialise(model);
		}
	}
}
