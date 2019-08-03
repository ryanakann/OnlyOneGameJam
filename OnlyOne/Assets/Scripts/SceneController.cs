using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public static SceneController instance;

	public float defaultFadeInTime = 2f;
	public float defaultFadeOutTime = 2f;

	private void Awake () {
		if (null == instance) {
			instance = this;
			DontDestroyOnLoad(transform.root.gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	public static void LoadNextScene (bool fade) {
		LoadNextScene(fade, instance.defaultFadeOutTime, instance.defaultFadeInTime);
	}

	public static void LoadNextScene (bool fade, float fadeTime) {
		LoadNextScene(fade, fadeTime, fadeTime);
	}

	public static void LoadNextScene (bool fade, float fadeOutTime, float fadeInTime) {
		if (fade) {
			instance.StartCoroutine(instance.LoadSceneCR(fadeOutTime, fadeInTime));
		} else {
			int targetIndex = SceneManager.GetActiveScene().buildIndex + 1 % SceneManager.sceneCountInBuildSettings;
			SceneManager.LoadScene(targetIndex);
		}
	}

	IEnumerator LoadSceneCR (float fadeOutTime, float fadeInTime) {
		int targetIndex = SceneManager.GetActiveScene().buildIndex + 1 % SceneManager.sceneCountInBuildSettings;
		FadeController.FadeOut(fadeOutTime);
		yield return new WaitForSeconds(fadeOutTime);
		SceneManager.LoadScene(targetIndex);
		FadeController.FadeIn(fadeInTime);
	}
}
