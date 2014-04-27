using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {
	public GameObject lookAtObj;
	private float rotateSpeed = 5f;

	private bool buttonPressed = false;

	public float smooth = 1;
	private Vector3 startPosition;
	public Vector3 endPosition;
	public Vector3 startRotation;
	public Vector3 endRotation;

	void Start () {
		startPosition = transform.position;
		startRotation = transform.eulerAngles;
	}
	// Update is called once per frame
	void Update () {
		//while(!buttonPressed) {
			//transform.RotateAround (Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
		//}
		//endPosition = new Vector3(0, 0, 0);
		//endRotation = new Vector3(10, 0, 0);
		if (Input.GetMouseButton (0)) {
			transform.position = Vector3.Lerp(transform.position, endPosition, smooth * Time.deltaTime);
		}
		//transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, 1.0f * Time.deltaTime);
	}

}
