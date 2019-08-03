using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventHandler : MonoBehaviour {
	public UnityEvent OnButtonDown;
	public UnityEvent OnButtonStay;
	public UnityEvent OnButtonUp;

	public Transform buttonTop;
	public Vector3 startPosition;
	public Vector3 currentPosition;
	public Vector3 difference;

	public bool buttonPressed;
	public bool buttonPressedLF;

	private void Start () {
		if (buttonTop) {
			startPosition = buttonTop.position;
		} else {
			startPosition = Vector3.zero;
		}

		buttonPressed = false;
		buttonPressedLF = buttonPressed;
	}

	private void Update () {
		buttonPressed = (buttonTop.position.y - startPosition.y < -0.05f);

		currentPosition = buttonTop.position;
		difference = currentPosition - startPosition;

		if (buttonPressed && !buttonPressedLF) {
			print("Button Down!");
			OnButtonDown.Invoke();
		} else if (buttonPressedLF && !buttonPressed) {
			print("Button Up!");
			OnButtonUp.Invoke();
		} else if (buttonPressed && buttonPressedLF) {
			print("Button Stay!");
			OnButtonStay.Invoke();
		}

		buttonPressedLF = buttonPressed;
	}
}