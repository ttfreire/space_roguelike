using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	public GameObject m_projectile;
	public float m_shootForce;
	public float m_shootsPerSecond;
	float m_shootCooldown;
	Transform m_targetToShoot;
	bool m_isPlayerOnView = false;
	public float m_repelForce;
	bool shake = false;
	Camera m_camera;

	// Use this for initialization
	void Start () {
		m_shootCooldown = 1 / m_shootsPerSecond;
		m_camera = FindObjectOfType<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_shootCooldown -= Time.deltaTime;
		if (m_shootCooldown <= 0.0f && m_isPlayerOnView) {
			m_shootCooldown = 1 / m_shootsPerSecond;
			ShootTarget (m_targetToShoot.transform);
		}
		if(shake)
			CameraShake ();
	}

	void OnTriggerStay(Collider other){
		if (other.tag.Equals ("Player")) {
			m_isPlayerOnView = true;
			m_targetToShoot = other.transform;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag.Equals ("Player"))
			m_isPlayerOnView = false;
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

	void ShootTarget(Transform target){
		GameObject proj = (GameObject)Instantiate (m_projectile, this.transform.position, Quaternion.identity);
		Vector3 shootDir = (target.position - transform.position).normalized;
		proj.transform.TransformDirection (shootDir);
		proj.GetComponent<Rigidbody> ().AddForce (shootDir * m_shootForce, ForceMode.Impulse);
	}

	void RepelPlayer(Rigidbody player){
		Vector3 repelVector = -(transform.position - player.transform.position) * m_repelForce;
		player.AddForceAtPosition (repelVector, player.transform.position, ForceMode.Impulse);

	}

	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}
}
