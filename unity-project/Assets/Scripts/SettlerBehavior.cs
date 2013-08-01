using UnityEngine;
using System.Collections;

public class SettlerBehavior : MonoBehaviour {
	
	public MapPoint destination;
	public MapPath path;
	public string owner;
	public float speed = 100f;
	public bool soldier = true;
	public float attack = 1.0f;
	
	private bool forward;
	private float xOffset;
	private float zOffset;
	private float offset;
	private float animOffset = 0.2f;
	
	private float passedTime = 0f;
	
	private float passedDistance = 0f;
	private float lastParam = 0f;
	
	private Spline spline;
	
	void Awake () {
	}
	
	void Start () {
		spline = path.gameObject.GetComponent<Spline>();
		
		SetLayerRecursively(gameObject, path.gameObject.layer);
		
		float startParam = Mathf.Clamp( spline.GetClosestPointParam(transform.position, 5), 0f, 1f );
		
		if (startParam < 0.5f) {
			forward = true;
			lastParam = 0f;
		}
		else {
			forward = false;
			lastParam = 1f;
		}
		
		offset = Random.Range(0f,0.02f);
		xOffset = Random.Range(-0.4f,0.4f);
		zOffset = Random.Range(-0.4f,0.4f);
		
		if (Random.Range(1,100) > 10) {
			Transform flag = transform.Find("Flag");
			flag.gameObject.SetActive(false);
		}
		
		if (!soldier) {
			Color villagerColor = 
				new Color(.0549f, 0.3960f, .0941f, 1);
		
			Renderer r = GetComponent(typeof(Renderer)) as Renderer;
			r.material.SetColor("_Color", villagerColor);
		}
		
		transform.Find("Flag").GetComponent<FlagHandler>().SetMaterial(owner);
	}
	
	void Update () {
		if (GameManager.Instance.Paused) { return; }
		
		passedTime += Time.deltaTime * speed / path.length;
		
		float clampedParam = 0f;
		if (forward) {
			clampedParam = Mathf.Clamp(passedTime + offset, 0f, 1f);
			passedDistance += (clampedParam - lastParam) * path.length;
		}
		else {
			clampedParam = Mathf.Clamp(1f - (passedTime + offset), 0f, 1f);
			passedDistance += (lastParam - clampedParam) * path.length;
		}
		
		if (passedDistance > path.deathRollInterval) {
			passedDistance -= path.deathRollInterval;
			
			float chance = 0f;
			
			if (soldier) {
				chance = (path.difficulty * path.deathChanceSoldier) / 100;
			}
			else {
				chance = (path.difficulty * path.deathChanceCiv) / 100;
			}
			
			if (Random.Range(0f,1f) < chance) {
				path.Death();
				Destroy(gameObject);
			}
		}
		
		animOffset = Mathf.PingPong(passedTime * 50.0f, 0.8f) - 0.4f + Random.Range(0, 0.02f);
		
		if ((forward && clampedParam == 1f) || (!forward && clampedParam == 0f)) {
			if (soldier) {
				destination.SoldierArrived(this);	
			}
			else {
				destination.CivilianArrived(this);	
			}
			
			Destroy(gameObject);
		}
		else {
			transform.position = spline.GetPositionOnSpline(clampedParam);
			transform.rotation = spline.GetOrientationOnSpline(clampedParam);
			
			transform.position += Vector3.up * transform.localScale.y * 0.5f;
			transform.position += Vector3.up * animOffset;
			transform.position += Vector3.forward * zOffset;
			transform.position += Vector3.right * xOffset;
			
			lastParam = clampedParam;
		}
	}
	
	private void SetLayerRecursively(GameObject obj, int newLayer) {
		obj.layer = newLayer;
		foreach (Transform child in obj.transform) {
			SetLayerRecursively(child.gameObject, newLayer);	
		}
	}
}
