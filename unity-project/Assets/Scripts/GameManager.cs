using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public MapPathManager pathManager;
	public MapPointManager cityManager;
	
	public int playerDeaths = 0;
	
	private string playerCiv = "";
	private string startingRoute = "";
	
	private bool paused = true;
	
	private MapPath selectedPath;
	private MapPoint selectedCity;
	private MapPoint targetCity;
	
	private static GameManager instance;
	
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);	
	}
	
	void Start () {
	}
	
	void Update () {
	}
	
	void OnApplicationQuit() {
		instance = null;	
	}
	
	public void StartGame() {
		cityManager.InitCities();
		paused = false;
	}
	
	public void PlayerWins() {
		paused = true;
		cityManager.winDialog.gameObject.SetActive(true);
		
		UILabel message = cityManager.winDialog.Find("WinMessage").GetComponent<UILabel>();
		message.text = "Through the blood of " + playerDeaths + " of your fallen countrymen, you've achieved destiny.";
	}
	
	public void PlayerLoses(string winningCiv) {
		paused = true;
		cityManager.winDialog.gameObject.SetActive(true);
		
		UILabel message = cityManager.winDialog.Find("WinMessage").GetComponent<UILabel>();
		message.text = "You've disgraced your country and " + playerDeaths + " of your countrymen have died for nothing.";
	}
	
	#region Public Properties
	
	public static GameManager Instance {
		get {
			if (!instance) {
				instance = new GameObject("GameManager").AddComponent<GameManager>();
			}
			
			return instance;
		}
	}
	
	public bool Paused {
		get { return paused; }
		set { paused = value; }
	}
	
	public string PlayerCiv {
		get { return playerCiv; }
		set { playerCiv = value; }
	}
	
	public string StartingRoute {
		get { return startingRoute; }
		set { startingRoute = value; }
	}
	
	public MapPath SelectedPath {
		get { return selectedPath; }
		set { 
			if (selectedPath != value) {
				if (selectedPath) selectedPath.Deselect();
				selectedPath = value;
			}
		}
	}
	
	public MapPoint SelectedCity {
		get { return selectedCity; }
		set { 
			if (selectedCity != value) {
				if (selectedCity) selectedCity.Deselect();
				selectedCity = value;
			}
			
		}
	}
	
	public MapPoint TargetCity {
		get { return targetCity; }
		set { targetCity = value; }
	}
	
	#endregion
}
