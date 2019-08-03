using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public static SceneController instance;

	public float defaultFadeInTime = 2f;
	public float defaultFadeOutTime = 2f;

	private bool changingScenes = false;

	private void Awake () {
		if (null == instance) {
			instance = this;
			DontDestroyOnLoad(transform.root.gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	public static void ResetScene (bool fade) {
		if (instance.changingScenes) return;
		instance.changingScenes = true;
		ResetScene(fade, instance.defaultFadeOutTime, instance.defaultFadeInTime);
	}

	public static void ResetScene (bool fade, float fadeTime) {
		if (instance.changingScenes) return;
		instance.changingScenes = true;
		ResetScene(fade, fadeTime, fadeTime);
	}

	public static void ResetScene (bool fade, float fadeOutTime, float fadeInTime) {
		if (instance.changingScenes) return;
		instance.changingScenes = true;
		int targetIndex = SceneManager.GetActiveScene().buildIndex;
		LoadScene(targetIndex, fade, fadeOutTime, fadeInTime);
	}

	public static void LoadNextScene (bool fade) {
		if (instance.changingScenes) return;
		instance.changingScenes = true;
		LoadNextScene(fade, instance.defaultFadeOutTime, instance.defaultFadeInTime);
	}

	public static void LoadNextScene (bool fade, float fadeTime) {
		if (instance.changingScenes) return;
		instance.changingScenes = true;
		LoadNextScene(fade, fadeTime, fadeTime);
	}

	public static void LoadNextScene (bool fade, float fadeOutTime, float fadeInTime) {
		if (instance.changingScenes) return;
		instance.changingScenes = true;
		int targetIndex = SceneManager.GetActiveScene().buildIndex + 1 % SceneManager.sceneCountInBuildSettings;
		LoadScene(targetIndex, fade, fadeOutTime, fadeInTime);
	}

	public static void LoadScene (int sceneIndex, bool fade, float fadeOutTime, float fadeInTime) {
		if (fade) {
			instance.StartCoroutine(instance.LoadSceneCR(sceneIndex, fadeOutTime, fadeInTime));
		} else {
			SceneManager.LoadScene(sceneIndex);
			instance.changingScenes = false;
		}
	}

	IEnumerator LoadSceneCR (int sceneIndex, float fadeOutTime, float fadeInTime) {
		FadeController.FadeOut(fadeOutTime);
		yield return new WaitForSeconds(fadeOutTime);
		SceneManager.LoadScene(sceneIndex);
		FadeController.FadeIn(fadeInTime);
		instance.changingScenes = false;
	}
}
