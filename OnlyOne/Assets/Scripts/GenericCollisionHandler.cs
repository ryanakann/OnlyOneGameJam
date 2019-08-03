using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericCollisionHandler : MonoBehaviour {
	public UnityEvent OnPlayerTriggerEnter2D;
	public UnityEvent OnPlayerTriggerStay2D;
	public UnityEvent OnPlayerTriggerExit2D;
	public UnityEvent OnPlayerCollisionEnter2D;
	public UnityEvent OnPlayerCollisionStay2D;
	public UnityEvent OnPlayerCollisionExit2D;

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.CompareTag("Player")) {
			OnPlayerTriggerEnter2D.Invoke();
		}
	}

	private void OnTriggerStay2D (Collider2D collision) {
		if (collision.CompareTag("Player")) {
			OnPlayerTriggerStay2D.Invoke();
		}
	}

	private void OnTriggerExit2D (Collider2D collision) {
		if (collision.CompareTag("Player")) {
			OnPlayerTriggerExit2D.Invoke();
		}
	}

	private void OnCollisionEnter2D (Collision2D collision) {
		if (collision.collider.CompareTag("Player")) {
			OnPlayerCollisionEnter2D.Invoke();
		}
	}

	private void OnCollisionStay2D (Collision2D collision) {
		if (collision.collider.CompareTag("Player")) {
			OnPlayerCollisionStay2D.Invoke();
		}
	}

	private void OnCollisionExit2D (Collision2D collision) {
		if (collision.collider.CompareTag("Player")) {
			OnPlayerCollisionExit2D.Invoke();
		}
	}
}