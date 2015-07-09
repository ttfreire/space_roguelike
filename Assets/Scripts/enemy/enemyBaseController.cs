using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyBaseController : MonoBehaviour {
	public enum EnemyState {IDLE, ENGAGING, ATTACKING, MOVING, RECHARGING, DEAD};

	public EnemyState m_currentState = EnemyState.IDLE;
	
	public float m_repelForce;
	protected bool shake = false;
	protected Camera m_camera;

	protected Transform m_targetToShoot;
	protected GameObject player;
	public float m_speed;
	public GameObject explosionObject;
	
	protected enemyHealth m_healthController;
	protected enemySight m_sightController;
	protected enemyShoot m_shootController;
	
	
	
	public List<GameObject> ItemsDrop;

	protected float engageTime = 0.5f;
	
	//Animation
	protected Animator anim;
	protected bool isMoving = false;
	protected bool isAttacking = false;
	protected bool isFacingRight = true;
	protected bool isDead = false;
	protected float explosionTime = 1;
	protected float timeToAttack;
	protected float attackTime;
	AudioSource[] audios;
	AudioSource hitSource;
	protected AudioSource detectSource;
	// Use this for initialization

	protected virtual void Awake () {
		m_healthController = GetComponent<enemyHealth> ();
		m_sightController = GetComponent<enemySight> ();
		m_shootController = GetComponent<enemyShoot> ();

		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator>();
		audios = GetComponents<AudioSource> ();
		detectSource = audios [0];
		hitSource = audios [1];
	}

	void Start(){
		m_camera = playerController.p_controller.p_camera;
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
				anim.SetBool ("isPlayerOnSight", m_sightController.m_isPlayerOnView);
				anim.SetFloat ("health", m_healthController.m_health);
			}
		}
	}
	
	protected virtual void EnterState(EnemyState state){
		ExitState (m_currentState);
		m_currentState = state;
		
		switch (m_currentState) {
		case EnemyState.IDLE:

			break;
		case EnemyState.ENGAGING:
			engageTime = 1f;
			detectSource.Play();
			break;
		case EnemyState.ATTACKING:
			isAttacking = true;
			attackTime = anim.GetCurrentAnimatorStateInfo(0).length;
			break;
		case EnemyState.MOVING:
			isMoving = true;
			timeToAttack = 2f;
			break;
		case EnemyState.DEAD:
			isDead = true;
			//boomSource.Play();
			break;
		}
	}
	
	protected virtual void ExitState(EnemyState state){
		switch (m_currentState) {
		case EnemyState.IDLE:
			
			break;
		case EnemyState.ENGAGING:

			break;
		case EnemyState.ATTACKING:
			isAttacking = false;

			break;
		case EnemyState.MOVING:
			isMoving = false;

			break;
		case EnemyState.DEAD:
			break;
		}
	}
	
	protected virtual void UpdateState(EnemyState state){
		switch (m_currentState) {
		case EnemyState.IDLE:
			
			if(m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ENGAGING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;
		case EnemyState.ENGAGING:
			engageTime -= Time.deltaTime;
				if(engageTime < 0f)
					EnterState(EnemyState.MOVING);
			break;
		case EnemyState.ATTACKING:
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			FollowTarget (player.transform.position, m_speed);
			m_shootController.Shoot();
			attackTime -= Time.deltaTime;
			if(attackTime < 0f)
				EnterState(EnemyState.MOVING);
			break;
		case EnemyState.MOVING:
			FollowTarget (player.transform.position, m_speed);
			timeToAttack -= Time.deltaTime;
			if(timeToAttack < 0f)
				EnterState(EnemyState.ATTACKING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;
		case EnemyState.DEAD:
			explosionTime -= Time.deltaTime;
			//if(explosionTime < 0){
				DropItems();
				Instantiate(explosionObject, this.transform.position, this.transform.rotation);
				Destroy(gameObject);
			//}
			break;
		}
	}


	protected virtual void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Projectile")) {
			projectileController proj = other.gameObject.GetComponent<projectileController> ();
			if (proj.m_shooter != this.gameObject)
				if (proj.m_shooter.tag.Equals("Player")){
					m_healthController.TakeDamage (proj.m_damage);
					Vector3 shootForce = player.GetComponent<playerShoot> ().m_pushForce * (this.transform.position-proj.m_shooter.transform.position);
					this.rigidbody.AddForceAtPosition (shootForce, this.transform.position);
					if(!proj.isPiercing)
						Destroy(other.gameObject);
				}
		}
	}

	protected virtual void OnCollisionStay(Collision other){
		if (other.gameObject.Equals (player)) {
			RepelPlayer (other.rigidbody);
			shake = true;
			//isAttacking = true;
		}
		
	}
	
	
	protected virtual void OnCollisionExit(Collision other){
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
			Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count)], this.transform.position, Quaternion.identity);
	}
	
}
