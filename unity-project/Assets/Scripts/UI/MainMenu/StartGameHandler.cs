using UnityEngine;
using System.Collections;

public class StartGameHandler : MonoBehaviour {
	public FadeHandler fader;
	
	void OnClick() {
		fader.NextScene = "civselect";
		fader.FadeOut();
	}
}
