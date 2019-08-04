using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class FadeController : MonoBehaviour {
	public static FadeController instance;

	public RawImage fade;
	public AudioMixer audioMixer;
	public float defaultFadeTime = 1f;
	private bool fading = false;

	[Range(0f, 1f)]
	[Tooltip("0 = no fade. 1 = fully black.")]
	public float fadeAmount = 1f;

	private void Awake () {
		fading = false;
		if (null == instance) {
			instance = this;
			DontDestroyOnLoad(transform.root.gameObject);
		} else {
			Destroy(gameObject);
		}
		fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
		FadeIn();
	}

	public static void FadeIn () {
		FadeIn(instance.defaultFadeTime);
	}

	public static void FadeIn (float fadeTime) {
		//print("Fade Time 1: " + fadeTime);
		if (instance.fading) return;
		instance.fading = true;
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.FadeInCR(fadeTime));
	}

	IEnumerator FadeInCR (float fadeTime) {
		//print("Fade Time 2: " + fadeTime);
		float startVolume = 0f;
		float volumeDifference;
		if (audioMixer) {
			audioMixer.GetFloat("Volume", out startVolume);
		}
		volumeDifference = startVolume + 80f;
		fadeAmount = 1f;
		while (fadeAmount > 0f) {
			//print("Fade Amount: " + fadeAmount);
			fadeAmount -= Time.deltaTime / fadeTime;
			fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);

			if (audioMixer) {
				audioMixer.SetFloat("Volume", Mathf.Lerp(0f, - 80f, fadeAmount));
			}
			yield return new WaitForEndOfFrame();
		}
		fadeAmount = 0f;
		fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
		fading = false;
	}

	public static void FadeOut () {
		FadeOut(instance.defaultFadeTime);
	}

	public static void FadeOut (float fadeTime) {
		if (instance.fading) return;
		instance.fading = true;
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.FadeOutCR(fadeTime));
	}

	IEnumerator FadeOutCR (float fadeTime) {
		float startVolume = 1f;
		if (audioMixer) {
			audioMixer.GetFloat("Volume", out startVolume);
		}
		fadeAmount = 0f;
		while (fadeAmount < 1f) {
			fadeAmount += Time.deltaTime / fadeTime;
			fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);

			if (audioMixer) {
				audioMixer.SetFloat("Volume", Mathf.Lerp(0f, -80f, fadeAmount));
			}
			yield return new WaitForEndOfFrame();
		}
		fadeAmount = 1f;
		fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
		fading = false;
	}
}