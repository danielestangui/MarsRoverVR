using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverCameraController : MonoBehaviour {

	public GameObject _pivot;

	public float _rotationSpeed;
	public float _raiseSpeed;
	public float _zoomSpeed;
	public float _minZoomDistance;
	public float _maxZoomDistance;


	void Update () {
		// Up or Down
		transform.Translate (Vector3.up * Input.GetAxis ("Vertical") * Time.deltaTime * _raiseSpeed);

		float horizontal = Input.GetAxis ("Horizontal") * _rotationSpeed * (-1);
		transform.RotateAround (_pivot.transform.position,Vector3.up, horizontal * Time.deltaTime);
		transform.LookAt (_pivot.transform.position);

		// ZOOM
		float distance = Vector3.Distance (transform.position, _pivot.transform.position);

		// +Zoom 
		if(Input.GetKey(KeyCode.E)){
			if(distance > _minZoomDistance){
				transform.Translate (Vector3.forward * Time.deltaTime * _zoomSpeed);
			}
		}
		// -Zoom
		if(Input.GetKey(KeyCode.R))
		{
			if(distance < _maxZoomDistance){
				transform.Translate (-Vector3.forward * Time.deltaTime * _zoomSpeed);
			}
		}

	}
}
