using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Ruta : MonoBehaviour {
	public List<PuntoRuta> puntos;
	public List<Texture2D> premios;
	public int indiceRuta;
	public int experienciaRuta = 0;
	public int expTotalRuta = 0;
	public int expPrincipal = 50;
	public int expSecundario = 25;

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll();
		experienciaRuta = PlayerPrefs.GetInt("expRuta"+indiceRuta,0);
		for(int i = 0; i < puntos.Count; i++){
			puntos[i].InitPunto(i,this);
			expTotalRuta += puntos[i].expPunto;
		}
			
	}

	//Al visitar un punto
	public void CheckPunto(int i){
		PuntoRuta p = puntos[i];
		//no primera vez
		if(p.puntoVisitado){
			//mostrar texto
		}
		//primera vez
		else{
			//guardar estado
			p.PuntoVisitado();
			//asignar exp
			experienciaRuta += p.expPunto;
			PlayerPrefs.SetInt("expRuta"+indiceRuta,experienciaRuta);
			if(experienciaRuta > 50){
				//mostrar pantalla premio
				DesbloquearPremio();
			}
			GUIController.Instance.MostrarRecompensa(p);
			UserData.Instance.CheckMedalla(p);
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

	public bool PrimerPuntoVisitado(){
		int aux = 0;
		foreach(PuntoRuta p in puntos)
			if(p.puntoVisitado) aux++;
		return aux == 1;
	}

	public bool TodosPuntosVisitados(){
		int aux = 0;
		foreach(PuntoRuta p in puntos)
			if(p.puntoVisitado) aux++;
		return aux == puntos.Count;
	}
}
