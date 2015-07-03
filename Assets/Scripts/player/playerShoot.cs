using UnityEngine;
using System.Collections;

public class playerShoot : MonoBehaviour {
	public enum ProjectileType {NORMAL, DAMAGE, VELOCITY, AREA, PIERCING};

	public static playerShoot p_Shoot;

	public float m_damage;
	public float m_shootForce;
	public float m_pushForce;
	public float m_recoilForce;
	public float m_shootsPerSecond;
	public GameObject m_projectile;
	Sprite projectileSprite;
	public Sprite projectileSpriteNormal;
	public Sprite projectileSpriteDamage;
	public Sprite projectileSpriteVelocity;
	public Sprite projectileSpriteArea;
	public Sprite projectileSpritePiercing;
	//public AudioClip shootSound;
	AudioSource source;
	Vector3 shootDirection;
	float m_nextShot;
	public Camera m_camera;
	Rigidbody m_rigidbody;
	public ProjectileType currentAmmoType;
	void Awake () {
		p_Shoot = this;
		m_rigidbody = rigidbody;
		source = GetComponent <AudioSource>();
		projectileSprite = projectileSpriteNormal;
		currentAmmoType = ProjectileType.NORMAL;
	}

	void Update(){
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING)) {
			Vector3 mouseDir = m_camera.ScreenToWorldPoint (Input.mousePosition);
			playerMovement.p_Movement.SetFacingDirection (mouseDir);
		}
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
			//playerMovement.p_Movement.SetFacingDirection((shootDirection-this.transform.position).x);
			source.Play();
			ShootProjectile (shootDirection, currentAmmoType);
			if(currentAmmoType.Equals(ProjectileType.AREA)){
				Vector3 v = shootDirection;
				Vector3 v2 = Quaternion.AngleAxis(45, Vector3.forward) * v;
				Vector3 v3 = Quaternion.AngleAxis(-45, Vector3.forward) * v;

				ShootProjectile ((shootDirection+v2).normalized, currentAmmoType);
				ShootProjectile ((shootDirection+v3).normalized, currentAmmoType);
			}
		}
	}

	Vector3 GetShootDirection(){
		Vector3 mouseDir =  m_camera.ScreenToWorldPoint (Input.mousePosition);
		Vector3 direction = mouseDir - this.transform.position;
		direction.z = 0;
		return direction.normalized;
	}

	void ShootProjectile (Vector3 directionToShoot, ProjectileType type){
		float projDamage = m_damage;
		float projectileSpriteSpeed = m_shootForce;
		switch (type) {
		case ProjectileType.NORMAL:
			projDamage = m_damage;
			projectileSprite = projectileSpriteNormal;
			projectileSpriteSpeed = m_shootForce;
			break;
		case ProjectileType.DAMAGE:
			projDamage = m_damage+30;
			projectileSprite = projectileSpriteDamage;
			projectileSpriteSpeed = m_shootForce;
			break;
		case ProjectileType.VELOCITY:
			projDamage = m_damage;
			projectileSprite = projectileSpriteVelocity;
			projectileSpriteSpeed = m_shootForce+20;
			break;
		case ProjectileType.AREA:
			projDamage = m_damage;
			projectileSprite = projectileSpriteArea;
			projectileSpriteSpeed = m_shootForce;
			break;
		case ProjectileType.PIERCING:
			projDamage = m_damage;
			projectileSprite = projectileSpritePiercing;
			projectileSpriteSpeed = m_shootForce+10;
			break;
		}


		Vector3 projSpawnPos =  transform.position + directionToShoot;
		GameObject proj = (GameObject)Instantiate (m_projectile, projSpawnPos, Quaternion.identity);
		proj.layer = 14;
		proj.GetComponent<projectileController>().SetTargetTag("Enemy");
		proj.GetComponent<projectileController> ().m_shooter = gameObject;
		proj.GetComponent<projectileController> ().m_damage = projDamage;
		proj.GetComponent<SpriteRenderer> ().sprite = projectileSprite;
		if(currentAmmoType.Equals(ProjectileType.PIERCING))
		   proj.GetComponent<projectileController>().isPiercing = true;
		proj.transform.TransformDirection (directionToShoot);
		AimAtTarget (proj, directionToShoot);
		proj.GetComponent<Rigidbody> ().AddForce (directionToShoot * projectileSpriteSpeed, ForceMode.Impulse);

		//Recoil
		m_rigidbody.AddForce (-(shootDirection * m_recoilForce), ForceMode.Impulse);
		//PlayerShake ();
	}


	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}


	void PlayerShake(){
		this.gameObject.GetComponent<CameraShake> ().enabled = true;
	}

	void AimAtTarget(GameObject objToRotate, Vector3 targetDirection){
		float zRotation = Mathf.Atan2( targetDirection.y, targetDirection.x )*Mathf.Rad2Deg;
		objToRotate.transform.rotation = Quaternion.Euler(new Vector3 ( 0, 0, zRotation+180));
	}
}
