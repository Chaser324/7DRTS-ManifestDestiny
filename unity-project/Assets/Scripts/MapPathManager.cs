using UnityEngine;
using System.Collections;

public class MapPathManager : MonoBehaviour {
	
	private int completedPaths = 0;
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.pathManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CompletedPath() {
		++completedPaths;
		
		if (completedPaths == transform.childCount) {
			// Map Connecting Complete. Cleanup and Start Game.
			foreach(Transform child in transform) {
				MapPath path = child.GetComponent<MapPath>();
				path.endPointA.ClearCollider();
				path.endPointB.ClearCollider();
			}
			
			GameManager.Instance.StartGame();
		}
	}
}
