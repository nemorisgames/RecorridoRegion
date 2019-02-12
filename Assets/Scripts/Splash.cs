using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour {
	AsyncOperation loadScene;
	public bool autoLoad = false;
	public float loadTime = 5f;
	public CanvasGroup fadeIn;

	// Use this for initialization
	void Start () {
		StartCoroutine(cargarMapa());
	}
	
	public void Mapa(){
		loadScene.allowSceneActivation = true;
	}

	IEnumerator cargarMapa(){
		StartCoroutine(fade(true));
		loadScene = SceneManager.LoadSceneAsync("Ruta");
		loadScene.allowSceneActivation = false;
		if(autoLoad)
			StartCoroutine(AutoLoad(loadTime));
		yield return loadScene;
	}

	IEnumerator AutoLoad(float f){
		yield return new WaitForSeconds(f);
		if(loadScene.progress >= 0.9f){
			StartCoroutine(fade(false));
			yield return new WaitForSeconds(0.5f);
			Mapa();
		}
		else
			StartCoroutine(AutoLoad(1f));
	}

	IEnumerator fade(bool b){
		if(b){
			fadeIn.alpha = 1;
			while(fadeIn.alpha > 0){
				fadeIn.alpha -= 0.05f;
				yield return new WaitForSeconds(0.01f);
			}
			fadeIn.alpha = 0;		
		}
		else{
			fadeIn.alpha = 0;
			while(fadeIn.alpha < 1){
				fadeIn.alpha += 0.05f;
				yield return new WaitForSeconds(0.01f);
			}
			fadeIn.alpha = 1;	
		}
	}
}
