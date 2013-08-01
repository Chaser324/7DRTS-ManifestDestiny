using UnityEngine;
using System.Collections;

public class OceanPathClickHandler : MonoBehaviour {
	
	public string pathName;
	public string pathInfo;
	
	public UILabel nameField;
	public UILabel infoField;
	
	private Color originalColor;
	
	void Start() {
	}
	
	public void Deselect() {
		if (GameManager.Instance.StartingRoute == pathName) {
			Renderer r = GetComponent(typeof(Renderer)) as Renderer;
			r.material.SetColor("_Color", originalColor);
		}
	}
	
	void OnMouseDown() {
		if (GameManager.Instance.StartingRoute != pathName) {
			OceanPathClickHandler[] handlers =
				transform.parent.gameObject.GetComponentsInChildren<OceanPathClickHandler>();
			
			foreach (OceanPathClickHandler obj in handlers) {
				obj.Deselect();
			}
			
			GameManager.Instance.StartingRoute = pathName;
			
			nameField.text = pathName;
			infoField.text = pathInfo;
			
			Renderer r = GetComponent(typeof(Renderer)) as Renderer;
			originalColor = r.material.GetColor("_Color");
			
			Color highlightedColor = 
				new Color(originalColor.r + 0.2f, originalColor.g + 0.2f, originalColor.b + 0.2f, originalColor.a);
			
			r.material.SetColor("_Color", highlightedColor);
		}	
	}
}
