using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turretController : MonoBehaviour {

	public enum EnemyState {IDLE, ATTACKING, GETTINGITENS, DEAD};
	
	public EnemyState m_currentState = EnemyState.IDLE;
	
	public float m_repelForce;
	bool shake = false;
	Camera m_camera;
	
	Transform m_targetToShoot;
	protected GameObject player;
	public float m_speed;
	
	
	protected enemyHealth m_healthController;
	protected enemySight m_sightController;
	
	
	
	public List<GameObject> ItemsDrop;
	
	
	//Animation
	Animator anim;
	bool isMoving = false;
	bool isAttacking = false;
	protected bool isFacingRight = true;
	// Use this for initialization
	
	protected virtual void Awake () {
		m_healthController = GetComponent<enemyHealth> ();
		m_sightController = GetComponent<enemySight> ();
		m_camera = FindObjectOfType<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(shake)
			CameraShake ();
		
		//if(transform.parent != null){
		//Vector3 localPos = new Vector3(transform.position.x, transform.position.y, transform.parent.position.z) + transform.localPosition;
		//transform.position = localPos;
		//}
		UpdateState (m_currentState);
		if (anim != null) {
			anim.SetBool ("isMoving", isMoving);
			anim.SetBool ("isAttacking", isAttacking);
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
			FollowPlayer ();
			AimAtPlayer();
			break;
			
		case EnemyState.DEAD:
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
	
	protected virtual void FollowPlayer(){
		Transform target = player.transform;
		Vector3 direction = (target.position - this.transform.position).normalized;
		direction.z = 0f;
		transform.Translate (direction * Time.deltaTime * m_speed, Space.World);
		
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
}
