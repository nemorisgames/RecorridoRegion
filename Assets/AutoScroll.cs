using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour {
	private static AutoScroll _instance;
	public static AutoScroll Instance { get { return _instance;}}
	public bool fijarEnUsuario = false;
	private UICenterOnChild centerOnChild;
	public bool inBounds;
	
	void Awake(){
		if(_instance == null)
			_instance = this;
		centerOnChild = GetComponent<UICenterOnChild>();
		centerOnChild.enabled = false;
		fijarEnUsuario = false;
	}
	void Update () {
		inBounds = GPS.Instance.UserInBounds();
		if(fijarEnUsuario)
			FijarEnUsuario();
	}

	void FijarEnUsuario(){
		if(GPS.Instance.UserInBounds())
			centerOnChild.CenterOn(GPS.Instance.userLocation.point);
	}

	public void GoToUser(){
		if(GPS.Instance.UserInBounds() && fijarEnUsuario){
			centerOnChild.CenterOn(GPS.Instance.userLocation.point);
			StartCoroutine(GoToUpdate());
		}
	}

	public void ToggleLockUser(){
		fijarEnUsuario = !fijarEnUsuario;
		centerOnChild.enabled = fijarEnUsuario;
		GUIController.Instance.UIBotonLock(fijarEnUsuario);
	}

	public void StartOnUser(){
		//Debug.Log("here " +GPS.Instance.UserInBounds());
		if(!GPS.Instance.UserInBounds())
			return;
		centerOnChild.CenterOn(GPS.Instance.userLocation.point);
		StartCoroutine(GoTo());
	}

	IEnumerator GoTo(){
		fijarEnUsuario = true;
		centerOnChild.enabled = true;
		yield return new WaitForEndOfFrame();
		fijarEnUsuario = false;
		centerOnChild.enabled = false;
	}

	IEnumerator GoToUpdate(){
		centerOnChild.enabled = true;
		yield return new WaitForEndOfFrame();
		centerOnChild.enabled = false;
	}

	public IEnumerator DelayStartOnUser(){
		yield return new WaitForEndOfFrame();
		StartOnUser();
	}
}
