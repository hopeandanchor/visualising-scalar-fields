using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	[SerializeField] private float Speed = 8;
	private Vector3 playerPosition;


	void Start()
	{
		playerPosition = transform.position;
	}

	void Update () {
	
		playerPosition.x += Speed * Time.deltaTime * Input.GetAxis("Horizontal");
		playerPosition.y += Speed * Time.deltaTime * Input.GetAxis("Vertical");
		transform.position = playerPosition;
	}
}
