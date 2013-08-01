using UnityEngine;
using System.Collections;

public class UICityInfo : MonoBehaviour {
	
	private UILabel nameLabel;
	private UILabel armyCountLabel;
	private UILabel civCountLabel;
	private UILabel foodCountLabel;
	private UILabel ownerLabel;
	private UILabel occupierLabel;
	private UILabel deadLabel;
	
	private Transform deadIcon;
	private Transform civIcon;
	private Transform armyIcon;
	private Transform foodIcon;
	
	private bool visible;
	
	// Use this for initialization
	void Start () {
		nameLabel = transform.Find("Name").GetComponent<UILabel>() as UILabel;
		armyCountLabel = transform.Find("ArmyCount").GetComponent<UILabel>() as UILabel;
		civCountLabel = transform.Find("CivCount").GetComponent<UILabel>() as UILabel;
		foodCountLabel = transform.Find("FoodCount").GetComponent<UILabel>() as UILabel;
		ownerLabel = transform.Find("Owner").GetComponent<UILabel>() as UILabel;
		occupierLabel = transform.Find("Occupier").GetComponent<UILabel>() as UILabel;
		deadLabel = transform.Find("DeadCount").GetComponent<UILabel>() as UILabel;
		
		deadIcon = transform.Find("DeadIcon");
		civIcon = transform.Find("CivIcon");
		armyIcon = transform.Find("ArmyIcon");
		foodIcon = transform.Find("FoodIcon");
		
		visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (visible && GameManager.Instance.SelectedCity == null && GameManager.Instance.SelectedPath == null) {
			visible = false;
			foreach (Transform child in transform) {
				child.gameObject.SetActive(false);	
			}
		}
		else {
			if (!visible) {
				visible = true;
				foreach (Transform child in transform) {
					child.gameObject.SetActive(true);	
				}
			}
			
			if (GameManager.Instance.SelectedCity != null) {
				//nameLabel.text = GameManager.Instance.SelectedCity.cityName;
				armyCountLabel.text = GameManager.Instance.SelectedCity.soldiers.ToString();
				civCountLabel.text = GameManager.Instance.SelectedCity.civilians.ToString();
				foodCountLabel.text = GameManager.Instance.SelectedCity.food.ToString();
				
				civIcon.gameObject.SetActive(true);
				civCountLabel.gameObject.SetActive(true);
				armyIcon.gameObject.SetActive(true);
				armyCountLabel.gameObject.SetActive(true);
				foodIcon.gameObject.SetActive(true);
				foodCountLabel.gameObject.SetActive(true);
				
				deadIcon.gameObject.SetActive(false);
				deadLabel.gameObject.SetActive(false);
				
				if (GameManager.Instance.SelectedCity.owner != "") {
					nameLabel.text = GameManager.Instance.SelectedCity.owner + " Colony";
					ownerLabel.text = "";
				}
				else {
					nameLabel.text = "Unsettled Land";
					ownerLabel.text = "";
				}
				
				if (GameManager.Instance.SelectedCity.owner != GameManager.Instance.SelectedCity.occupier) {
					occupierLabel.text = "Occupied by " + GameManager.Instance.SelectedCity.occupier;
				}
				else {
					occupierLabel.text = "";	
				}
				
			}
			else if (GameManager.Instance.SelectedPath != null) {
				//nameLabel.text = GameManager.Instance.SelectedPath.pathName;
				nameLabel.text = "Path";
				deadLabel.text = GameManager.Instance.SelectedPath.deaths.ToString();
				ownerLabel.text = GameManager.Instance.SelectedPath.length.ToString("0.00") + " miles";
				occupierLabel.text = GameManager.Instance.SelectedPath.difficulty.ToString("0.0") + "/5.0 Difficulty";
				
				civIcon.gameObject.SetActive(false);
				civCountLabel.gameObject.SetActive(false);
				armyIcon.gameObject.SetActive(false);
				armyCountLabel.gameObject.SetActive(false);
				foodIcon.gameObject.SetActive(false);
				foodCountLabel.gameObject.SetActive(false);
				
				deadIcon.gameObject.SetActive(true);
				deadLabel.gameObject.SetActive(true);
			}
		}
	}
}
