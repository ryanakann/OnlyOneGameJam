using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour {
	public static FadeController instance;

	public RawImage fade;
	public float defaultFadeTime = 1f;

	[Range(0f, 1f)]
	[Tooltip("0 = no fade. 1 = fully black.")]
	public float fadeAmount = 1f;

	private void Awake () {
		if (null == instance) {
			instance = this;
			DontDestroyOnLoad(transform.root.gameObject);
		} else {
			Destroy(gameObject);
		}
		fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
	}

	public static void FadeIn () {
		FadeIn(instance.defaultFadeTime);
	}

	public static void FadeIn (float fadeTime) {
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.FadeInCR(fadeTime));
	}

	IEnumerator FadeInCR (float fadeTime) {
		while (fadeAmount > 0f) {
			fadeAmount -= Time.deltaTime / fadeTime;
			fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
			yield return new WaitForEndOfFrame();
		}
		fadeAmount = 0f;
		fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
	}

	public static void FadeOut () {
		FadeOut(instance.defaultFadeTime);
	}

	public static void FadeOut (float fadeTime) {
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.FadeOutCR(fadeTime));
	}

	IEnumerator FadeOutCR (float fadeTime) {
		while (fadeAmount < 1f) {
			fadeAmount += Time.deltaTime / fadeTime;
			fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
			yield return new WaitForEndOfFrame();
		}
		fadeAmount = 1f;
		fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
	}
}