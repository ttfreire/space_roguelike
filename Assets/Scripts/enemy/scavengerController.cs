using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scavengerController : enemyBaseController {
	public float m_scavengeSpeed;
	List<GameObject> m_seenItems = new List <GameObject> ();
	List<GameObject> m_scavangedItems = new List <GameObject> ();
	
	public bool canScavenge;
	// Use this for initialization


	protected override void Awake () {
		base.Awake ();
		m_seenItems.Clear ();
		m_scavangedItems.Clear ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING))
			UpdateState (m_currentState);
	}

	protected override void EnterState(EnemyState state){
		ExitState (m_currentState);
		m_currentState = state;
		
		switch (m_currentState) {
		case EnemyState.IDLE:
			isMoving = false;
			break;
		case EnemyState.ENGAGING:
			engageTime = 1f;
			detectSource.Play();
			break;
		case EnemyState.ATTACKING:
			isMoving = true;
			break;
		case EnemyState.MOVING:
			isMoving = true;
			break;
		case EnemyState.DEAD:
			transform.FindChild("FOV").collider.enabled = false;
			transform.collider.enabled = false;
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
			engageTime -= Time.deltaTime;
			if(engageTime < 0f)
				EnterState(EnemyState.MOVING);
			break;
		case EnemyState.ATTACKING:
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			if(isMoving)
				EnterState(EnemyState.MOVING);
			break;
		case EnemyState.MOVING:
			FollowTarget (player.transform.position, m_speed);
			if(isAttacking)
				EnterState(EnemyState.ATTACKING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;
		case EnemyState.DEAD:
			base.UpdateState(m_currentState);
			break;
		}
	}


	
	protected void DropItems(){
		Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count)], this.transform.position, this.transform.rotation);
	}

	protected override void OnCollisionStay(Collision other){
		base.OnCollisionStay (other);
		if (other.gameObject.Equals (player))
			isAttacking = true;
	}
	
	
	protected override void OnCollisionExit(Collision other){
		base.OnCollisionExit (other);
		if (other.gameObject.Equals (player))
			isMoving = true;
	}

	/**
	void OnTriggerExit(Collider col){
		if (collider.tag.Equals ("Item"))
			if(m_seenItems.Contains(col.gameObject))
				m_seenItems.Remove (col.gameObject);
	}
	**/
	/**
	void OnTriggerStay(Collider col){
		if (col.tag.Equals ("Item")) {
			//if (!m_seenItems.Contains (col.gameObject))
			m_seenItems.Clear();
			m_seenItems.Add (col.gameObject);
		}
		if (!m_sightController.m_isPlayerOnView && canScavenge)
			EnterState (EnemyState.GETTINGITENS);
	}
	**/
}