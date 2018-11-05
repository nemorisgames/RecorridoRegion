using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagenGaleria : MonoBehaviour {
	public Image imagen;
	public bool clickable = false;

	public void OnClick(){
		if(!clickable)
			return;
		GUIController.Instance.MostrarImagenHitoZoom(imagen.sprite);
	}
}
