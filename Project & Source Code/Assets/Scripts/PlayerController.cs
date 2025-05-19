using UnityEngine;

public class DroneController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float ascentSpeed = 5f;
	public float mouseSensitivity = 200f;
	public Transform playerCameraTransform;

	private float xRotation = 0;

	// WARNING this drone like movement script for the camera/player
	// is mostly authored by ai, as it was not the focus of the project
	// do not trust it to be efficient or bug free for real production use
	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (playerCameraTransform == null) {
			if (Camera.main != null && Camera.main.transform.IsChildOf(transform)) {
				playerCameraTransform = Camera.main.transform;
			}
			else {
				Camera childCam = GetComponentInChildren<Camera>();
				if (childCam != null) {
					playerCameraTransform = childCam.transform;
				}
			}
		}

		// set camera level
		playerCameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
	}

	// movement and look logic
	void Update() {
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		transform.Rotate(Vector3.up * mouseX);

		if (playerCameraTransform != null) {
			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			playerCameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput);

		if (moveDirection.sqrMagnitude > 0.01f) 
		{
			transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.E)) {
			transform.position += transform.up * ascentSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Q)) {
			transform.position -= transform.up * ascentSpeed * Time.deltaTime; 
		}
	}
}
