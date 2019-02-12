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

		//cargar estado medallas
		Medalla aux;
		for(int i = 0; i < medallas.Count; i++){
			aux = medallas[i];
			aux.desbloqueada = PlayerPrefs.GetInt("EstadoMedalla"+i,0) == 1 ? true : false;
			medallas[i] = aux;
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
				//visitado uno de cada region
				if(visitadosX != 0 && visitadosXIV != 0)
					DesbloquearMedalla(4);
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
				//visitado uno de cada region
				if(visitadosIX != 0 && visitadosXIV != 0)
					DesbloquearMedalla(4);
			}
			visitadosX++;
			if(visitadosX == totalX){
				DesbloquearMedalla(7);
			}
			break;
			case PuntoRuta.Region.XIV:
			if(visitadosXIV == 0){
				//asignar medalla por visitar region
				//visitado uno de cada region
				if(visitadosX != 0 && visitadosX != 0)
					DesbloquearMedalla(4);
			}
			visitadosXIV++;
			if(visitadosXIV == totalXIV){
				DesbloquearMedalla(6);
			}
			break;
		}

		int indiceRuta = rutas.FindIndex(x => x == p.ruta);
		if(indiceRuta < 0)
			return;

		
		//revisar si es 1er punto de ruta
		if(TotalVisitados() == 1){
			//entregar medalla
			DesbloquearMedalla(0);
			Debug.Log("primero ruta");
		}

		if(TotalVisitados() >= Total()/2)
			DesbloquearMedalla(1);

		if(TotalVisitados() >= Total())
			DesbloquearMedalla(2);
		
		if(PlayerPrefs.GetInt("EstadoMedalla"+3,0) == 0){
			bool desbloquear = true;
			foreach(Ruta r in rutas){
				if(r.PuntosVisitados() == 0){
					desbloquear = false;
					break;
				}
			}
			if(desbloquear)
				DesbloquearMedalla(3);
		}

		//revisar total puntos de ruta
		if(rutas[indiceRuta].TodosPuntosVisitados()){
			//entregar medalla
			Debug.Log("todos ruta");
			int aux = Mathf.Clamp(8+indiceRuta,8,medallas.Count);
			DesbloquearMedalla(aux);
		}


	}

	int TotalVisitados(){
		return visitadosIX + visitadosX + visitadosXIV;
	}

	int Total(){
		return totalIX + totalX + totalXIV;
	}

	void DesbloquearMedalla(int i){
		if(PlayerPrefs.GetInt("EstadoMedalla"+i,0) == 1)
			return;
		Medalla aux = medallas[i];
		aux.desbloqueada = true;
		StartCoroutine(GUIController.Instance.MostrarMedalla(aux));
		medallas[i] = aux;
		PlayerPrefs.SetInt("EstadoMedalla"+i,1);
	}
}
