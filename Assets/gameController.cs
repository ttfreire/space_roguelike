using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
	}
}
