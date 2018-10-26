using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medalla : MonoBehaviour {
	public Text nombre, descripcion;
	public GameObject bloqueada;

	public void Init(string nombre, string descripcion, bool bloqueada){
		this.nombre.text = nombre;
		this.descripcion.text = descripcion;
		this.bloqueada.SetActive(!bloqueada);
	}
}
