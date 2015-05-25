using UnityEngine;
using System.Collections;

public class enemyShoot : MonoBehaviour {
	Transform m_targetToShoot;
	public GameObject m_projectile;
	public Sprite projectileSprite;
	public float m_shootForce;
	public float m_shootsPerSecond;
	public GameObject shooterObject;
	GameObject m_player;
	enemySight m_sight;
	enemyHealth m_health;
	float m_shootCooldown;
	public float m_damage;


	void Awake () {
		m_shootCooldown = ResetShootCooldown();
		m_sight = GetComponent<enemySight> ();
		m_health = GetComponent<enemyHealth> ();
		m_player = FindObjectOfType<playerController> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {

	}

	float ResetShootCooldown ()
	{
		return 1 / m_shootsPerSecond;
	}

	void ShootTarget(Transform target){

		Transform shotRootPos = shooterObject.transform;
		Vector3 shootDir = (target.position - shotRootPos.position).normalized;
		GameObject proj = (GameObject)Instantiate (m_projectile, shotRootPos.position + shootDir , Quaternion.identity);
		proj.GetComponent<projectileController>().SetTargetTag("Player");
		proj.GetComponent<projectileController> ().m_shooter = this.gameObject;
		proj.GetComponent<projectileController> ().m_damage = m_damage;
		proj.GetComponent<SpriteRenderer> ().sprite = projectileSprite;
		proj.transform.TransformDirection (shootDir);
		AimAtTarget (proj, shootDir);
	proj.GetComponent<Rigidbody> ().AddForce (shootDir * m_shootForce, ForceMode.Impulse);
	}

	void AimAtTarget(GameObject objToRotate, Vector3 targetDirection){
		float zRotation = Mathf.Atan2( targetDirection.y, targetDirection.x )*Mathf.Rad2Deg;
		objToRotate.transform.rotation = Quaternion.Euler(new Vector3 ( 0, 0, zRotation+180));
	}

	public void Shoot(){
		m_shootCooldown -= Time.deltaTime;
		if (m_shootCooldown <= 0.0f && m_sight.m_isPlayerOnView) {
			m_shootCooldown = ResetShootCooldown();
			m_targetToShoot = m_player.transform;
			ShootTarget (m_targetToShoot);
		}
	}
}
