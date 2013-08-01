using UnityEngine;
using System.Collections;

public class FadeHandler : MonoBehaviour {
	public string NextScene;
	
	public void DisablePanel() {
		Transform panel = transform.Find("Overlay");
		panel.gameObject.SetActive(false);
	}
	
	public void FadeOut() {
		Camera mainCamera = Camera.mainCamera;
		AudioSource musicPlayer = mainCamera.GetComponent(typeof(AudioSource)) as AudioSource;
		TweenVolume volTween = mainCamera.gameObject.AddComponent<TweenVolume>() as TweenVolume;
		volTween.from = musicPlayer.volume;
		volTween.to = 0f;
		volTween.duration = 2.5f;
		
		Transform panel = transform.Find("Overlay");
		panel.gameObject.SetActive(true);
		
		TweenAlpha tween = panel.GetComponent<TweenAlpha>() as TweenAlpha;
		tween.from = 0f;
		tween.to = 1f;
		tween.callWhenFinished = "TransitionScene";
		
		tween.Reset();
		tween.Play(true);
		volTween.Play(true);
	}
	
	public void TransitionScene() {
		Application.LoadLevel(NextScene);	
	}
}
