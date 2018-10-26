using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	public UIScrollView scrollView;
	public CanvasGroup overlay, hito, recompensa, medalla;
	public static GUIController Instance { get { return _instance;}}
	public static GUIController _instance;
	public float velocidadTween = 0.2f;
	[Header("UIMapa")]
	public CanvasGroup UIMapa;
	[Header("Puntaje")]
	public Text puntajeTotal;
	[Header("Hito")]
	public Text nombreHito;
	public Text descripcionHito;
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
	public GameObject templateMedalla;
	public GridLayoutGroup grid;

	void Awake(){
		if(_instance == null)
			_instance = this;
	}
	void Start () {
		HideAll();
	}

	public void MostrarHito(PuntoRuta p){
		nombreHito.text = p.nombre;
		if(p.puntoVisitado)
			descripcionHito.text = p.descripcion;
		else
			descripcionHito.text = "Visita este hito para obtener más información.";
		MostrarOverlay();
		PlayForward(hito);
	}

	public void OcultarHito(){
		OcultarOverlay();
		PlayBackwards(hito);
	}
	
	public void MostrarRecompensa(PuntoRuta p){
		descripcionRecompensa.text = "Has obtenido una recompensa por visitar "+p.nombre+" por primera vez!";
		puntosRecompensa.text = p.expPunto.ToString();
		PlayForward(recompensa);
		autoOcultarRecompensa = AutoOcultarPanel(recompensa,5f);
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

	public void MostrarMedalla(UserData.Medalla m){
		descripcionMedalla.text = m.descripcion;
		nombreMedalla.text = m.nombre;
		PlayForward(medalla);
		autoOcultarMedalla = AutoOcultarPanel(medalla,5f);
		StartCoroutine(autoOcultarMedalla);
	}

	public void OcultarMedalla(){
		PlayBackwards(medalla);
	}

	public void MostrarGaleriaMedalla(List<UserData.Medalla> medallas){
		raycastBlocker.SetActive(true);
		UIMapa.alpha = 0;
		grid.transform.DestroyChildren();
		foreach(UserData.Medalla m in medallas){
			GameObject go = (GameObject)Instantiate(templateMedalla,grid.transform.position,grid.transform.rotation,grid.transform);
			go.GetComponent<Medalla>().Init(m.nombre,m.descripcion,m.desbloqueada);
		}
		PlayForward(galeriaMedallas);
	}

	public void OcultarGaleriaMedalla(){
		UIMapa.alpha = 1;
		raycastBlocker.SetActive(false);
		PlayBackwards(galeriaMedallas);
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
		raycastBlocker.SetActive(false);
	}

	IEnumerator AutoOcultarPanel(CanvasGroup c, float f){
		yield return new WaitForSeconds(f);
		if(c.alpha == 1)
			PlayBackwards(c);
	}
}
