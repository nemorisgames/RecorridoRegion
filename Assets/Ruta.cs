using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Ruta : MonoBehaviour {
	public List<PuntoRuta> puntos;
	public List<Texture2D> premios;
	public int indiceRuta;
	public int experienciaRuta = 0;
	private int expPrincipal = 50;
	private int expSecundario = 25;

	// Use this for initialization
	void Start () {
		experienciaRuta = PlayerPrefs.GetInt("expRuta"+indiceRuta,0);
		for(int i = 0; i < puntos.Count; i++)
			puntos[i].InitPunto(i,this);
	}

	public void CheckPunto(int i){
		PuntoRuta p = puntos[i];
		if(p.puntoVisitado){
			//mostrar texto
		}
		else{
			//guardar estado
			p.PuntoVisitado();
			//asignar exp
			if(p.tipo == PuntoRuta.Tipo.Principal)
				experienciaRuta += expPrincipal;
			else
				experienciaRuta += expSecundario;
			PlayerPrefs.SetInt("expRuta"+indiceRuta,experienciaRuta);
			if(experienciaRuta > 50){
				//mostrar pantalla premio
				DesbloquearPremio();
			}
		}
	}

	void OnApplicationQuit(){
		ActualizarEstado();
	}

	void ActualizarEstado(){
		//Guarda exp ruta
		PlayerPrefs.SetInt("expRuta"+indiceRuta,experienciaRuta);
		//Guarda estado visita puntos
		for(int i = 0; i < puntos.Count; i++){
			PlayerPrefs.SetInt("visitadoPunto"+i,(puntos[i].puntoVisitado ? 1 : 0));
		}
	}

	void DesbloquearPremio(){
		if(premios.Count == 0)
			return;
		byte [] data = ImageConversion.EncodeToPNG(premios[0]);
		File.WriteAllBytes("test.png",data);
	}
}
