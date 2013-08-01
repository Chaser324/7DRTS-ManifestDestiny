using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OceanPathManager : MonoBehaviour {
	
	public FadeHandler fader;
	
	public void CastOff() {
		Transform selectedPath;
		Transform ship = transform.Find("Ship");
		
		if (GameManager.Instance.StartingRoute == "") {
			GameManager.Instance.StartingRoute = "The Verrazzano Path";
		}
		
		foreach (Transform child in transform) {
			MeshRenderer renderComponent = child.GetComponent<MeshRenderer>();
			MeshCollider colliderComponent = child.GetComponent<MeshCollider>();
			
			if (renderComponent) { renderComponent.enabled = false; }
			if (colliderComponent) { colliderComponent.enabled = false; }
			
			OceanPathClickHandler handler = child.GetComponent<OceanPathClickHandler>();
			
			if (handler != null && handler.pathName == GameManager.Instance.StartingRoute) {
				Spline path = handler.gameObject.GetComponent<Spline>();
				ship.GetComponent<BoatBehavior>().Go(path);
			}
		}
		
		fader.NextScene = "game";
		fader.FadeOut();
	}
}
