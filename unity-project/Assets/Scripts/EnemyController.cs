using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
	
	public string civName;
	
	private float elapsedTime;
	private int phase = 0;
	
	private Stack<MapPoint> occupiedCities = new Stack<MapPoint>();
	private MapPoint selectedCity;
	private MapPoint targetCity;
	private MapPath connectingPath;
	
	private int civLimit = 0;
	private int soldierLimit = 0;
	private float diffLimt = 0f;
	
	void Start () {
		elapsedTime = 0f;
	}
	
	void Update () {
		if (GameManager.Instance.Paused) {
			return;	
		}
		
		while (occupiedCities.Count > 0 && occupiedCities.Peek().occupier != civName) {
			occupiedCities.Pop();
			elapsedTime = 0f;
		}
		
		if (occupiedCities.Count == 0) {
			return;
		}
		
		if (occupiedCities.Peek() != selectedCity) {
			elapsedTime = 0;
			phase = 0;
			selectedCity = occupiedCities.Peek();
			MapPoint[] connectedCities = selectedCity.ConnectedCities;	
			targetCity = connectedCities[Random.Range(0, connectedCities.Length-1)];
			
			civLimit = 300 + Random.Range(0,100);
			soldierLimit = 200 + Random.Range(0,100);
			diffLimt = 1f + Random.Range(-0.5f, 1f);
			
			foreach (MapPath path in selectedCity.ConnectedPaths) {
				if (path.endPointA == targetCity || path.endPointB == targetCity) {
					connectingPath = path;
				}
			}
		}
		
		elapsedTime += Time.deltaTime;
		
		if (targetCity.occupier == "") {
			// Train Settlers Until Limit
			if (phase == 0) {
				if (selectedCity.civilians >= civLimit) {
					++phase;	
				}
				if (elapsedTime > selectedCity.civDelay && selectedCity.food > 0) {
					elapsedTime -= selectedCity.civDelay;
					selectedCity.ConvertFoodToCiv();
				}
			}
			// Send Settlers Until Limit
			else if (phase == 1) {
				elapsedTime = 0;
				if (selectedCity.civilians > 20 + Random.Range(0, 30)) {
					selectedCity.SendCivsToTarget(targetCity);	
				}
				else {
					++phase;	
				}
			}
			else {
				phase = 0;	
			}
		}
		else if (targetCity.occupier != civName) {
			// Train Settlers Until Limit
			if (phase == 0) {
				if (selectedCity.civilians >= 50) {
					++phase;	
				}
				if (elapsedTime > selectedCity.civDelay && selectedCity.food > 0) {
					elapsedTime -= selectedCity.civDelay;
					selectedCity.ConvertFoodToCiv();
				}
			}
			// Train Soldiers Until Limit
			else if (phase == 1) {
				if (selectedCity.soldiers >= Random.Range(30,50)) {
					++phase;	
				}
				if (elapsedTime > selectedCity.armyDelay && selectedCity.food > 0 && selectedCity.civilians > 50) {
					elapsedTime -= selectedCity.armyDelay;
					selectedCity.ConvertCivToSoldier();
				}
				else if (elapsedTime > selectedCity.armyDelay && selectedCity.food > 0 && selectedCity.food > 1) {
					elapsedTime -= selectedCity.armyDelay;
					selectedCity.ConvertFoodToSoldier();
				}
			}
			// Train Settlers Until Limit or Path Difficulty Limit
			else if (phase == 2) {
				if (selectedCity.civilians >= civLimit || connectingPath.difficulty < diffLimt) {
					++phase;	
				}
				if (elapsedTime > selectedCity.civDelay && selectedCity.food > 0) {
					elapsedTime -= selectedCity.civDelay;
					selectedCity.ConvertFoodToCiv();
				}
			}
			// Send Settlers Until Path Difficulty Limit
			else if (phase == 3) {
				elapsedTime = 0;
				if (selectedCity.civilians > 20 && connectingPath.difficulty >= diffLimt) {
					selectedCity.SendCivsToTarget(targetCity);	
				}
				else if (connectingPath.difficulty < diffLimt) {
					++phase;
				}
				else {
					if (Random.Range(0f,1f) > 0.5f) {
						phase = 0;	
					}
					else {
						++phase;	
					}
				}
			}
			// Train Soldiers Until Limit
			else if (phase == 4) {
				if (selectedCity.soldiers >= soldierLimit) {
					++phase;	
				}
				if (elapsedTime > selectedCity.armyDelay && selectedCity.food > 0 && selectedCity.civilians > 50) {
					elapsedTime -= selectedCity.armyDelay;
					selectedCity.ConvertCivToSoldier();
				}
			}
			// Send Soldiers Until Limit
			else if (phase == 5) {
				elapsedTime = 0;
				if (selectedCity.soldiers > 20 + Random.Range(0, 30)) {
					if (selectedCity.food > 0) {
						selectedCity.SendSoldiersToTarget(targetCity);
					}
				}
				else {
					++phase;	
				}
			}
			else {
				phase = 0;	
			}
		}
		else {
			// Shuffle Occupied Cities List
			MapPoint[] cityArray = occupiedCities.ToArray();
			for (int i=1; i<occupiedCities.Count; i++) {
				int j = Random.Range(0,i);
				
				MapPoint tempCity = cityArray[i];
				cityArray[i] = cityArray[j];
				cityArray[j] = tempCity;
			}
			occupiedCities = new Stack<MapPoint>(cityArray);
		}
	}
	
	public void AddCity(MapPoint city) {
		occupiedCities.Push(city);
	}
}
