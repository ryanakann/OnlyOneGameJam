using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour {
	SpriteRenderer sr;
	bool changingColors = false;

	private void Awake () {
		sr = GetComponent<SpriteRenderer>();
		changingColors = false;
	}

	public void SetRed () {
		SetColor(Color.red);
	}

	public void SetGreen () {
		SetColor(Color.green);
	}

	public void SetColor (Color color) {
		sr.color = color;
	}

	public void SetColor (Color color, float time) {
		if (changingColors) return;
		changingColors = true;
		StartCoroutine(SetColorCR(color, time));
	}

	private IEnumerator SetColorCR (Color color, float time) {
		float timeElapsed = 0f;
		Color startColor = sr.color;
		Color endColor = color;
		while (timeElapsed < time) {
			sr.color = Color.Lerp(startColor, endColor, timeElapsed / time);
			timeElapsed += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		sr.color = color;
	}
}
