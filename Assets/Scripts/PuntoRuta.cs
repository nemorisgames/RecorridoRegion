using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenColor))]
public class PuntoRuta : MonoBehaviour {
	public enum Tipo{
		Principal,
		Secundario
	}

	public enum Region{
		IX,
		XIV,
		X
	}

	public Region region;

	public Tipo tipo = Tipo.Secundario;
	public int indicePunto;
	public bool puntoVisitado = false;
	[HideInInspector]
	public Ruta ruta;
	private TweenColor tweenColor;
	private UI2DSprite sprite;
	public string nombre;
	public string descripcion;
	public List<UI2DSprite> imagenes;
	public int expPunto = 0;

	void Awake(){
		tweenColor = GetComponent<TweenColor>();
		sprite = GetComponent<UI2DSprite>();
	}

	void Start(){
		tweenColor.from = sprite.color;
	}

	public void InitPunto(int i, Ruta r){
		ruta = r;
		indicePunto = i;
		puntoVisitado = PlayerPrefs.GetInt("visitadoPunto"+indicePunto) == 1;
		expPunto = (tipo == Tipo.Principal ? ruta.expPrincipal : ruta.expSecundario);
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
		ruta.CheckPunto(indicePunto);
	}

	public void ExitPoint(){
		tweenColor.PlayReverse();
	}

	public void ClickHito(){
		GUIController.Instance.MostrarHito(this);
	}
}
