using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
	public List<Sprite> imagenes;
	public int expPunto = 0;
	private UIButton btn;

	void Awake(){
		tweenColor = GetComponent<TweenColor>();
		sprite = GetComponent<UI2DSprite>();
		btn = GetComponent<UIButton>();
	}

	void Start(){
		//tweenColor.from = sprite.color;
	}

	public void InitPunto(int i, Ruta r){
		ruta = r;
		indicePunto = i;
		puntoVisitado = PlayerPrefs.GetInt("visitadoPunto"+indicePunto) == 1;
		expPunto = (tipo == Tipo.Principal ? ruta.expPrincipal : ruta.expSecundario);
		btn.defaultColor = r.color;
		btn.UpdateColor(true);
		tweenColor.from = btn.defaultColor;
		tweenColor.to = btn.pressed;
		tweenColor.ResetToBeginning();
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
		AnalyticsEvent.Custom("VisitaHito",new Dictionary<string, object>
		{
			{ "nombreHito", this.nombre },
			{ "ruta", this.ruta.nombre }
		});
		ruta.CheckPunto(indicePunto);
	}

	public void ExitPoint(){
		tweenColor.PlayReverse();
	}

	public void ClickHito(){
		//AnalyticsEvent.Custom(string customEventName, IDictionary<string, object> eventData);
		AnalyticsEvent.Custom("ClickHito",new Dictionary<string, object>
		{
			{ "nombreHito", this.nombre },
			{ "ruta", this.ruta.nombre }
		});
		GUIController.Instance.MostrarHito(this);
	}
}
