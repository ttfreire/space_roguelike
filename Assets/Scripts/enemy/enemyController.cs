using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	public float m_repelForce;
	bool shake = false;
	Camera m_camera;
	Transform m_targetToShoot;
	// Use this for initialization
	void Start () {

		m_camera = FindObjectOfType<Camera> ();

	}
	
	// Update is called once per frame
	void Update () {
		if(shake)
			CameraShake ();
	}



	void OnCollisionStay(Collision other){
		if (other.gameObject.tag.Equals ("Player")) {
			RepelPlayer (other.rigidbody);
			shake = true;
		}
	}

	void OnCollisionExit(Collision other){
		shake = false;
		//m_camera.GetComponent<CameraShake> ().enabled = false;
	}

	
	void RepelPlayer(Rigidbody player){
		Vector3 repelVector = -(transform.position - player.transform.position) * m_repelForce;
		player.AddForceAtPosition (repelVector, player.transform.position, ForceMode.Impulse);
	}

	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}
}
