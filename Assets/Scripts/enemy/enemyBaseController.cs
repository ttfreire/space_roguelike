using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyBaseController : MonoBehaviour {
	public enum EnemyState {IDLE, ENGAGING, ATTACKING, GETTINGITENS, RECHARGING, DEAD};

	public EnemyState m_currentState = EnemyState.IDLE;
	
	public float m_repelForce;
	protected bool shake = false;
	protected Camera m_camera;

	protected Transform m_targetToShoot;
	protected GameObject player;
	public float m_speed;
	
	
	protected enemyHealth m_healthController;
	protected enemySight m_sightController;
	protected enemyShoot m_shootController;
	
	
	
	public List<GameObject> ItemsDrop;
	public AudioSource boomSource;
	
	//Animation
	protected Animator anim;
	protected bool isMoving = false;
	protected bool isAttacking = false;
	protected bool isFacingRight = true;
	protected bool isDead = false;
	protected float explosionTime = 2;

	// Use this for initialization

	protected virtual void Awake () {
		m_healthController = GetComponent<enemyHealth> ();
		m_sightController = GetComponent<enemySight> ();
		m_shootController = GetComponent<enemyShoot> ();
		m_camera = FindObjectOfType<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING)) {
			if (shake && !isDead)
				CameraShake ();
			UpdateState (m_currentState);
			if (anim != null) {
				anim.SetBool ("isMoving", isMoving);
				anim.SetBool ("isAttacking", isAttacking);
				anim.SetFloat ("health", m_healthController.m_health);
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
			isMoving = true;
			break;
		case EnemyState.DEAD:
			boomSource.Play();
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
			FollowTarget (player.transform.position, m_speed);
			if(m_shootController != null)
				m_shootController.Shoot();
			break;
			
		case EnemyState.DEAD:
			explosionTime -= Time.deltaTime;
			if(explosionTime < 0){
				DropItems();
				Destroy(gameObject);
			}
			break;
		}
	}
	
	protected virtual void OnCollisionEnter(Collision other){
		if (other.gameObject.tag.Equals ("Projectile")) {
			projectileController proj = other.gameObject.GetComponent<projectileController> ();
				if (proj.m_shooter != this.gameObject) {
					Vector3 dirFromProjectile = (this.transform.position - other.gameObject.transform.position);
					Vector3 shootForce = player.GetComponent<playerShoot> ().m_pushForce * dirFromProjectile;
					//this.rigidbody.AddForceAtPosition (shootForce, this.transform.position, ForceMode.Impulse);
					Destroy (other.gameObject);
				}
			if (proj.m_shooter != this.gameObject)
				if (proj.m_shooter.tag.Equals("Player"))
					m_healthController.TakeDamage (proj.m_damage);
		}
	}
	
	void OnCollisionStay(Collision other){
		if (other.gameObject.Equals (player)) {
			RepelPlayer (other.rigidbody);
			shake = true;
			//isAttacking = true;
		}
		
	}
	
	
	void OnCollisionExit(Collision other){
		shake = false;
		//isAttacking = false;
		//m_camera.GetComponent<CameraShake> ().enabled = false;
	}
	

	void RepelPlayer(Rigidbody player){
		Vector3 repelVector = -(transform.position - player.transform.position) * m_repelForce;
		player.AddForceAtPosition (repelVector, player.transform.position, ForceMode.Impulse);
	}
	
	void CameraShake(){
		if(!isDead)
			m_camera.GetComponent<CameraShake> ().enabled = true;
	}
	
	protected virtual void FollowTarget(Vector3 targetPos, float speed){
		Vector3 direction = (targetPos - this.transform.position).normalized;
		direction.z = 0f;
		transform.Translate (direction * Time.deltaTime * speed, Space.World);	
		if (transform.position.x < targetPos.x && isFacingRight) {
			transform.LookAt (transform.position - transform.forward);
			isFacingRight = false;
		} else if (transform.position.x > targetPos.x && !isFacingRight) {
			transform.LookAt (transform.position - transform.forward);
			isFacingRight = true;
		}
	}

	protected void DropItems(){
		if(ItemsDrop.Count > 0)
			Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count)], this.transform.position, this.transform.rotation);
	}
}
