using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseColorHandler : MonoBehaviour {
		
	private static List<string> civs = 
		new List<string>(new string[] {"", "french", "spanish", "dutch", "english", "swedish"});
	
	private static Color[] civColors = 
		new Color[] {new Color(46f/255f, 69f/255f, 14f/255f),
					 new Color(0f, 0f, 153f/255f), 
		             new Color(1f, 1f, 21f/255f), 
		             new Color(240f/255f, 107f/255f, 0f), 
		             new Color(0.8f, 0.8f, 0.8f), 
		             new Color(0f, 82f/255f, 147f/255f)};
	
	void Start() {
			
	}
	
	public void SetColor(string civName) {
		Renderer r = GetComponent(typeof(Renderer)) as Renderer;
		r.material.SetColor("_Color", civColors[civs.IndexOf(civName.ToLower())]);
		
		transform.GetComponent<MapPointClickHandler>().originalColor = civColors[civs.IndexOf(civName.ToLower())];
	}
}
