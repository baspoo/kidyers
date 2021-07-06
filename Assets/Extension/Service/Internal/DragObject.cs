using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {
	public Camera camera;
	public GameObject DragThis;
	public bool isLock_x;
	public bool isLock_y;
	public bool isLock_z;
	public float deltaFoce = 1.0f;

	public Vector3 StartPost;
	void Start(){
		if (camera == null)
			camera = Camera.main;
		if (StartPost == Vector3.zero)
			StartPost = transform.position;
	}


	public SpriteRenderer spr;
	public Color[] color;
	public float timeHold;

	float time=0.0f;
	bool isDown = false;
	private Vector3 screenPoint;
	private Vector3 offset;
	void OnMouseDown()
	{
		
		if (DragThis == null || !enabled || Input.touchCount > 1)
			return;
		
		Debug.Log ("OnMouseDown");
		if(!TouchZoom_.isZooming)
		if (Input.touchCount != 2) 
			{
				isCanDrag = false;
				isDown = true;
			}
	}
	void OnMouseUp()
	{
		time = 0.0f;
		isCanDrag = false;
		isDown = false;
	}

	void OnEnable()
	{
		time = 0.0f;
		isCanDrag = false;
		isDown = false;
	}



	bool isCanDrag = false;

    private void Update()
    {
		if (isDown) 
		{
			time += Time.deltaTime;
			if (time >= timeHold) 
			{
				screenPoint = camera.WorldToScreenPoint(DragThis.transform.position);
				offset = DragThis.transform.position - camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				isCanDrag = true;
				isDown = false;
				time = 0.0f;
			}
		}

		spr.color =  color[(isCanDrag) ? 1 : 0];


	}


    void OnMouseDrag()
	{

		if (!isCanDrag)
			return;

		if (DragThis == null || !enabled || Input.touchCount > 1)
			return;
		
		if(!TouchZoom_.isZooming)
		if (Input.touchCount != 2) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = (camera.ScreenToWorldPoint (curScreenPoint)) + offset;
			if (isLock_x)
				curPosition.x = DragThis.transform.position.x;
			if (isLock_y)
				curPosition.y = DragThis.transform.position.y;
			if (isLock_z)
				curPosition.z = DragThis.transform.position.z;

			DragThis.transform.position = curPosition;

		}
	}

}
