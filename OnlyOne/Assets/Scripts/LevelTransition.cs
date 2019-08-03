using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour {
	public void NextLevel () {
		FindObjectOfType<UnityStandardAssets._2D.PlatformerCharacter2D>().ToggleInput(false);
		SceneController.LoadNextScene(true);
	}
}