using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	//--------------------------------------------------------------------------------------------------
	// PUBLIC INSPECTOR PROPERTIES

	public Transform Pivot;
	[Header("Orbit")]
	public float OrbitStrg = 3f;
	public float OrbitClamp = 50f;
	public Vector2 AngleForce = Vector2.one;

	[Header("Panning")]
	public float PanStrg = 0.1f;
	public float PanClamp = 2f;
	float yMin, yMax;
	[Header("Zooming")]
	public Camera cam;
	public Transform LookAt;
	public float ZoomStrg = 40f;
	public float ZoomClamp = 30f;
	public float ZoomDistMin = 1f;
	public float ZoomDistMax = 2f;
	[Header("Misc")]
	public float Decceleration = 8f;
	public float DeccelerationMobile = 30f;
	public Rect ignoreMouseRect;


	//--------------------------------------------------------------------------------------------------
	// PRIVATE PROPERTIES
	private float m_decceleration;
	private Vector3 mouseDelta;
	public Vector3 orbitAcceleration;
	private Vector3 panAcceleration;
	private Vector3 moveAcceleration;
	private float zoomAcceleration;
	public  float XMax = 60;
	public  float XMin = 300;

	private Vector3 mResetCamPos, mResetPivotPos, mResetCamRot, mResetPivotRot;
	float mZoomviwe;

	bool leftMouseHeld;
	bool rightMouseHeld;
	bool middleMouseHeld;

	//--------------------------------------------------------------------------------------------------
	// UNITY EVENTS

	void Awake()
	{
		m_decceleration = Ismobile ? DeccelerationMobile : Decceleration;
		mResetCamPos = transform.position;
		mResetCamRot = transform.eulerAngles;
		mResetPivotPos = Pivot.position;
		mResetPivotRot = Pivot.eulerAngles;
		mZoomviwe = (cam!=null)?cam.fieldOfView:0.0f;
		zoomAcceleration = mZoomviwe;
	}

	void OnEnable()
	{
		mouseDelta = Input.mousePosition;
	}






	bool GetPosition() {

		float x_limit = Screen.width * limitmonitor.x;
		var x = Input.mousePosition.x;
		var l_limit = x_limit;
		var r_limit = Screen.width - x_limit;
		if (x < l_limit)
			return false;
		if (x > r_limit)
			return false;



		float y_limit = Screen.height * limitmonitor.y;
		var y = Input.mousePosition.y;
		l_limit = y_limit;
		r_limit = Screen.height - y_limit;
		if (y < l_limit)
			return false;
		if (y > r_limit)
			return false;


		return true;
	
	
	}
	public Vector2 limitmonitor;
	public bool isFreez;
	public bool Ismobile;
	public float clickhold = 0.0f;

	void Update()
	{
		

		if (isFreez)
			return;


		

		if (!leftMouseHeld && !GetPosition() )
			return;


		//clickhold += Time.deltaTime;
		//if (clickhold < 0.1f) return;


		mouseDelta = Input.mousePosition - mouseDelta;

		var rightAlignedRect = ignoreMouseRect;
		rightAlignedRect.x = Screen.width - ignoreMouseRect.width;
		var ignoreMouse = rightAlignedRect.Contains(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
			leftMouseHeld = !ignoreMouse;
		else if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)) { 
			leftMouseHeld = false;
			clickhold = 0.0f;
		}

		if (Input.GetMouseButtonDown(1))
			rightMouseHeld = !ignoreMouse;
		else if (Input.GetMouseButtonUp(1) || !Input.GetMouseButton(1))
			rightMouseHeld = false;

		if (Input.GetMouseButtonDown(2))
			middleMouseHeld = !ignoreMouse;
		else if (Input.GetMouseButtonUp(2) || !Input.GetMouseButton(2))
        {
			//mouseDelta = Vector3.zero;
			middleMouseHeld = false;
		}
			

		//Left Button held
		if (leftMouseHeld)
		{
			orbitAcceleration.x += Mathf.Clamp(mouseDelta.x * OrbitStrg, -OrbitClamp, OrbitClamp);
			orbitAcceleration.y += Mathf.Clamp(-mouseDelta.y * OrbitStrg, -OrbitClamp, OrbitClamp);
		}
		//Middle/Right Button held
		else if (middleMouseHeld || rightMouseHeld)
		{
			//panAcceleration.x += Mathf.Clamp(-mouseDelta.x * PanStrg, -PanClamp, PanClamp);
			panAcceleration.y += Mathf.Clamp(-mouseDelta.y * PanStrg, -PanClamp, PanClamp);
		}

		//Keyboard support
		//orbitAcceleration.x += Input.GetKey(KeyCode.LeftArrow) ? 15 : (Input.GetKey(KeyCode.RightArrow) ? -15 : 0);
		//orbitAcceleration.y += Input.GetKey(KeyCode.UpArrow) ? 15 : (Input.GetKey(KeyCode.DownArrow) ? -15 : 0);

		if (Input.GetKeyDown(KeyCode.R))
		{
			//ResetView();
		}



	

		//X Angle Clamping
		var angle = transform.localEulerAngles;
		if (angle.x < 180 && angle.x >= XMax && orbitAcceleration.y > 0) orbitAcceleration.y = 0;
		if (angle.x > 180 && angle.x <= XMin && orbitAcceleration.y < 0) orbitAcceleration.y = 0;

		//Rotate
		transform.RotateAround(Pivot.position, transform.right, orbitAcceleration.y * Time.deltaTime * AngleForce.y);
		transform.RotateAround(Pivot.position, Vector3.up, orbitAcceleration.x * Time.deltaTime * AngleForce.x);

	



			//Translate
			var pos = Pivot.transform.position;
		var yDiff = pos.y;
		pos.y += panAcceleration.y * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, yMin, yMax);
		yDiff = pos.y - yDiff;
		Pivot.transform.position = pos;

		pos = transform.position;
		pos.y += yDiff;
		transform.position = pos;


		//Zoom


		var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
		if (scrollWheel != 0) 
		{
			zoomAcceleration += scrollWheel * ZoomStrg * Time.deltaTime;
		}
		
		//zoomAcceleration = Mathf.Clamp(zoomAcceleration, -ZoomClamp, ZoomClamp);
		if (zoomAcceleration < ZoomDistMin) zoomAcceleration = ZoomDistMin;
		if (zoomAcceleration > ZoomDistMax) zoomAcceleration = ZoomDistMax;
		if(cam!=null) cam.fieldOfView = zoomAcceleration;
		/*
		var dist = Vector3.Distance(transform.position, Pivot.position);
		if ((dist >= ZoomDistMin && zoomAcceleration > 0) || (dist <= ZoomDistMax && zoomAcceleration < 0))
		{
			transform.Translate(Vector3.forward * zoomAcceleration * Time.deltaTime, Space.Self);
		}
		*/
		//Deccelerate
		orbitAcceleration = Vector3.Lerp(orbitAcceleration, Vector3.zero, m_decceleration * Time.deltaTime);
		panAcceleration = Vector3.Lerp(panAcceleration, Vector3.zero, m_decceleration * Time.deltaTime);
		//zoomAcceleration = Mathf.Lerp(zoomAcceleration, 0, m_decceleration * Time.deltaTime);
		moveAcceleration = Vector3.Lerp(moveAcceleration, Vector3.zero, m_decceleration * Time.deltaTime);



		if(LookAt!=null && cam != null)
			cam.transform.LookAt(LookAt);

		mouseDelta = Input.mousePosition;
	}

	public void ResetView()
	{
		moveAcceleration = Vector3.zero;
		orbitAcceleration = Vector3.zero;
		panAcceleration = Vector3.zero;
		zoomAcceleration = mZoomviwe;

		transform.position = mResetCamPos;
		transform.eulerAngles = mResetCamRot;
		Pivot.position = mResetPivotPos;
		Pivot.eulerAngles = mResetPivotRot;
	}
}

