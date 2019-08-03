using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStrength : MonoBehaviour {
	[Header("Audio")]
	public AudioClip igniteSound;
	public AudioSource loopSource;
	public AudioSource ignitionSource;
	public AudioSource heartbeatSource;

	[Header("Strength")]
	public float maxStrength = 100f;
	public float currentStrength = 100f;
	public float percentLossPerSecond = 1f;
	private bool resettingStrength;
	private bool resettingStrengthLF;
	private bool dead;
	private bool deadLF;

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
		resettingStrength = false;
		resettingStrengthLF = false;
		dead = false;
		deadLF = dead;

		//Audio
		//loopSource = gameObject.GetComponent<AudioSource>();
		//ignitionSource = gameObject.AddComponent<AudioSource>();
		//loopSource.volume = 0.5f;
		//ignitionSource.clip = igniteSound;
		//ignitionSource.outputAudioMixerGroup = loopSource.outputAudioMixerGroup;
		//ignitionSource.spatialBlend = 0f;
		//ignitionSource.volume = 0.5f;
		//ignitionSource.playOnAwake = false;
		//ignitionSource.loop = false;

		//Particle Systems
		particleSystemCount = particleSystems.Length;
		initialParticleEmissionRates = new float[particleSystemCount];
		rateOfDecreaseOfParticleEmissionRates = new float[particleSystemCount];
		for (int i = 0; i < particleSystemCount; i++) {
			float emissionRate = particleSystems[i].emission.rateOverTime.constant;
			initialParticleEmissionRates[i] = emissionRate;
			rateOfDecreaseOfParticleEmissionRates[i] = emissionRate * percentLossPerSecond / maxStrength;
		}

		//Lights
		lightsCount = lights.Length;
		initialLightSizes = new Vector3[lightsCount];
		rateOfDecreaseOfLightSizes = new Vector3[lightsCount];
		for (int i = 0; i < lightsCount; i++) {
			initialLightSizes[i] = lights[i].localScale;
			rateOfDecreaseOfLightSizes[i] = initialLightSizes[i] * percentLossPerSecond / maxStrength;
		}
	}

	private void Update () {
		if (Input.GetKeyDown(KeyCode.F)) {
			ResetStrength();
		}

		//print(currentStrength / maxStrength);
		if (currentStrength > 0f) {
			heartbeatSource.volume = (currentStrength / maxStrength > 0.5f ? 0 : 0.5f * (1 - (currentStrength / maxStrength + 0.5f)));
 		} else {
			heartbeatSource.volume = 0f;
		}

		DecreaseStrength();
	}

	IEnumerator IncreaseAudio () {
		while (loopSource.volume < 0.5f) {
			loopSource.volume += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		loopSource.volume = 0.5f;
	}

	private void ResetStrength () {
		//Strength
		currentStrength = maxStrength;
		resettingStrength = true;

		//Audio
		loopSource.volume = 0.5f;
		//if (resettingStrength && !resettingStrengthLF) {
		//	ignitionSource.Play();
		//	StartCoroutine(IncreaseAudio());
		//}

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

		//Audio
		loopSource.volume -= percentLossPerSecond / (maxStrength * 2f) * Time.deltaTime;

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
		dead = true;
		if (dead && !deadLF) {
			print("Dead!");
			SceneController.ResetScene(true, 5f);
		}
		deadLF = true;
	}

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.CompareTag("Fire Pit")) {
			if (!ignitionSource.isPlaying) {
				ignitionSource.Play();
			}
		}
	}

	private void OnTriggerStay2D (Collider2D collision) {
		if (collision.CompareTag("Fire Pit")) {
			ResetStrength();
		}
	}
}
