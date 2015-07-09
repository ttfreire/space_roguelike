using UnityEngine;
using System.Collections;

public class mineController : enemyBaseController {
	public float m_pullForce;
	public bool isEngaging = false;
	public float explosionDamage;
	float engagetime;
	Animator pulseAnim;
	AudioSource pulseSound;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		engagetime = 1;
		pulseAnim = transform.FindChild("Pulse").GetComponent<Animator>();
		pulseSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		UpdateState (m_currentState);
		if (anim != null) {
			anim.SetBool ("isEngaging", isEngaging);
			anim.SetBool ("isAttacking", isAttacking);
			anim.SetBool("isDead", isDead);
		}
		if(pulseAnim != null){
			pulseAnim.SetBool ("isAttacking", isAttacking);
			pulseAnim.SetBool("isDead", isDead);
		}
	}

	protected override void EnterState(EnemyState state){
		ExitState (m_currentState);
		m_currentState = state;
		
		switch (m_currentState) {
		case EnemyState.IDLE:
			isAttacking = false;
			isEngaging = false;
			break;
		case EnemyState.ENGAGING:
			isEngaging = true;
			break;
		case EnemyState.ATTACKING:
			isAttacking = true;
			isEngaging = false;
			break;
		case EnemyState.DEAD:
			isDead = true;
			//boomSource.Play();
			break;
		}
	}

	protected override void UpdateState(EnemyState state){

		switch (m_currentState) {
		case EnemyState.IDLE:
			
			if(m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ENGAGING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;

		case EnemyState.ENGAGING:
			engagetime -= Time.deltaTime;
			if(engagetime < 0){
				engagetime = 1;
				EnterState(EnemyState.ATTACKING);
			}
			break;
		case EnemyState.ATTACKING:
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			FollowTarget (player.transform.position, m_speed);
			if(m_shootController != null)
				m_shootController.Shoot();
			pulseSound.Play();
			break;
			
		case EnemyState.DEAD:
			base.UpdateState(m_currentState);
			break;
		}
	}

	void pullTarget(Rigidbody target){
		Vector3 pullVector = (transform.position - target.transform.position) * m_pullForce;
		target.AddForceAtPosition (pullVector, target.transform.position);
	}

	void SelfDestruct(){
		playerHealth.p_Health.TakeDamage (explosionDamage);
		EnterState (EnemyState.DEAD);
	}

	void OnTriggerStay(Collider other){
		if (other.tag.Equals ("Player"))
			pullTarget (other.gameObject.rigidbody);
	}

	void OnCollisionEnter(Collision other){
		if(other.transform.tag.Equals("Player"))
			SelfDestruct();
		//if (other.transform.tag.Equals ("Projectile"))
		//	Destroy (other.gameObject);
	}

	protected override void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Projectile"))
			Destroy (other.gameObject);
	}



}
