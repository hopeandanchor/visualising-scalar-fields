using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThumbStick : MonoBehaviour
{
	[SerializeField] private Image Holder;
	[SerializeField] private Image Stick;

	public float HorizontalAxis { private set; get; }
	public float VerticalAxis { private set; get; }

	#if UNITY_EDITOR || UNITY_WEBGL
	private Vector3 startPosition;
	private Vector3 touchDeltaPosition;
	#elif UNITY_IOS || UNITY_ANDROID
	private Vector2 startPosition;
	private Vector2 touchDeltaPosition;
	private bool touching;
	#endif

	private float maxTouchRadius = 100f;

	void Awake()
	{
		maxTouchRadius = Holder.rectTransform.rect.width/2;
	}

	#if UNITY_EDITOR || UNITY_WEBGL 
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) 
		{
			startPosition = Input.mousePosition;
			touchDeltaPosition = Vector3.zero;
			transform.position = startPosition;
			Holder.gameObject.SetActive(true);
			Stick.gameObject.SetActive(true);
		}
		else if (Input.GetMouseButtonUp(0)) 
		{
			Holder.gameObject.SetActive(false);
			Stick.gameObject.SetActive(false);
			HorizontalAxis = 0;
			VerticalAxis = 0;
		}

		if (Input.GetMouseButton(0)) 
		{
			touchDeltaPosition = Input.mousePosition - startPosition;
			float distanceFromStartSqrd = (touchDeltaPosition.x * touchDeltaPosition.x) + (touchDeltaPosition.y * touchDeltaPosition.y);
			float maxDistance = maxTouchRadius*maxTouchRadius;

			if(distanceFromStartSqrd > maxDistance)
			{
				float srtDiff = Mathf.Sqrt(maxDistance/distanceFromStartSqrd);
				touchDeltaPosition.x = touchDeltaPosition.x * srtDiff;
				touchDeltaPosition.y = touchDeltaPosition.y * srtDiff;
			}

			HorizontalAxis = Mathf.Lerp(-1, 1, ((touchDeltaPosition.x+maxTouchRadius)/(2*maxTouchRadius)));
			VerticalAxis = Mathf.Lerp(-1, 1, ((touchDeltaPosition.y+maxTouchRadius)/(2*maxTouchRadius)));	

			Stick.rectTransform.localPosition = touchDeltaPosition;
		}

	}

	#elif UNITY_ANDROID || UNITY_IOS 

	private void Update()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			startPosition = Input.mousePosition;
			touchDeltaPosition = Vector3.zero;
			transform.position = startPosition;
			Holder.gameObject.SetActive(true);
			Stick.gameObject.SetActive(true);
			touching = true;
		}

		if (Input.touchCount > 0 && ((Input.GetTouch(0).phase == TouchPhase.Canceled) || (Input.GetTouch(0).phase == TouchPhase.Ended)))
		{
			touching = false;
			Holder.gameObject.SetActive(false);
			Stick.gameObject.SetActive(false);
			HorizontalAxis = 0;
			VerticalAxis = 0;
		}

		if (touching) 
		{
			touchDeltaPosition = Input.GetTouch(0).position - startPosition;
			float distanceFromStartSqrd = (touchDeltaPosition.x * touchDeltaPosition.x) + (touchDeltaPosition.y * touchDeltaPosition.y);
			float maxDistance = maxTouchRadius*maxTouchRadius;

			if(distanceFromStartSqrd > maxDistance)
			{
				float srtDiff = Mathf.Sqrt(maxDistance/distanceFromStartSqrd);
				touchDeltaPosition.x = touchDeltaPosition.x * srtDiff;
				touchDeltaPosition.y = touchDeltaPosition.y * srtDiff;
			}

			HorizontalAxis = Mathf.Lerp(-1, 1, ((touchDeltaPosition.x+maxTouchRadius)/(2*maxTouchRadius)));
			VerticalAxis = Mathf.Lerp(-1, 1, ((touchDeltaPosition.y+maxTouchRadius)/(2*maxTouchRadius)));
			Stick.rectTransform.localPosition = touchDeltaPosition;


		}
	}

	#endif

}
