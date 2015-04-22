using UnityEngine;
using System.Collections;


public class MoveableController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag.Equals("Projectile"))
			Destroy(other.gameObject);
	}
}
