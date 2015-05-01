using UnityEngine;
using System.Collections;

public class playerShoot : MonoBehaviour {
	public static playerShoot p_Shoot;

	public float m_damage;
	public float m_shootForce;
	public float m_pushForce;
	public float m_recoilForce;
	public float m_shootsPerSecond;
	public GameObject m_projectile;
 	
	Vector3 shootDirection;
	float m_nextShot;
	Camera m_camera;
	Rigidbody m_rigidbody;

	void Awake () {
		p_Shoot = this;
		m_rigidbody = rigidbody;
		m_camera = FindObjectOfType<Camera> ();
	}
	
	float ResetShootCooldown (){
		return 1 / m_shootsPerSecond;
	}


	bool ShootingDistanceIsGreaterThanZero (){
		shootDirection = this.GetShootDirection();
		return Mathf.Round (shootDirection.sqrMagnitude) != 0;
	}


	public void Shoot(){
		if (Time.time > m_nextShot && ShootingDistanceIsGreaterThanZero ()) {
			m_nextShot = Time.time + ResetShootCooldown ();
			ShootProjectile ();
		}
	}

	Vector3 GetShootDirection(){
		Vector3 mouseDir =  m_camera.ScreenToWorldPoint (Input.mousePosition);
		Vector3 direction = mouseDir - this.transform.position;
		direction.z = 0;
		return direction.normalized;
	}

	void ShootProjectile (){
		Vector3 projSpawnPos = this.transform.position + shootDirection;
		GameObject proj = (GameObject)Instantiate (m_projectile, projSpawnPos, Quaternion.identity);
		proj.GetComponent<projectileController>().SetTargetTag("Enemy");
		proj.GetComponent<projectileController> ().m_shooter = gameObject;
		proj.GetComponent<projectileController> ().m_damage = m_damage;
		proj.transform.TransformDirection (shootDirection);
		proj.GetComponent<Rigidbody> ().AddForce (shootDirection * m_shootForce, ForceMode.Impulse);

		//Recoil
		m_rigidbody.AddForce (-(shootDirection * m_recoilForce), ForceMode.Impulse);
		PlayerShake ();
	}


	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}


	void PlayerShake(){
		this.gameObject.GetComponent<CameraShake> ().enabled = true;
	}

}
