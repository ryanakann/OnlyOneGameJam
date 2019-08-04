using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	//bool quit = false;


	private void Start () {
		//	//quit = false;
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(transform.root.gameObject);
		} else {
			Destroy(gameObject);
		}

	}

	private void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			StartCoroutine(Quit());
		}

		//if (quit) {
			//Application.Quit();
		//}
	}

	private IEnumerator Quit () {
		FadeController.FadeOut(2f);
		yield return new WaitForSeconds(2f);
		Application.Quit(0);
	}
}