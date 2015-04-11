using UnityEngine;
using System.Collections;

public class enemyShoot : MonoBehaviour {
	Transform m_targetToShoot;
	public GameObject m_projectile;
	public float m_shootForce;
	public float m_shootsPerSecond;
	GameObject m_player;
	enemySight m_sight;
	float m_shootCooldown;


	void Awake () {
		m_shootCooldown = ResetShootCooldown();
		m_sight = GetComponent<enemySight> ();
		m_player = FindObjectOfType<playerController> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		m_shootCooldown -= Time.deltaTime;
		if (m_shootCooldown <= 0.0f && m_sight.m_isPlayerOnView) {
			m_shootCooldown = ResetShootCooldown();
			m_targetToShoot = m_player.transform;
			ShootTarget (m_targetToShoot);
		}
	}

	float ResetShootCooldown ()
	{
		return 1 / m_shootsPerSecond;
	}

	void ShootTarget(Transform target){
		Vector3 shootDir = (target.position - transform.position).normalized;
		GameObject proj = (GameObject)Instantiate (m_projectile, this.transform.position + shootDir , Quaternion.identity);
		proj.GetComponent<projectileController>().SetTargetTag("Player");
		proj.GetComponent<projectileController> ().m_shooter = this.gameObject;

		proj.transform.TransformDirection (shootDir);
		proj.GetComponent<Rigidbody> ().AddForce (shootDir * m_shootForce, ForceMode.Impulse);
	}
}
