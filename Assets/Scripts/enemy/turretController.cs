using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turretController : enemyBaseController {
	float rechargeTime;
	float secondsToRecharge = 2;
	float secondstoShoot = 2;
	float shootTime;
	Animator deathAnimator;
	Animator baseAnimator;
	Animator cannonAnimator;

	protected override void Awake () {
		base.Awake ();
		rechargeTime = 0;
		shootTime = Time.deltaTime * 60 * secondstoShoot;
		baseAnimator = transform.FindChild("base").GetComponent<Animator>();
		cannonAnimator = transform.FindChild("cannon").FindChild("Cannon_base").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING)) {
			if (shake)
				CameraShake ();
			UpdateState (m_currentState);
			if (baseAnimator != null) {
				baseAnimator.SetBool ("isAttacking", isAttacking);
				baseAnimator.SetFloat("rechargeTime", rechargeTime);
				baseAnimator.SetFloat("health", m_healthController.m_health);
			}
			if(cannonAnimator != null){
				cannonAnimator.SetBool ("isAttacking", isAttacking);
				cannonAnimator.SetFloat("health", m_healthController.m_health);
			}

		}
	}
	
	protected override void EnterState(EnemyState state){
		ExitState (m_currentState);
		m_currentState = state;
		
		switch (m_currentState) {
		case EnemyState.IDLE:
			isAttacking = false;
			break;
		case EnemyState.ATTACKING:
			isAttacking = true;
			shootTime = Time.deltaTime * 60 * secondstoShoot;

			break;
		case EnemyState.MOVING:
			break;
		case EnemyState.RECHARGING:
			rechargeTime = Time.deltaTime * 60 * secondsToRecharge;
			break;
		case EnemyState.DEAD:
			isDead = true;
			//boomSource.Play();
			break;
		}
	}

	protected virtual void UpdateState(EnemyState state){
		switch (m_currentState) {
		case EnemyState.IDLE:
			AimAtPlayer();
			if(m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ATTACKING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;
		case EnemyState.ATTACKING:
			shootTime -= Time.deltaTime;
			if(shootTime < 0)
				EnterState(EnemyState.RECHARGING);
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			AimAtPlayer();
			if(m_shootController != null)
				m_shootController.Shoot();
			break;

		case EnemyState.RECHARGING:
			rechargeTime -= Time.deltaTime;
			if(rechargeTime < 0)
				EnterState(EnemyState.ATTACKING);
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;
			
		case EnemyState.DEAD:
			explosionTime -= Time.deltaTime;
			if(explosionTime < 0){
				DropItems();
				Instantiate(explosionObject, this.transform.position, this.transform.rotation);
				Destroy(gameObject);
			}
			break;
		}
	}
	/**
	protected override void OnCollisionEnter(Collision other){
		if (other.gameObject.tag.Equals ("Projectile")) {
			projectileController proj = other.gameObject.GetComponent<projectileController> ();
			if (!other.contacts [0].thisCollider.tag.Equals ("Undamagable")) {
				if (proj.m_shooter != this.gameObject) {
					Vector3 dirFromProjectile = (this.transform.position - other.gameObject.transform.position);
					Vector3 shootForce = player.GetComponent<playerShoot> ().m_pushForce * dirFromProjectile;
					this.rigidbody.AddForceAtPosition (shootForce, this.transform.position, ForceMode.Impulse);
					Destroy (other.gameObject);
				}
			} else
				Destroy (other.gameObject);
			if (proj.m_shooter != this.gameObject)
				if (proj.m_shooter.tag.Equals("Player"))
					m_healthController.TakeDamage (proj.m_damage);
		}
		
	}
	**/
	
	void OnCollisionStay(Collision other){
		if (other.gameObject.Equals (player)) {
			RepelPlayer (other.rigidbody);
			shake = true;
			isAttacking = true;
		}
		
	}
/**
	void OnTriggerEnter(Collider other){
		if(other.tag.Equals("Projectile"))
			if (other.gameObject.GetComponent<projectileController>().m_shooter != this.gameObject)
				if (other.gameObject.GetComponent<projectileController>().m_shooter.tag.Equals("Player"))
					m_healthController.TakeDamage (other.gameObject.GetComponent<projectileController>().m_damage);
	}
**/
	
	void OnCollisionExit(Collision other){
		shake = false;
		isAttacking = false;
		//m_camera.GetComponent<CameraShake> ().enabled = false;
	}
	
	
	void RepelPlayer(Rigidbody player){
		Vector3 repelVector = -(transform.position - player.transform.position) * m_repelForce;
		player.AddForceAtPosition (repelVector, player.transform.position, ForceMode.Impulse);
	}
	
	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}

	void AimAtPlayer(){
		GameObject turretCannon = gameObject.transform.FindChild ("cannon").gameObject;
		Vector3 toTargetVector = turretCannon.transform.position - player.transform.position;
		float zRotation = Mathf.Atan2( toTargetVector.y, toTargetVector.x )*Mathf.Rad2Deg;
		turretCannon.transform.rotation = Quaternion.Euler(new Vector3 ( 0, 0, zRotation));
	}

	void ReturnToOriginalAimPosition(){
		GameObject turretCannon = gameObject.transform.FindChild ("cannon").gameObject;
		float step = 2 * Time.deltaTime;
		turretCannon.transform.rotation = Quaternion.RotateTowards(turretCannon.transform.rotation, Quaternion.identity, step);
	}

	void DropItems(){
		Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count)], this.transform.position, this.transform.rotation);
	}
}
