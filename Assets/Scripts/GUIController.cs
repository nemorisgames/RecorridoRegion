using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	public UIScrollView scrollView;
	public CanvasGroup overlay, hito, recompensa, medalla;
	public static GUIController Instance { get { return _instance;}}
	private static GUIController _instance;
	public float velocidadTween = 0.2f;
	public float tiempoNotificacion = 3f;
	[Header("Top")]
	public Button btnGaleriaMedallas;
	public Button btnLockUsuario;
	public Button btnMute;
	[Header("UIMapa")]
	public CanvasGroup UIMapa;
	[Header("Puntaje")]
	public Text puntajeTotal;
	[Header("Hito")]
	public Text nombreHito;
	public Text descripcionHito;
	public GameObject prefabImagen;
	public GridLayoutGroup gridImagenesHito;
	[Header("Recompensa")]
	public Text descripcionRecompensa;
	public Text puntosRecompensa;
	[Header("Medalla")]
	public Text nombreMedalla;
	public Text descripcionMedalla;
	private IEnumerator autoOcultarRecompensa;
	private IEnumerator autoOcultarMedalla;
	public GameObject raycastBlocker;
	[Header("Galerias")]
	public CanvasGroup galerias;
	[Header("GaleriaMedalla")]
	public CanvasGroup galeriaMedallas;
	public GameObject prefabMedalla;
	public GridLayoutGroup grid;
	[Header("ImagenHitoZoom")]
	public CanvasGroup imagenHitoZoom;
	public Image imagenZoom;
	public bool mostrandoMedalla;
	private AudioSource audioSource;

	void Awake(){
		audioSource = GetComponent<AudioSource>();
		if(_instance == null)
			_instance = this;
	}
	void Start () {
		HideAll();
	}

	public void MostrarHito(PuntoRuta p){
		gridImagenesHito.transform.DestroyChildren();
		nombreHito.text = p.nombre;
		/*if(p.puntoVisitado){
			descripcionHito.text = p.descripcion;
			for(int i = 0; i < p.imagenes.Count; i++){
				GameObject go = (GameObject)Instantiate(prefabImagen,gridImagenesHito.transform.position,gridImagenesHito.transform.rotation,gridImagenesHito.transform);
				go.GetComponent<ImagenGaleria>().imagen.sprite = p.imagenes[i];
				go.GetComponent<ImagenGaleria>().clickable = true;
			}
		}
		else{
			descripcionHito.text = "Visita este hito para obtener más información.";
			for(int i = 0; i < p.imagenes.Count; i++){
				GameObject go = (GameObject)Instantiate(prefabImagen,gridImagenesHito.transform.position,gridImagenesHito.transform.rotation,gridImagenesHito.transform);
				go.GetComponent<ImagenGaleria>().imagen.color = Color.gray;
			}
		}*/

		descripcionHito.text = p.puntoVisitado ? p.descripcion : "Visita este hito para obtener más información.";
		for(int i = 0; i < p.imagenes.Count; i++){
			GameObject go = (GameObject)Instantiate(prefabImagen,gridImagenesHito.transform.position,gridImagenesHito.transform.rotation,gridImagenesHito.transform);
			ImagenGaleria ig = go.GetComponent<ImagenGaleria>();
			if(p.puntoVisitado){
				ig.imagen.sprite = p.imagenes[i];
				ig.clickable = true;
			}
			else{
				ig.imagen.color = Color.gray;
			}
		}
			
		MostrarOverlay();
		PlayForward(hito);
	}

	public void OcultarHito(){
		OcultarOverlay();
		PlayBackwards(hito);
	}

	public void MostrarImagenHitoZoom(Sprite s){
		imagenHitoZoom.blocksRaycasts = true;
		imagenZoom.sprite = s;
		PlayForward(imagenHitoZoom);
	}

	public void OcultarImagenHitoZoom(){
		imagenHitoZoom.blocksRaycasts = false;
		imagenZoom.sprite = null;
		PlayBackwards(imagenHitoZoom);
	}
	
	public void MostrarRecompensa(PuntoRuta p){
		descripcionRecompensa.text = "Has obtenido una recompensa por visitar "+p.nombre+" por primera vez!";
		puntosRecompensa.text = p.expPunto.ToString();
		if(recompensa.alpha > 0)
			recompensa.alpha = 0;
		PlayForward(recompensa);
		autoOcultarRecompensa = AutoOcultarPanel(recompensa,tiempoNotificacion);
		StartCoroutine(autoOcultarRecompensa);
		ActualizarPuntaje(UserData.Instance.rutas.Count);
	}

	public void OcultarRecompensa(){
		StopCoroutine(autoOcultarRecompensa);
		PlayBackwards(recompensa);
	}

	public void MostrarOverlay(){
		PlayForward(overlay);
		overlay.blocksRaycasts = true;
		scrollView.enabled = false;
		raycastBlocker.SetActive(true);
	}

	public void OcultarOverlay(){
		PlayBackwards(overlay);
		overlay.blocksRaycasts = false;
		scrollView.enabled = true;
		raycastBlocker.SetActive(false);
	}

	public IEnumerator MostrarMedalla(UserData.Medalla m){
		while(mostrandoMedalla){
			yield return new WaitForEndOfFrame();
		}
		mostrandoMedalla = true;
		descripcionMedalla.text = m.descripcion;
		nombreMedalla.text = m.nombre;
		PlayForward(medalla);
		autoOcultarMedalla = AutoOcultarPanel(medalla,tiempoNotificacion);
		StartCoroutine(autoOcultarMedalla);

	}

	public void OcultarMedalla(){
		PlayBackwards(medalla);
		mostrandoMedalla = false;
	}

	public void MostrarGaleriaMedalla(List<UserData.Medalla> medallas){
		btnGaleriaMedallas.GetComponent<Image>().color = btnGaleriaMedallas.colors.pressedColor;
		raycastBlocker.SetActive(true);
		grid.transform.DestroyChildren();
		foreach(UserData.Medalla m in medallas){
			GameObject go = (GameObject)Instantiate(prefabMedalla,grid.transform.position,grid.transform.rotation,grid.transform);
			go.GetComponent<Medalla>().Init(m.nombre,m.descripcion,m.desbloqueada);
		}
		PlayBackwards(UIMapa);
		PlayForward(galeriaMedallas);
	}

	public void OcultarGaleriaMedalla(){
		btnGaleriaMedallas.GetComponent<Image>().color = btnGaleriaMedallas.colors.normalColor;
		raycastBlocker.SetActive(false);
		PlayBackwards(galeriaMedallas);
		PlayForward(UIMapa);
	}

	public void BotonGaleriaMedallas(){
		if(galeriaMedallas.alpha == 0)
			MostrarGaleriaMedalla(UserData.Instance.medallas);
		else
			OcultarGaleriaMedalla();
	}

	public void ActualizarPuntaje(int rutas){
		int total = 0;
		for(int i = 0; i < rutas; i++)
			total += PlayerPrefs.GetInt("expRuta"+i,0);
		puntajeTotal.text = total.ToString();
	}

	void PlayForward(CanvasGroup c){
		StartCoroutine(PlayTween(c,1));
	}

	void PlayBackwards(CanvasGroup c){
		StartCoroutine(PlayTween(c,-1));
	}

	IEnumerator PlayTween(CanvasGroup c, int dir){
		float aux = c.alpha;
		if(dir >= 0 && c.alpha == 0){
			while(aux < 1){
				aux += velocidadTween;
				c.alpha = aux;
				yield return new WaitForSeconds(Time.deltaTime);
			}
			c.alpha = 1;
		}
		if(dir < 0 && c.alpha == 1){
			while(aux > 0){
				aux -= velocidadTween;
				c.alpha = aux;
				yield return new WaitForSeconds(Time.deltaTime);
			}
			c.alpha = 0;
			if(c == medalla)
				mostrandoMedalla = false;
		}
		if(c == overlay && dir < 0)
			HideAll();
	}

	void HideAll(){
		overlay.alpha = 0;
		overlay.blocksRaycasts = false;
		hito.alpha = 0;
		recompensa.alpha = 0;
		medalla.alpha = 0;
		imagenHitoZoom.alpha = 0;
		imagenHitoZoom.blocksRaycasts = false;
		raycastBlocker.SetActive(false);
		UIMapa.alpha = 1;
	}

	IEnumerator AutoOcultarPanel(CanvasGroup c, float f){
		yield return new WaitForSeconds(f);
		if(c.alpha == 1){
			PlayBackwards(c);
		}
	}

	public void UIBotonLock(bool b){
		btnLockUsuario.GetComponent<Image>().color = (b ? btnLockUsuario.colors.pressedColor : btnLockUsuario.colors.normalColor);
	}

	public void UIBotonMute(){
		audioSource.volume = audioSource.volume == 1 ? 0 : 1;
		btnMute.GetComponent<Image>().color = audioSource.volume == 1 ? btnMute.colors.normalColor : btnMute.colors.pressedColor;
	}
	
}
