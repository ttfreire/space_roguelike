using UnityEngine;
using System.Collections;


public class MoveableController : MonoBehaviour {
	float m_force;
	// Use this for initialization
	void Start () {
		m_force = 10;
	
	}
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag.Equals("Projectile")){
			Vector3 vectorForce = (other.gameObject.transform.position - this.transform.position )* m_force;
			this.gameObject.rigidbody.AddForceAtPosition(vectorForce, this.gameObject.transform.position);
			Destroy(other.gameObject);
		}
	}
}
