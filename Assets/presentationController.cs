using UnityEngine;
using System.Collections;

public class presentationController : MonoBehaviour {
	float timer = 2;
	public GameObject Menu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0)
			timer -= Time.deltaTime;
		if (timer < 0)
			Menu.SetActive(true);
	}
}
