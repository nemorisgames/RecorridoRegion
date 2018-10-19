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
		public float latitud, longitud;
		public Transform point;
	}
	public Vector2 originAdjust;
	public bool debug = false;
	public Text coordText;

	void Awake(){
		if(_instance == null)
			_instance = this;
		DontDestroyOnLoad(this);
	}

	void Start () {
		StartCoroutine(StartGPS());
	}
	
	IEnumerator StartGPS(){
		if (!Input.location.isEnabledByUser)
            yield break;
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
		}
	}

	void StopGPS(){
		if(Input.location.status == LocationServiceStatus.Running){
			Input.location.Stop();
			activated = false;
		}
	}

	void Update(){
		foreach(MapPoint p in points)
			LocatePoint(p);
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
	}

	void DebugGPS(){
		if(activated)
			coordText.text = userLocation.latitud+", "+userLocation.longitud;
	}
}
