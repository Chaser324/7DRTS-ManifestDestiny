using UnityEngine;
using System.Collections;

public class MapPointClickHandler : MonoBehaviour {
	
	public delegate void OnClickDelegate(GameObject clickedObject);
	
	private OnClickDelegate clickDelegate;
	private OnClickDelegate rightClickDelegate;
	
	public Color originalColor;
	
	void Awake() {
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		originalColor = r.material.GetColor("_Color");
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseOver() {
		if (Input.GetMouseButtonDown(1)) {
			rightClickDelegate(gameObject);
		}
	}
	
	void OnMouseDown() {
		if (clickDelegate != null) {
			clickDelegate(gameObject);	
		}
		
		Color highlightedColor = 
			new Color(originalColor.r + 0.2f, originalColor.g + 0.2f, originalColor.b + 0.2f, originalColor.a);
		
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		r.material.SetColor("_Color", highlightedColor);
	}
	
	public void Deselect() {
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		r.material.SetColor("_Color", originalColor);
	}
	
	public OnClickDelegate ClickDelegate {
		set { clickDelegate = value; }	
	}
	
	public OnClickDelegate RightClickDelegate {
		set { rightClickDelegate = value; }
	}
}
