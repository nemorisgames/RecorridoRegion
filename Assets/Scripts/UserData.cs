using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserData : MonoBehaviour {
	public static UserData _instance;
	public static UserData Instance { get { return _instance;}}
	public List<Ruta> rutas;
	private List<PuntoRuta> puntos;
	[Header("Parametros")]
	public int visitadosIX = 0;
	public int visitadosX, visitadosXIV = 0;
	public int totalIX, totalX, totalXIV = 0;
	[Header("Medallas")]
	public List<Medalla> medallas;
	[System.Serializable]
	public struct Medalla{
		public string nombre;
		public string descripcion;
		public bool desbloqueada;
	}

	void Awake(){
		if(_instance == null)
			_instance = this;
	}
	

	// Use this for initialization
	void Start () {
		puntos = new List<PuntoRuta>();
		foreach(Ruta r in rutas){
			foreach(PuntoRuta p in r.puntos)
				if(!puntos.Contains(p))
					puntos.Add(p);
		}

		if(puntos.Count <= 0)
			return;
		foreach(PuntoRuta p in puntos){
			switch(p.region){
				case PuntoRuta.Region.IX:
				totalIX++;
				if(p.puntoVisitado)
					visitadosIX++;
				break;

				case PuntoRuta.Region.X:
				totalX++;
				if(p.puntoVisitado)
					visitadosX++;
				break;

				case PuntoRuta.Region.XIV:
				totalXIV++;
				if(p.puntoVisitado)
					visitadosXIV++;
				break;
			}
		}

		GUIController.Instance.ActualizarPuntaje(rutas.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckMedalla(PuntoRuta p){
		//revisar region del punto
		switch(p.region){
			case PuntoRuta.Region.IX:
			if(visitadosIX == 0){
				//asignar medalla por visitar region
			}
			visitadosIX++;
			if(visitadosIX == totalIX){
				//asignar medalla total region
				DesbloquearMedalla(5);
				Debug.Log("visitados todos region");
			}
			break;
			case PuntoRuta.Region.X:
			if(visitadosX == 0){
				//asignar medalla por visitar region
			}
			visitadosX++;
			if(visitadosX == totalX){
				//asignar medalla total region
			}
			break;
			case PuntoRuta.Region.XIV:
			if(visitadosXIV == 0){
				//asignar medalla por visitar region
			}
			visitadosXIV++;
			if(visitadosXIV == totalXIV){
				//asignar medalla total region
			}
			break;
		}

		int indiceRuta = rutas.FindIndex(x => x == p.ruta);
		if(indiceRuta < 0)
			return;

		
		//revisar si es 1er punto de ruta
		if(rutas[indiceRuta].PrimerPuntoVisitado()){
			//entregar medalla
			DesbloquearMedalla(0);
			Debug.Log("primero ruta");
		}
		

		//revisar total puntos de ruta
		else if(rutas[indiceRuta].TodosPuntosVisitados()){
			//entregar medalla
			Debug.Log("todos ruta");
			//GUIController.Instance.MostrarMedalla(medallas[2]);
		}

		//revisar exp ruta


		//revisar exp total


	}

	void DesbloquearMedalla(int i){
		Medalla aux = medallas[i];
		aux.desbloqueada = true;
		GUIController.Instance.MostrarMedalla(aux);
		medallas[i] = aux;
	}
}
