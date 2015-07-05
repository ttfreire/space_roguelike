using UnityEngine;
using System.Collections;

public class destroyAfterXseconds : MonoBehaviour {
	public float SecondsToDestroy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (SecondsToDestroy <= 0.0f)
			Destroy (gameObject);
		SecondsToDestroy -= Time.deltaTime;
	}
}
