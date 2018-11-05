using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour {
	public static GPS Instance {get {return _instance;}}
	private static GPS _instance;
	public LocationInfo locationInfo;
	public bool activated = false;
	public UI2DSprite map;
	public MapPoint userLocation;
	[SerializeField]
	public List<MapPoint> points;
	[System.Serializable]
	public struct MapPoint{
		public string name;
		public string descripcion;
		public float latitud, longitud;
		public Transform point;
		public Sprite imagen;
	}
	public Vector2 originAdjust;
	public bool debug = false;
	public Text coordText;

	void Awake(){
		if(_instance == null)
			_instance = this;
		if(debug)
			PlayerPrefs.DeleteAll();
	}

	void Start () {
		StartCoroutine(StartGPS());
		foreach(MapPoint p in points){
			InitPoint(p);
			LocatePoint(p);
		}
	}

	void InitPoint(MapPoint p){
		PuntoRuta pr = p.point.GetComponent<PuntoRuta>();
		if(pr == null)
			return;
		pr.nombre = p.name;
		pr.descripcion = p.descripcion;
		pr.imagenes = new List<Sprite>();
		pr.imagenes.Add(p.imagen);
	}
	
	IEnumerator StartGPS(){
		if (!Input.location.isEnabledByUser){
			#if UNITY_EDITOR
			AutoScroll.Instance.StartOnUser();
			#endif
			yield break;
		}
		Input.location.Start();
		int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
		if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }
        else
        {
			activated = true;
			locationInfo = Input.location.lastData;
			Debug.Log(locationInfo.latitude+","+locationInfo.longitude);
			LocateUser();
			StartCoroutine(AutoScroll.Instance.DelayStartOnUser());
		}
	}

	void StopGPS(){
		if(Input.location.status == LocationServiceStatus.Running){
			Input.location.Stop();
			activated = false;
		}
	}

	void Update(){
		if(Input.location.status == LocationServiceStatus.Running)
			locationInfo = Input.location.lastData;
		if(activated)
			LocateUser();
		if(debug)
			DebugGPS();
	}

	Vector3 CoordToXY(float latitud, float longitud){
		Vector2 coord = new Vector2();

		float longStart, latStart, mapLong, mapLat;
		longStart = -74.30899f;
		latStart = -38.16292f;
		mapLong = -69.66879f - longStart;
		mapLat = latStart - (-41.73970f);

		coord.x = map.localSize.x * (longitud/mapLong);
		coord.y = map.localSize.y * (latitud/mapLat);

		coord += originAdjust;
		return coord;
	}

	void LocatePoint(MapPoint p){
		Vector2 finalPos = CoordToXY(p.latitud,p.longitud);
		p.point.localPosition = finalPos;
	}

	void LocateUser(){
		userLocation.latitud = locationInfo.latitude;
		userLocation.longitud = locationInfo.longitude;
		LocatePoint(userLocation);
		AutoScroll.Instance.GoToUser();
	}

	void DebugLocateUser(){
		LocatePoint(userLocation);
		AutoScroll.Instance.GoToUser();
	}

	void DebugGPS(){
		if(activated)
			coordText.text = "activated: "+activated + " | In bounds: "+ UserInBounds().ToString() + "\n" + userLocation.latitud +", "+userLocation.longitud;
		else{
			#if !UNITY_EDITOR
				return;
			#endif
			
			if(Input.GetAxis("Horizontal") != 0)
				userLocation.longitud += Input.GetAxis("Horizontal") * Time.deltaTime * 0.5f;
			if(Input.GetAxis("Vertical") != 0)
				userLocation.latitud += Input.GetAxis("Vertical") * Time.deltaTime * 0.5f;

			DebugLocateUser();
		}
	}

	public bool UserInBounds(){
		float longStart, latStart, longEnd, latEnd;
		longStart = -74.30899f;
		latStart = -38.16292f;
		longEnd = -69.66879f;
		latEnd = -41.73970f;
		bool inBounds = (userLocation.latitud < latStart && userLocation.latitud > latEnd && userLocation.longitud > longStart && userLocation.longitud < longEnd);
		userLocation.point.gameObject.SetActive(inBounds);
		return inBounds;
	}
}
