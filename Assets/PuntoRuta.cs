using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenColor))]
public class PuntoRuta : MonoBehaviour {
	public enum Tipo{
		Principal,
		Secundario
	}

	public Tipo tipo = Tipo.Secundario;
	public int indicePunto;
	public bool puntoVisitado = false;
	private Ruta ruta;
	private TweenColor tweenColor;

	void Awake(){
		tweenColor = GetComponent<TweenColor>();
	}

	public void InitPunto(int i, Ruta r){
		ruta = r;
		indicePunto = i;
		puntoVisitado = PlayerPrefs.GetInt("visitadoPunto"+indicePunto) == 1;
		if(puntoVisitado)
			PuntoVisitado();
	}

	public void PuntoVisitado(){
		if(!puntoVisitado){
			PlayerPrefs.SetInt("visitadoPunto"+indicePunto,1);
			puntoVisitado = true;
		}
		//marcar sprite como visitado
	}

	public void EnterPoint(){
		tweenColor.PlayForward();
	}

	public void ExitPoint(){
		tweenColor.PlayReverse();
	}
}
