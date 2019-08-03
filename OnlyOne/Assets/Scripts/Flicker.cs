﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class Flicker : MonoBehaviour
{
	[HideInInspector] public Vector3 scale;
	[HideInInspector] public Vector3 position;
	private float tau;
	Color color;

	public float scaleFrequency = 1f;
	public float scaleAmplitude = 1f;

	public float shakeAmount = 1f;
	public float shakeRegularization = 1f;

	public float hueRange = 0.1f; // 0 to 1

    // Start is called before the first frame update
    void Start()
    {
		scale = transform.localScale;
		position = transform.localPosition;
		tau = 2 * Mathf.PI;
		color = GetComponent<LightSprite>().Color;
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.localScale.x > 0f) {
			transform.localScale = new Vector3(scale.x + scaleAmplitude * Mathf.Sin(tau * scaleFrequency * Time.time),
											   scale.y + scaleAmplitude * Mathf.Sin(tau * scaleFrequency * Time.time),
											   scale.z + scaleAmplitude * Mathf.Sin(tau * scaleFrequency * Time.time));

			transform.localPosition += new Vector3(Random.value * shakeAmount * 2 - shakeAmount,
											   Random.value * shakeAmount * 2 - shakeAmount,
											   0f) * Time.deltaTime;

			transform.localPosition += -(transform.localPosition - position) * Time.deltaTime;
		} else {
			transform.localScale = Vector3.zero;
		}
	}
}
