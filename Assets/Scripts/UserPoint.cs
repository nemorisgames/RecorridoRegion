using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPoint : MonoBehaviour {
	public Vector2 worldPos;

	void OnTriggerEnter(Collider c){
		if(c.tag == "Point"){
			PuntoRuta p = c.GetComponent<PuntoRuta>();
			if(p != null){
				p.EnterPoint();
			}
		}
	}

	void OnTriggerExit(Collider c){
		if(c.tag == "Point"){
			PuntoRuta p = c.GetComponent<PuntoRuta>();
			if(p != null){
				p.ExitPoint();
			}
		}
	}

	void Update(){
		worldPos = transform.position;
	}
}
