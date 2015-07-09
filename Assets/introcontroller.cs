using UnityEngine;
using System.Collections;

public class introcontroller : MonoBehaviour {
	float timer;
	// Use this for initialization
	void Start () {
		timer = 6;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0)
			timer -= Time.deltaTime;
		else
			Application.LoadLevel ("MainScreen");
	}
}
