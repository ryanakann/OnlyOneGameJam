using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStrength : MonoBehaviour {
	[Header("Strength")]
	public float maxStrength = 100f;
	public float currentStrength = 100f;
	public float percentLossPerSecond = 1f;

	[Header("Particle System")]
	[Tooltip("Fill these in manually with any Particle Systems you want to decrease!")]
	public ParticleSystem[] particleSystems;
	private int particleSystemCount;

	public float[] initialParticleEmissionRates;
	public float[] rateOfDecreaseOfParticleEmissionRates;

	[Header("Light")]
	[Tooltip("Fill these in manually with any lights you want to dim!")]
	public Transform[] lights;
	private int lightsCount;
	public Vector3[] initialLightSizes;
	public Vector3[] rateOfDecreaseOfLightSizes;

	private void Start () {
		//Strength
		currentStrength = maxStrength;

		//Particle Systems
		particleSystemCount = particleSystems.Length;
		initialParticleEmissionRates = new float[particleSystemCount];
		rateOfDecreaseOfParticleEmissionRates = new float[particleSystemCount];
		for (int i = 0; i < particleSystemCount; i++) {
			float emissionRate = particleSystems[i].emission.rateOverTime.constant;
			initialParticleEmissionRates[i] = emissionRate;
			rateOfDecreaseOfParticleEmissionRates[i] = emissionRate * percentLossPerSecond / 100f;
		}

		//Lights
		lightsCount = lights.Length;
		initialLightSizes = new Vector3[lightsCount];
		rateOfDecreaseOfLightSizes = new Vector3[lightsCount];
		for (int i = 0; i < lightsCount; i++) {
			initialLightSizes[i] = lights[i].localScale;
			rateOfDecreaseOfLightSizes[i] = initialLightSizes[i] * percentLossPerSecond / 100f;
		}
	}

	private void Update () {
		if (Input.GetKeyDown(KeyCode.F)) {
			ResetStrength();
		}

		DecreaseStrength();
	}

	private void ResetStrength () {
		print("Strength Reset!");
		//Strength
		currentStrength = maxStrength;

		//Particle Systems
		for (int i = 0; i < particleSystemCount; i++) {
			ParticleSystem.EmissionModule emmision = particleSystems[i].emission;
			ParticleSystem.MinMaxCurve tempCurve = emmision.rateOverTime;
			tempCurve.constant = initialParticleEmissionRates[i];
			emmision.rateOverTime = tempCurve;
		}

		//Lights
		for (int i = 0; i < lightsCount; i++) {
			if (lights[i].GetComponent<Flicker>()) {
				lights[i].GetComponent<Flicker>().scale = initialLightSizes[i];
			} else {
				lights[i].localScale = initialLightSizes[i];
			}
		}
	}

	private void DecreaseStrength () {
		//Strength
		currentStrength -= percentLossPerSecond * Time.deltaTime;

		if (currentStrength < 0f) {
			Extinguish();
			return;
		}

		//Particle Systems
		for (int i = 0; i < particleSystemCount; i++) {
			ParticleSystem.EmissionModule emmision = particleSystems[i].emission;
			ParticleSystem.MinMaxCurve tempCurve = emmision.rateOverTime;
			tempCurve.constant -= rateOfDecreaseOfParticleEmissionRates[i] * Time.deltaTime;
			emmision.rateOverTime = tempCurve;
		}

		//Lights
		for (int i = 0; i < lightsCount; i++) {
			Flicker flicker;
			if (null != (flicker = lights[i].GetComponent<Flicker>())) {
				flicker.scale -= rateOfDecreaseOfLightSizes[i] * Time.deltaTime;
				if (flicker.scale.x < 0f) {
					flicker.scale = Vector3.zero;
				}
			} else {
				lights[i].localScale -= rateOfDecreaseOfLightSizes[i] * Time.deltaTime;
			}
		}
	}

	private void Extinguish () {
		print("Dead!");
	}

	private void OnTriggerStay2D (Collider2D collision) {
		if (collision.CompareTag("Fire Pit")) {
			ResetStrength();
		}
	}
}
