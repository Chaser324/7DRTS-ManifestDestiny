using UnityEngine;
using System.Collections;

public class BoatBehavior : MonoBehaviour {
	
	private Spline spline;
	
	private float passedTime = 0f;
	private float speed = 0.04f;
	
	private bool started = false;
	
	// Update is called once per frame
	void Update () {
		if (spline != null && started) {
			passedTime += Time.deltaTime * speed;
			
			float clampedParam = 0f;
			clampedParam = Mathf.Clamp(passedTime, 0f, 1f);
			
			Vector3 nextPosition = spline.GetPositionOnSpline(clampedParam);
			nextPosition.y = transform.position.y;
			
			transform.position = nextPosition;
			transform.rotation = spline.GetOrientationOnSpline(clampedParam);
			
			//transform.position += Vector3.up * transform.localScale.y * 0.5f;
		}
	}

	public void Go(Spline path) {
		spline = path;
		started = true;
	}
}
