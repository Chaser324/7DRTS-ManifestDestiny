using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapPointManager : MonoBehaviour {
	
	public Transform winDialog;
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.cityManager = this;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void InitCities() {
		List<string> civs = 
			new List<string>(new string[]{"Dutch", "French", "English", "Spanish", "Swedish"});
		List<string> starters = 
			new List<string>(new string[]{"The Verrazzano Path", "Northwest Passage", 
				"Chris Columbus Carribean Cruise", "StartCity4", "StartCity5"});
		
		if (GameManager.Instance.StartingRoute == "") {
			GameManager.Instance.StartingRoute = "Chris Columbus Carribean Cruise";
			GameManager.Instance.PlayerCiv = "Dutch";
		}
		
		civs.Remove(GameManager.Instance.PlayerCiv);
		starters.Remove(GameManager.Instance.StartingRoute);
		
		MapPoint startingCity = transform.Find(GameManager.Instance.StartingRoute).GetComponent<MapPoint>();
		startingCity.Discover();
		Settle(startingCity, GameManager.Instance.PlayerCiv, 50, 50, 50);
		
		Camera.mainCamera.transform.parent.position = 
			new Vector3(startingCity.transform.position.x, 
				Camera.mainCamera.transform.parent.position.y, 
				startingCity.transform.position.z - 40);
		
		foreach (string civ in civs) {
			string cityName = starters[Random.Range(0,starters.Count-1)];
			starters.Remove(cityName);
			startingCity = transform.Find(cityName).GetComponent<MapPoint>();
			Settle(startingCity, civ, 50, 50, 50);
			
			EnemyController enemy = gameObject.AddComponent<EnemyController>();
			enemy.civName = civ;
			enemy.AddCity(startingCity);
		}
	}
	
	public void AddAICity(MapPoint city, string civ) {
		EnemyController[] enemyList = gameObject.GetComponents<EnemyController>();
		
		foreach (EnemyController enemy in enemyList) {
			if (enemy.civName == civ) {
				enemy.AddCity(city);	
			}
		}
	}
	
	private void Settle(MapPoint city, string owner, int army, int civs, int food) {
		city.owner = "";
		city.occupier = owner;
		city.food = food;
		city.soldiers = army;
		city.civilians = civs;
		city.startingCity = true;
	}
}
