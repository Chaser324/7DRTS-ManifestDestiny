using UnityEngine;
using System.Collections;

public class MapPath : MonoBehaviour {
	
	public string pathName;
	public MapPoint endPointA;
	public MapPoint endPointB;
	
	public int deaths = 0;
	public float length;
	
	public float difficulty = 5f;
	public float maxWidth = 3.5f;
	public float lengthScale = 2f;
	public float deathChanceCiv = 10f;
	public float deathChanceSoldier = 5f;
	public float deathReduction = 0.005f;
	public float deathRollInterval = 20f;
	public float difficultyDrift = 0.0010f;
	private int successfulCrosses = 0;
	
	private float redScale = 75f;
	private Color originalColor;
	private Color highlightedColor;

	void Start () {
		Spline spline = gameObject.GetComponent<Spline>();
		length = spline.Length * 10;
		
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		originalColor = r.material.GetColor("_Color");
		highlightedColor = 
			new Color(originalColor.r + 0.2f, originalColor.g + 0.2f, originalColor.b + 0.2f, originalColor.a);
	}
	
	void Update () {		
		if (GameManager.Instance.Paused) { return; }
		
		difficulty += Time.deltaTime * difficultyDrift;
	}
	
	void OnCollisionEnter(Collision info) {
		MapPoint city = info.transform.GetComponent<MapPoint>();
		if (endPointA == null) {
			endPointA = city;
			endPointA.AddConnectedPath(this);
		}
		else {
			endPointB = city;
			endPointB.AddConnectedPath(this);
			
			endPointA.AddConnectedCity(endPointB);
			endPointB.AddConnectedCity(endPointA);
			
			transform.parent.GetComponent<MapPathManager>().CompletedPath();
		}
	}
	
	public void Death() {
		++deaths;
		
		GameManager.Instance.playerDeaths += 1;
		
		float reduction = deathReduction / (length * 0.001f) + Random.Range(-0.001f, 0.001f);
		difficulty = Mathf.Clamp(difficulty - reduction, 0f, 5f);
		
		SplineMesh splineMesh = gameObject.GetComponent<SplineMesh>();
		splineMesh.xyScale = new Vector2(3.5f - ((difficulty / 5f) * 2.5f), 0.3f);
		splineMesh.UpdateMesh();
		
		redScale = (170f - 95f * (difficulty / 5f)) / 255;
		
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		Color tempColor = r.material.GetColor("_Color");
		r.material.SetColor("_Color", new Color(redScale, tempColor.g, tempColor.b));
	}
	
	public void Deselect() {
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		r.material.SetColor("_Color", new Color(redScale, originalColor.g, originalColor.b));
	}
	
	void OnMouseDown() {
		if (GameManager.Instance.SelectedPath != this) {
			GameManager.Instance.SelectedPath = this;
			GameManager.Instance.SelectedCity = null;
			
			Renderer r = GetComponent(typeof(Renderer)) as Renderer;
			r.material.SetColor("_Color", new Color(redScale, highlightedColor.g, highlightedColor.b));
		}
	}
}
