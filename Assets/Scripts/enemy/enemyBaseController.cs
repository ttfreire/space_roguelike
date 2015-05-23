using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyBaseController : MonoBehaviour {
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
	
	protected virtual void FollowPlayer(){
		Transform target = player.transform;
		Vector3 direction = (target.position - this.transform.position).normalized;
		direction.z = 0f;
		transform.Translate (direction * Time.deltaTime * m_speed, Space.World);
		
	}

	void DropItems(){
		Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count-1)], this.transform.position, this.transform.rotation);
	}
}
