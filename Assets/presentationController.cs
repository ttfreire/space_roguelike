using UnityEngine;
using System.Collections;

public class presentationController : MonoBehaviour {
	float timer;
	public GameObject Menu;
	public Canvas start;
	public Canvas idle;
	// Use this for initialization
	void Start () {
		timer = 2;
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0) {
			timer -= Time.deltaTime;
		}
		if (timer < 0) {
			Menu.SetActive (true);
			start.gameObject.SetActive(false);
			idle.gameObject.SetActive(true);
			timer = 0;
		}
	}
}
