using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : MonoBehaviour {
	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.CompareTag("Player")) {
			//SceneCon
		}
	}
}
