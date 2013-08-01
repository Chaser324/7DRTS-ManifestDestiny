using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionsPanelHandler : MonoBehaviour {
	
	private AudioSource musicPlayer;
	
	void OnEnable() {
		Camera mainCamera = Camera.mainCamera;
		
		musicPlayer = mainCamera.GetComponent(typeof(AudioSource)) as AudioSource;		
		UISlider musicVolSlider = transform.Find("Music/Slider").GetComponent("UISlider") as UISlider;
		UISlider soundVolSlider = transform.Find("Sound/Slider").GetComponent("UISlider") as UISlider;
		UIPopupList qualityList = transform.Find("Quality/Popup List").GetComponent("UIPopupList") as UIPopupList;
		UIPopupList resList = transform.Find("Resolution/Popup List").GetComponent("UIPopupList") as UIPopupList;
		
		qualityList.items = new List<string>();
		string[] qualityNames = QualitySettings.names;
		for (int i=0; i<qualityNames.Length; i++) {
			qualityList.items.Add(qualityNames[i]);
		}
		qualityList.selection = qualityList.items[QualitySettings.GetQualityLevel()];
		
		Resolution[] availableResolutions = Screen.resolutions;
		Resolution currentResolution = Screen.currentResolution;
		List<string> resNames = new List<string>(availableResolutions.Length);
		foreach (Resolution res in availableResolutions) {
			resNames.Add(res.width + "x" + res.height);
		}
		resList.items = resNames;
		resList.selection = currentResolution.width + "x" + currentResolution.height;
		
		if (musicPlayer)
		{
			musicVolSlider.sliderValue = musicPlayer.volume;
		}
		
		if (Application.loadedLevelName == "game") {
			GameManager.Instance.Paused = true;
		}
	}
	
	void OnDisable() {
		if (Application.loadedLevelName == "game") {
			GameManager.Instance.Paused = false;
		}
	}
	
	public void OnMusicVolumeChange(float val) {
		if (musicPlayer)
		{
			musicPlayer.volume = val;
		}
	}
	
	public void OnSoundVolumeChange(float val) {
		
	}
	
	public void OnResolutionValueChange(string val) {
		Resolution currentResolution = Screen.currentResolution;
		string[] dimensions = val.Split(new char[] {'x'});
		int newWidth = int.Parse(dimensions[0]);
		int newHeight = int.Parse(dimensions[1]);
		
		if (currentResolution.height != newHeight && currentResolution.width != newWidth) {
			Screen.SetResolution(newWidth, newHeight, true);
		}
	}
	
	public void OnQualityValueChange(string val) {
		List<string> qualityNames = new List<string>(QualitySettings.names);
		
		if (! val.Equals(qualityNames[QualitySettings.GetQualityLevel()]) ) {
			QualitySettings.SetQualityLevel(qualityNames.IndexOf(val));
		}
	}
}
