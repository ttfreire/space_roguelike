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
			transform.FindChild("FOV").collider.enabled = false;
			transform.collider.enabled = false;
			foreach(GameObject test in m_scavangedItems){
				test.transform.position = transform.position;
			}
			m_scavangedItems.Clear();

			break;
		}
	}
	protected  override void UpdateState(EnemyState state){
		base.UpdateState(state);
		switch (m_currentState) {
		case EnemyState.IDLE:
			break;
		case EnemyState.ATTACKING:
			break;

		case EnemyState.GETTINGITENS:
			if(base.m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ATTACKING);
			else if (m_seenItems != null)
				if(m_seenItems.Count == 0)
					EnterState(EnemyState.IDLE);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			ScavengeItems();
			break;
		case EnemyState.DEAD:

			break;
		}
	}

	void ScavengeItems(){
		if (m_seenItems.Count > 0) {
			GameObject target = m_seenItems[0];
			if(target != null){
				FollowTarget(target.transform.position, m_scavengeSpeed);
				float distance = (transform.position - target.transform.position).magnitude;
				if(distance < 1){
					if(!m_scavangedItems.Contains(target)){
						m_scavangedItems.Add(target);
						m_seenItems.Remove(target);
						target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -50);
					}
				}
			}
		}
	}
	
	protected void DropItems(){
		Instantiate(ItemsDrop[Random.Range(0, ItemsDrop.Count)], this.transform.position, this.transform.rotation);
	}


	void OnTriggerExit(Collider col){
		if (collider.tag.Equals ("Item"))
			if(m_seenItems.Contains(col.gameObject))
				m_seenItems.Remove (col.gameObject);
	}

	void OnTriggerStay(Collider col){
		if (col.tag.Equals ("Item")) {
			//if (!m_seenItems.Contains (col.gameObject))
			m_seenItems.Clear();
			m_seenItems.Add (col.gameObject);
		}
		if (!m_sightController.m_isPlayerOnView && canScavenge)
			EnterState (EnemyState.GETTINGITENS);
	}
}