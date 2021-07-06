using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//มือถือ
//zoom สลับ
//zoom speed -40%
//spin ar -35%
//spin normal -55%
//เปิด ปืดพื้น


public class TouchZoom_ : MonoBehaviour {
	public static bool isZooming = false;
	public float fieldOfView = 0.0f;
	public bool userActive = false;
	public float delta = 0.005f;
	public float max = 0.0f;
	public float min = 0.0f;
	public float speed = 0.0f;
	public bool isCanZoom = true;
	public bool isViewCanZoom = true;

	public Camera cam;
	public Transform root;
	// Update is called once per frame
	public void OnReset()
	{
		fieldOfView = def;
	}
	float def;
	void Start()
	{
		if(cam!=null) def = cam.fieldOfView;
		if (root != null) def = root.localScale.x;
		fieldOfView = def;
		volume = def;
	}
	void Update()
	{
		if (Input.touchCount == 0)
			isZooming = false;

		//isZooming = ( Input.GetKey(KeyCode.W ));

		#region UserZoom-Input
		if (isCanZoom)
		if (isViewCanZoom) {
			// If there are two touches on the device...
			if (Input.touchCount == 2) {
				userActive = true;
				isZooming = true;
				// Store both touches.
				Touch touchZero = Input.GetTouch (0);
				Touch touchOne = Input.GetTouch (1);

				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
				fieldOfView += deltaMagnitudeDiff * delta;
			}


			if (Input.GetAxis ("Mouse ScrollWheel") > 0f) { // forward
				userActive = true;
				fieldOfView += delta * 20.0f;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0f) { // forward
				userActive = true;
				fieldOfView -= delta * 20.0f;
			}
		}
		#endregion



		if (fieldOfView < min) {
			fieldOfView = min;
		}
		if (fieldOfView > max) {
			fieldOfView = max;
		}


		volume = Mathf.Lerp(volume, fieldOfView, Time.deltaTime * speed);


		if (cam != null) 
		{
			cam.fieldOfView = volume;
		} 

		if (root != null) 
		{
			
			root.transform.localScale = Vector3.one * volume;
		}

	}

	float volume = 0.0f;
}


