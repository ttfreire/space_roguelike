using UnityEngine;
using System.Collections;

public class mineController : enemyBaseController {
	public float m_pushForce;
	// Use this for initialization
	void Awake () {
		base.Awake ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	void pullTarget(Rigidbody target){
		Vector3 pullVector = (transform.position - target.transform.position) * m_pushForce;
		target.AddForceAtPosition (pullVector, target.transform.position, ForceMode.Impulse);
	}

	void OnTriggerStay(Collider other){
		if (other.tag.Equals ("Player"))
			pullTarget (other.gameObject.rigidbody);
	}
}
