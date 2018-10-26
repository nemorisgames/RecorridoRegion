using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path : MonoBehaviour {

	[HideInInspector]
	[SerializeField]
	List<Vector2> points;

	public Path(Vector2 centre){
		points = new List<Vector2>{
			centre + Vector2.left,
			centre + (Vector2.left + Vector2.up)*0.5f,
			centre + (Vector2.right + Vector2.down) * 0.5f,
			centre + Vector2.right
		};
	}

	public void AddSegment(Vector2 anchorPos){
		points.Add(points[points.Count - 1]*2 - points[points.Count - 2]);
		points.Add((points[points.Count - 1] + anchorPos) * 0.5f);
		points.Add(anchorPos);
	}

	/* public Vector2 [] GetPointsInSegment(){
		
	}*/
}
