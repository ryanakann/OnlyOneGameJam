using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public enum OpenDirection { up, down, left, right }

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 openVector;

	public OpenDirection direction;
	public float openDuration = 2f;

	private void Start () {
		startPosition = transform.position;

		if (GetComponent<SpriteRenderer>()) {

			switch (direction) {
				case OpenDirection.up:
					endPosition = startPosition + transform.up * GetComponent<SpriteRenderer>().size.y;
					break;
				case OpenDirection.down:
					endPosition = startPosition + -transform.up * GetComponent<SpriteRenderer>().size.y;
					break;
				case OpenDirection.left:
					endPosition = startPosition + -transform.right * GetComponent<SpriteRenderer>().size.x;
					break;
				case OpenDirection.right:
					endPosition = startPosition + transform.right * GetComponent<SpriteRenderer>().size.x;
					break;
				default:
					endPosition = startPosition;
					break;
			}
		}

		openVector = endPosition - startPosition;
	}

	public void Open () {
		//print("Opening door!");
		StopAllCoroutines();
		StartCoroutine(OpenCR());
	}

	private IEnumerator OpenCR () {
		while ((endPosition - transform.position).sqrMagnitude > openVector.magnitude / openDuration * Time.deltaTime) {
			transform.position += openVector / openDuration * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = endPosition;
	}

	public void Close () {
		//print("Closing door!");
		StopAllCoroutines();
		StartCoroutine(CloseCR());
	}

	private IEnumerator CloseCR () {
		while ((startPosition - transform.position).sqrMagnitude > openVector.magnitude / openDuration * Time.deltaTime) {
			transform.position -= openVector / openDuration * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = startPosition;
	}
}
