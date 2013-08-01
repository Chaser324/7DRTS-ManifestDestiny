using UnityEngine;
using System.Collections;

public class RtsCamera : MonoBehaviour {
	public float scrollSpeed = 50;
	public int scrollDistance = 50;
	public float scrollDragSpeed = 50;
	
	public float zoomSensitivity = 50;
	public float zoomDamping = 5;
	public float zoomMaxFOV = 90;
	public float zoomMinFOV = 20;
	public float zoomMaxHeight = 200;
	public float zoomMinHeight = 40;
	
	private Camera theCamera;
	private float zoomDistance = 60;
	private float zoomHeight = 40;
	
	void Start () {
		theCamera = Camera.main;
		zoomDistance = theCamera.fieldOfView;
	}
	
	void Update () {
		if (GameManager.Instance.Paused) { return; }
		
		MoveCamera();
		ZoomCamera();
	}
	
	private void MoveCamera() {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		
		bool collisionLeft = false;
		bool collisionRight = false;
		bool collisionUp = false;
		bool collisionDown = false;
		
		if ( transform.position.x < 100 ) {
			collisionLeft = true;
		}
		else if ( transform.position.x > 800 ) {
			collisionRight = true;
		}
		
		if ( transform.position.z > 900 ) {
			collisionUp = true;
		}
		else if ( transform.position.z < 0 ) {
			collisionDown = true;	
		}
		
		float currentScrollSpeed = scrollSpeed + scrollSpeed * zoomHeight / zoomMaxHeight + scrollSpeed * zoomDistance /zoomMaxFOV;
		
		// Middle Click + Drag Scrolling
		if ( (Input.GetKey("left alt") || Input.GetKey("right alt")) || Input.GetMouseButton(2) ) {
			Vector3 mouseTranslate = new Vector3(Input.GetAxis("Mouse X")*scrollDragSpeed, 0, Input.GetAxis("Mouse Y")*scrollDragSpeed);
			
			if (collisionLeft && mouseTranslate.x > 0) { mouseTranslate.x = 0; }
			else if (collisionRight && mouseTranslate.x < 0) { mouseTranslate.x = 0; }
			if (collisionUp && mouseTranslate.z < 0) { mouseTranslate.z = 0; }
			else if (collisionDown && mouseTranslate.z > 0) { mouseTranslate.z = 0; }
			
			transform.Translate( -1 * mouseTranslate );
		}
		// WASD Scrolling
		else if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") ||
			Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || 
			Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
			
			if ((Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) && !collisionUp) {
				transform.Translate(transform.forward * currentScrollSpeed * Time.deltaTime);
			}
			if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && !collisionDown) {
				transform.Translate(transform.forward * -currentScrollSpeed * Time.deltaTime);
			}
			if ((Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) && !collisionLeft) {
			transform.Translate(Vector3.right * -currentScrollSpeed * Time.deltaTime);	
			}
			if ((Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) && !collisionRight) {
				transform.Translate(Vector3.right * currentScrollSpeed * Time.deltaTime);	
			}
		}
		// Screen Edge Scrolling
		else {
			if (mouseX < scrollDistance && !collisionLeft) {
				transform.Translate(Vector3.right * -currentScrollSpeed * Time.deltaTime);	
			}
			else if (mouseX >= Screen.width - scrollDistance && !collisionRight) {
				transform.Translate(Vector3.right * currentScrollSpeed * Time.deltaTime);	
			}
			
			if (mouseY < scrollDistance && !collisionDown) {
				transform.Translate(transform.forward * -currentScrollSpeed * Time.deltaTime);	
			}
			else if (mouseY >= Screen.height - scrollDistance && !collisionUp) {
				transform.Translate(transform.forward * currentScrollSpeed * Time.deltaTime);	
			}
				
		}
		
	}
	
	private void ZoomCamera() {
		float scrollInput = Input.GetAxis("Mouse ScrollWheel");
		
		if ((zoomDistance == zoomMaxFOV && scrollInput < 0) || (zoomHeight > zoomMinHeight)) {
			
			theCamera.transform.eulerAngles = new Vector3(Mathf.LerpAngle(theCamera.transform.eulerAngles.x, 87, Time.deltaTime * 3),0,0);
			
			zoomHeight -= scrollInput * zoomSensitivity;
			zoomHeight = Mathf.Clamp(zoomHeight, zoomMinHeight, zoomMaxHeight);
			
			float currentHeight = transform.position.y;
			float targetHeight = Mathf.Lerp(currentHeight, zoomHeight, Time.deltaTime * zoomDamping);
			
			transform.Translate(0,(targetHeight-currentHeight),0);	
		}
		else {			
			theCamera.transform.eulerAngles = new Vector3(Mathf.LerpAngle(theCamera.transform.eulerAngles.x, 60, Time.deltaTime * 3),0,0);
			
			zoomDistance -= scrollInput * zoomSensitivity;
			zoomDistance = Mathf.Clamp(zoomDistance, zoomMinFOV, zoomMaxFOV);
			theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, zoomDistance, Time.deltaTime * zoomDamping);
		}
		
//		if (scrollInput != 0) {
//			Ray ray = theCamera.ViewportPointToRay(theCamera.ScreenToViewportPoint(Input.mousePosition));
//			theCamera.transform.Translate(ray.direction);	
//		}
	}
}
