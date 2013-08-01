using UnityEngine;
using System.Collections;

public class FlagHandler : MonoBehaviour {
	
	public Material[] materials;
	
	public void SetMaterial(string matName) {
		Transform plane1 = transform.Find("Plane1");
		Transform plane2 = transform.Find("Plane2");
		
		foreach (Material mat in materials) {
			if (mat.name == matName.ToLower()) {
				plane1.gameObject.renderer.material = mat;
				plane2.gameObject.renderer.material = mat;
				break;
			}
		}
	}
	
	public void SetHeight(float height) {
		Transform plane1 = transform.Find("Plane1");
		plane1.transform.localPosition = 
			new Vector3(plane1.transform.localPosition.x, 
				        -8.03f + (5f * height), 
				        plane1.transform.localPosition.z);
	}
}
