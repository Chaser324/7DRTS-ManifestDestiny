using UnityEngine;
using System.Collections;

public class LandClickHandler : MonoBehaviour {
	void OnMouseDown() {
		GameManager.Instance.SelectedCity = null;
	}
	
	void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			GameManager.Instance.TargetCity = null;
		}
	}
}
