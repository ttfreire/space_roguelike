﻿using UnityEngine;
using System.Collections;

public class mainScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
			Application.LoadLevel ("prototypeMovement");
	}
}
