﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turretController : enemyBaseController {
	

	protected virtual void Awake () {
		base.Awake ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING)) {
			if (shake)
				CameraShake ();
			UpdateState (m_currentState);
			if (anim != null) {
				anim.SetBool ("isMoving", isMoving);
				anim.SetBool ("isAttacking", isAttacking);
			}
		}
	}
	
	public void EnterState(EnemyState state){
		ExitState (m_currentState);
		m_currentState = state;
		
		switch (m_currentState) {
		case EnemyState.IDLE:
			isMoving = false;
			break;
		case EnemyState.ATTACKING:
			isMoving = true;
			break;
		case EnemyState.GETTINGITENS:
			break;
		case EnemyState.DEAD:
			
			break;
		}
	}
	
	public void ExitState(EnemyState state){
		switch (m_currentState) {
		case EnemyState.IDLE:
			
			break;
		case EnemyState.ATTACKING:

			break;
		case EnemyState.GETTINGITENS:
			
			break;
		case EnemyState.DEAD:
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
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			AimAtPlayer();
			break;
			
		case EnemyState.DEAD:
			DropItems();
			Destroy (gameObject);
			break;
		}
	}
	
	protected virtual void OnCollisionEnter(Collision other){
		
		
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
	
	void OnCollisionStay(Collision other){
		if (other.gameObject.Equals (player)) {
			RepelPlayer (other.rigidbody);
			shake = true;
			isAttacking = true;
		}
		
	}
	
	
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
		Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count-1)], this.transform.position, this.transform.rotation);
	}
}
