using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
	public float delay = 2f;
	public float fadeInTime = 1f;
	public float holdTime = 4f;
	public float fadeOutTime = 1f;

	private void Start () {
		StartCoroutine(RunIntro());
	}

	IEnumerator RunIntro () {
		yield return new WaitForSeconds(delay);
		FadeController.FadeIn(fadeInTime);
		yield return new WaitForSeconds(fadeInTime + holdTime);
		SceneController.LoadNextScene(true, fadeOutTime, 2f);
		//FadeController.FadeOut(fadeOutTime);
		//yield return new WaitForSeconds(fadeOutTime);
		//int targetIndex = SceneManager.GetActiveScene().buildIndex + 1 % SceneManager.sceneCountInBuildSettings;
		//print("Index: " + targetIndex);
		//SceneManager.LoadScene(targetIndex);
	}
}