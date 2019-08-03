using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventHandler : MonoBehaviour {
	private bool buttonPressed;
	private bool buttonPressedLF;
	[SerializeField] private bool stayPressed;

	public UnityEvent OnButtonDown;
	public UnityEvent OnButtonStay;
	public UnityEvent OnButtonUp;

	public Transform buttonTop;
	private Vector3 startPosition;
	private Vector3 currentPosition;
	private Vector3 difference;

	private void Start () {
		if (buttonTop) {
			startPosition = buttonTop.localPosition;
		} else {
			startPosition = Vector3.zero;
		}

		buttonPressed = false;
		buttonPressedLF = buttonPressed;

		Rigidbody2D rb;

		if (null != (rb = buttonTop.GetComponent<Rigidbody2D>())) {
			if (Mathf.Abs(transform.up.x) > Mathf.Abs(transform.up.y)) {
				rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			} else {
				rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
			}

		}
	}

	private void Update () {
		buttonPressed = (buttonTop.localPosition.y - startPosition.y < -0.05f);

		currentPosition = buttonTop.position;
		difference = currentPosition - startPosition;

		if (buttonPressed && !buttonPressedLF) {
			//print("Button Down!");
			if (stayPressed) {
				SpringJoint2D spring;
				if (null != (spring = buttonTop.GetComponent<SpringJoint2D>())) {
					print("Constrained!");
					spring.distance = 0.5f;
					spring.frequency = 5f;
				}
			}
			OnButtonDown.Invoke();
		} else if (buttonPressedLF && !buttonPressed) {
			//print("Button Up!");
			OnButtonUp.Invoke();
		} else if (buttonPressed && buttonPressedLF) {
			//print("Button Stay!");
			OnButtonStay.Invoke();
		}

		buttonPressedLF = buttonPressed;
	}
}