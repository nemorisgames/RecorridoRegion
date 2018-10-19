using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPoint : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "Point"){
			PuntoRuta p = c.GetComponent<PuntoRuta>();
			if(p != null){
				p.EnterPoint();
			}
		}
	}

	void OnTriggerExit2D(Collider2D c){
		if(c.tag == "Point"){
			PuntoRuta p = c.GetComponent<PuntoRuta>();
			if(p != null){
				p.ExitPoint();
			}
		}
	}
}
