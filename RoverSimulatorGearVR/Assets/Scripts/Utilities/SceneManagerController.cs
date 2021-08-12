using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour {

	public static void loadScene(string scene){
		SceneManager.LoadScene (scene);
	}
}
