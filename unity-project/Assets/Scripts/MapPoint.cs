using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapPoint : MonoBehaviour {
	private const float STACK_SCALE = 0.03f;
	
	public string owner;
	public string occupier;
	public string cityName;
	public int civilians;
	public int soldiers;
	public int food;
	public bool discovered = false;
	public bool startingCity = false;
	public bool winCity = false;
	
	public float armyDelay = 0.6f;
	public float civDelay = 0.3f;
	public float foodGenRate = 0.01f;
	public float civGenRate = 0.005f;
	public float decayRate = .001f;
	public float civGenStartingCity = 0.002f;
	public float foodGenStartingCity = 0.002f;
	
	public float maxFood = 400;
	public float maxPop = 600;
	
	public float defense = 0f;
	
	public GameObject citizenPrefab;
	
	public AudioClip civOutClip;
	public AudioClip civInClip;
	public AudioClip armyOutClip;
	public AudioClip armyInClip;
	
	private AudioSource cityAudio;
	
	private string leftClickedPart = "";
	private string rightClickedPart = "";
	
	private Transform armyStack;
	private Transform civStack;
	private Transform foodStack;
	private BaseColorHandler baseColor;
	private FlagHandler flag;
	
	private float elapsedTime = 0f;
	private float foodGen = 0f;
	private float civGen = 0f;
	private float accumulatedDamage = 0f;
	private float flagpole = 0.0f;
	
	private List<MapPath> connectedPaths;
	private List<MapPoint> connectedCities;
	
	#region Unity Event Handlers
	
	void Awake() {
		cityAudio = gameObject.AddComponent<AudioSource>();		
		
		foreach (Transform childTransform in transform) {
			if (childTransform.name != "Flag") {
				MapPointClickHandler clickHandler = childTransform.gameObject.AddComponent<MapPointClickHandler>();
				clickHandler.ClickDelegate = OnClickedChild;
				clickHandler.RightClickDelegate = OnRightClickedChild;
			}
		}
		
		flag = transform.Find("Flag").GetComponent<FlagHandler>();
		baseColor = transform.Find("Building").GetComponent<BaseColorHandler>();
		armyStack = transform.Find("ArmyStack");
		civStack = transform.Find("CivStack");
		foodStack = transform.Find("FoodStack");
	}
	
	void Start() {
	}
	
	void Update() {
		if (GameManager.Instance.Paused) { return; }
		
		if (GameManager.Instance.PlayerCiv == occupier) {
			PlayerControlledBehavior();
		}
		
		if (winCity && owner != "" && flagpole == 1f) {
			if (owner == GameManager.Instance.PlayerCiv) {
				GameManager.Instance.PlayerWins();	
			}
			else {
				GameManager.Instance.PlayerLoses(owner);	
			}
		}
		
		if (civilians == 0 && soldiers == 0 && !startingCity && owner != "") {
			occupier = "";
			
			flagpole -= decayRate * Time.deltaTime;
			if (flagpole <= 0f) {
				owner = occupier;
				baseColor.SetColor(owner);
				flagpole = 0f;
				flag.gameObject.SetActive(false);
			}
		}
		
		if (occupier != "") {
			flag.gameObject.SetActive(true);
			
			if (occupier != owner) {
				flagpole -= decayRate * Time.deltaTime * civilians;
				
				if (flagpole <= 0f) {
					owner = occupier;
					baseColor.SetColor(owner);
					flagpole = 0f;
					flag.SetMaterial(owner);
				}
			}
			else if (flagpole < 1f) {
				flagpole += decayRate/10 * Time.deltaTime * civilians;
				flagpole = Mathf.Min(1f, flagpole);
			}
			
			foodGen += foodGenRate * Time.deltaTime * civilians;
			civGen += civGenRate * Time.deltaTime * civilians;
			
			if (startingCity) {
				foodGen += foodGenRate * Time.deltaTime;
				civGen += civGenRate * Time.deltaTime;
			}
			
			if (foodGen > 1f) {
				if (food < maxFood) { ++food; }
				--foodGen;
			}
			
			if (civGen > 1f) {
				if (civilians + soldiers < maxPop) ++civilians;
				--civGen;
			}
		}
		
		flag.SetHeight(flagpole);
		
		armyStack.localScale = new Vector3(1, 0.1f + (soldiers * STACK_SCALE), 1);
		armyStack.localPosition = 
			new Vector3(armyStack.localPosition.x, -0.5f + (soldiers * STACK_SCALE * 0.5f), armyStack.localPosition.z);
		
		civStack.localScale = new Vector3(1, 0.1f + (civilians * STACK_SCALE), 1);
		civStack.localPosition = 
			new Vector3(civStack.localPosition.x, -0.5f + (civilians * STACK_SCALE * 0.5f), civStack.localPosition.z);
		
		foodStack.localScale = new Vector3(1, 0.1f + (food * STACK_SCALE), 1);
		foodStack.localPosition = 
			new Vector3(foodStack.localPosition.x, -0.5f + (food * STACK_SCALE * 0.5f), foodStack.localPosition.z);
	}
	
	#endregion
	
	#region Public Methods
	
	public void Deselect() {
		foreach(Transform childTransform in transform) {
			MapPointClickHandler clickHandler = childTransform.gameObject.GetComponent<MapPointClickHandler>();
			if (clickHandler != null) { clickHandler.Deselect(); }
		}
	}
	
	public void SoldierArrived(SettlerBehavior settler) {
		if (settler.owner != occupier && (soldiers > 0 || civilians > 0)) {
			float damage = settler.attack - defense; 
			if (soldiers > 0) { damage -= Random.Range(8f, 18f); }
			damage = Mathf.Clamp(damage, 0.5f, 20.0f);
			accumulatedDamage += damage;
			
			if (soldiers > 0 && accumulatedDamage > 1) {
				soldiers -= Mathf.FloorToInt(accumulatedDamage);
				accumulatedDamage %= 1f;
				
				if (soldiers < 0) { soldiers = 0; }
			}
			else {
				civilians -= Mathf.FloorToInt(accumulatedDamage);
				accumulatedDamage %= 1f;
				
				if (civilians < 0) { civilians = 0; }
			}
		}
		else {
			occupier = settler.owner;
			++soldiers;
			
			if (settler.owner != GameManager.Instance.PlayerCiv) {
				GameManager.Instance.cityManager.AddAICity(this, settler.owner);	
			}
			else {
				cityAudio.PlayOneShot(armyInClip, 0.5f);
			}
			
			if (!discovered && settler.owner == GameManager.Instance.PlayerCiv) { Discover(); }
		}
	}
	
	public void CivilianArrived(SettlerBehavior settler) {
		if (settler.owner == occupier || (soldiers == 0 && civilians == 0)) {
			occupier = settler.owner;
			++civilians;
			
			if (settler.owner != GameManager.Instance.PlayerCiv) {
				GameManager.Instance.cityManager.AddAICity(this, settler.owner);	
			}
			else {
				cityAudio.PlayOneShot(civInClip, 0.5f);
			}
			
			if (!discovered && settler.owner == GameManager.Instance.PlayerCiv) { Discover(); }
		}
		else {
			float damage = settler.attack - defense; 
			if (soldiers > 0) { damage -= Random.Range(8f, 18f); }
			damage = Mathf.Clamp(damage, 0.5f, 20.0f);
			accumulatedDamage += damage;
			
			if (soldiers > 0 && accumulatedDamage > 1) {
				soldiers -= Mathf.FloorToInt(accumulatedDamage);
				accumulatedDamage %= 1f;
				
				if (soldiers < 0) { soldiers = 0; }
			}
			else {
				civilians -= Mathf.FloorToInt(accumulatedDamage);
				accumulatedDamage %= 1f;
				
				if (civilians < 0) { civilians = 0; }
			}	
		}
	}
	
	public void AddConnectedPath(MapPath path) {
		if (connectedPaths == null)
		{
			connectedPaths = new List<MapPath>();	
		}
		connectedPaths.Add(path);
	}
	
	public void AddConnectedCity(MapPoint city) {
		if (connectedCities == null)
		{
			connectedCities = new List<MapPoint>();	
		}
		connectedCities.Add(city);	
	}
	
	public void Discover() {
		discovered = true;
		
		SetLayerRecursively(gameObject, 0);
		
		foreach (MapPoint city in connectedCities) {
			SetLayerRecursively(city.gameObject, 0);
		}
		
		foreach (MapPath path in connectedPaths) {
			SetLayerRecursively(path.gameObject, 0);
		}
	}
	
	public void ClearCollider() {
		Destroy(transform.GetComponent<SphereCollider>());
		Destroy(transform.GetComponent<Rigidbody>());
	}
	
	#region AI Methods
	
	public void ConvertFoodToCiv() {
		--food;
		++civilians;
	}
	
	public void ConvertFoodToSoldier() {
		food -= 2;
		++soldiers;
	}
	
	public void ConvertSoldierToCiv() {
		--soldiers;
		++civilians;
	}
	
	public void ConvertCivToSoldier() {
		--civilians;
		--food;
		++soldiers;
	}
	
	public void SendCivsToTarget(MapPoint target) {				
		GameObject obj = Instantiate(citizenPrefab) as GameObject;
		SettlerBehavior citizen = obj.GetComponent<SettlerBehavior>() as SettlerBehavior;
		
		citizen.transform.position = transform.position;
		
		citizen.destination = target;
		
		citizen.owner = occupier;
		
		foreach (MapPath path in connectedPaths) {
			if (path.endPointA == target || path.endPointB == target) {
				citizen.path = path;
			}
		}
		
		citizen.soldier = false;
		citizen.attack = 1f + Random.Range(0f,4f);
		--civilians;
	}
	
	public void SendSoldiersToTarget(MapPoint target) {				
		GameObject obj = Instantiate(citizenPrefab) as GameObject;
		SettlerBehavior citizen = obj.GetComponent<SettlerBehavior>() as SettlerBehavior;
		
		citizen.transform.position = transform.position;
		
		citizen.destination = target;
		
		citizen.owner = occupier;
		
		foreach (MapPath path in connectedPaths) {
			if (path.endPointA == target || path.endPointB == target) {
				citizen.path = path;
			}
		}
		
		citizen.soldier = true;
		citizen.attack = 10f + Random.Range(0f,10f);
		--soldiers;
	}
	
	#endregion
	
	#endregion
	
	#region Private Methods
	
	private void PlayerControlledBehavior() {
		if (Input.GetMouseButtonDown(1)) {
			elapsedTime = 0;
		}
		
		if (GameManager.Instance.SelectedCity == this && Input.GetMouseButton(1))
		{
			elapsedTime += Time.deltaTime;
			
			if (GameManager.Instance.TargetCity == this) {
				if ( leftClickedPart == "ArmyStack" && rightClickedPart == "CivStack" && soldiers > 0 && 
					 elapsedTime > civDelay) {
					elapsedTime -= civDelay;
					--soldiers;
					++civilians;
				}
				else if ( leftClickedPart == "CivStack" && rightClickedPart == "ArmyStack" && civilians > 1 &&
					      elapsedTime > armyDelay && food > 0) {
					elapsedTime -= armyDelay;
					--food;
					++soldiers;
					--civilians;
				}
				else if (leftClickedPart == "FoodStack" && rightClickedPart == "CivStack" && food > 0 && 
						 elapsedTime > civDelay) {
					elapsedTime -= civDelay;
					--food;
					++civilians;
				}
				else if (leftClickedPart == "FoodStack" && rightClickedPart == "ArmyStack" && food > 1 && 
						 elapsedTime > armyDelay && food > 0) {
					elapsedTime -= armyDelay;
					food -= 2;
					++soldiers;
				}
			}
			else if (GameManager.Instance.TargetCity != null && connectedCities.IndexOf(GameManager.Instance.TargetCity) > -1 &&
				     ((leftClickedPart == "CivStack" && civilians > 1) || 
				      (leftClickedPart == "ArmyStack" && soldiers > 0 && food > 0))) {
				MapPoint target = GameManager.Instance.TargetCity;
				
				GameObject obj = Instantiate(citizenPrefab) as GameObject;
				SettlerBehavior citizen = obj.GetComponent<SettlerBehavior>() as SettlerBehavior;
				
				citizen.transform.position = transform.position;
				
				citizen.destination = target;
				
				citizen.owner = occupier;
				
				foreach (MapPath path in connectedPaths) {
					if (path.endPointA == target || path.endPointB == target) {
						citizen.path = path;
					}
				}
				
				if (leftClickedPart == "CivStack") {
					citizen.soldier = false;
					citizen.attack = 1f + Random.Range(0f,4f);
					--civilians;
					
					if (Input.GetMouseButtonDown(1)) {
						cityAudio.PlayOneShot(civOutClip);	
					}
				}
				else {
					citizen.soldier = true;
					citizen.attack = 10f + Random.Range(0f,10f);
					--soldiers;
					--food;
					
					if (Input.GetMouseButtonDown(1)) {
						cityAudio.PlayOneShot(armyOutClip);	
					}
				}
			}
		}
	}
	
	private AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();	
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		newAudio.rolloffMode = AudioRolloffMode.Linear;
		return newAudio;
	}
	
	private void SetLayerRecursively(GameObject obj, int newLayer) {
		obj.layer = newLayer;
		foreach (Transform child in obj.transform) {
			SetLayerRecursively(child.gameObject, newLayer);	
		}
	}
	
	private void OnClickedChild(GameObject childObject) {		
		GameManager.Instance.SelectedCity = this;
		GameManager.Instance.SelectedPath = null;
		
		leftClickedPart = childObject.name;
		
		foreach(Transform childTransform in transform) {
			if (childTransform.gameObject != childObject && childTransform.name != "Flag") {
				MapPointClickHandler clickHandler = childTransform.gameObject.GetComponent<MapPointClickHandler>();
				clickHandler.Deselect();
			}
		}
	}
	
	private void OnRightClickedChild(GameObject childObject) {		
		GameManager.Instance.TargetCity = this;
		
		rightClickedPart = childObject.name;
	}
	
	#endregion
	
	#region Public Properties
	
	public MapPath[] ConnectedPaths {
		get { 
			if (connectedPaths == null) {
				connectedPaths = new List<MapPath>();	
			}
			return connectedPaths.ToArray(); 
		}
	}
	
	public MapPoint[] ConnectedCities {
		get { 
			if (connectedCities == null) {
				connectedCities = new List<MapPoint>();
			}
			return connectedCities.ToArray();
		}	
	}
	
	#endregion
}
