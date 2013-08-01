using UnityEngine;
using System.Collections;

public class CivConfirmClickHandler : MonoBehaviour {
	
	public Transform flagContainer;
	
	void OnClick() {
		flagContainer.gameObject.SetActive(false);
		
		Camera mainCamera = Camera.mainCamera;
		Vector3 camStartPos = mainCamera.transform.position;
		Vector3 camStartRot = mainCamera.transform.eulerAngles;
		
		TweenPosition posTween = mainCamera.gameObject.AddComponent<TweenPosition>();
		TweenRotation rotTween = mainCamera.gameObject.AddComponent<TweenRotation>();
		
		posTween.from = camStartPos;
		rotTween.from = camStartRot;
		
		posTween.to = new Vector3(-186.8f, 509.7f, 85.3f);
		rotTween.to = new Vector3(47.3f, -8.5f, 0f);
		
		posTween.Play(true);
		rotTween.Play(true);
	}
}
