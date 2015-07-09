using UnityEngine;
using System.Collections;

public class diamondController : MonoBehaviour {

	public Transform target;
	AudioSource sound;
	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.parent == target)
			transform.RotateAround(new Vector3(0,0,1), -Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Player")) {
			transform.parent = target;
			transform.localPosition = new Vector3(0, 3.5f, 0);
			GetComponent<BoxCollider>().enabled = false;
			sound.Play();
			playerController.p_controller.hasDiamond = true;
		}
	}
}
