using UnityEngine;
using System.Collections;

public class FlagClickHandler : MonoBehaviour {
	
	public string civName;
	public Transform civPanel;
	
	void OnMouseDown() {
		GameManager.Instance.PlayerCiv = civName;
		
		foreach (Transform panel in civPanel.parent.transform) {
			if (panel.name == "CivInfoPanel" || panel.name == "SelectCiv") {
				panel.gameObject.SetActive(false);	
			}
		}
		
		civPanel.gameObject.SetActive(true);
	}
}
