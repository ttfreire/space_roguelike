using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	public float m_repelForce;
	bool shake = false;
	Camera m_camera;
	Transform m_targetToShoot;
	GameObject player;
	public float m_speed;
	bool canFollowPlayer = false;
	// Use this for initialization
	void Start () {

		m_camera = FindObjectOfType<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");

	}
	
	// Update is called once per frame
	void Update () {
		if(shake)
			CameraShake ();
		if(renderer.isVisible)
			FollowPlayer ();
	}



	void OnCollisionStay(Collision other){
		if (other.gameObject.Equals (player)) {
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

	void FollowPlayer(){
		Transform target = player.transform;
		Vector3 direction = target.position - this.transform.position;
		transform.Translate (direction * Time.deltaTime * m_speed);
	}
}
