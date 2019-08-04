using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
	public float fadeInTime = 1f;
	public float holdTime = 4f;
	public float fadeOutTime = 1f;

	public bool intro = false;
	public bool awaitingKeyPress = false;

	public UnityEngine.UI.Text awaitingText;
	public AudioSource music;

	private void Start () {
		awaitingText.color = new Color(awaitingText.color.r, awaitingText.color.g, awaitingText.color.b, 0f);
		StartCoroutine(RunIntro());
	}

	private void Update () {
		if (!Input.GetKeyDown(KeyCode.Escape) && Input.anyKeyDown) {
			if (intro) {
				intro = false;
				StopAllCoroutines();
				StartCoroutine(AwaitKeyPress(0f));
			} else if (awaitingKeyPress) {
				awaitingKeyPress = false;
				SceneController.LoadNextScene(true, fadeOutTime, 2f);
			}
		}
	}

	IEnumerator AwaitKeyPress (float fadeDuration) {
		//print("Fade Duration: " + fadeDuration);
		float t = 0f;
		Color color = awaitingText.color;
		if (fadeDuration > 0f) {
			while (t < 1f) {
				color.a = t;
				awaitingText.color = color;
				t += Time.deltaTime / fadeDuration;
				yield return new WaitForEndOfFrame();
			}
		}
		color.a = 1f;
		awaitingText.color = color;
		awaitingKeyPress = true;
	}

	IEnumerator RunIntro () {
		//print("Fade Time 0: " + fadeInTime);
		//FadeController.FadeIn(fadeInTime);
		yield return new WaitForSeconds(fadeInTime * 0.65f);
		music.Play();
		yield return new WaitForSeconds(fadeInTime * 0.35f);
		intro = true;
		yield return new WaitForSeconds(holdTime);
		intro = false;
		StartCoroutine(AwaitKeyPress(1f));
		yield return new WaitForSeconds(1f);
		awaitingKeyPress = true;
	}
}