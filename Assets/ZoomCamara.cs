using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamara : MonoBehaviour {
	Camera camera;
	public float zoomFactor = 20f;
	public float minZoom = 200f;
	public float maxZoom = 400f;

	// Use this for initialization
	void Awake () {
		camera = GetComponent<Camera>();
	}
	
	public void ZoomIn(){
		Zoom(true);
	}

	public void ZoomOut(){
		Zoom(false);
	}

	void Zoom(bool b){
		float zoom = camera.orthographicSize + (b ? -1 : 1 )*zoomFactor;
		camera.orthographicSize = Mathf.Clamp(zoom,minZoom,maxZoom);
	}
}
