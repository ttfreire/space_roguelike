using UnityEngine;
using System.Collections;

public class Restartlevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartLevel(){
		Application.LoadLevel (Application.loadedLevel);
		Time.timeScale = 1;
	}
}
