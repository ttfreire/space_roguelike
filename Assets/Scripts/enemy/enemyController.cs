using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class enemyController : MonoBehaviour {
	public enum EnemyState {ROAMING, ATTACKING, GETTINGITENS, DEAD};
	public EnemyState m_currentState = EnemyState.ROAMING;

	public float m_repelForce;
	bool shake = false;
	Camera m_camera;

	Transform m_targetToShoot;
	GameObject player;
	public float m_speed;
	public float m_scavengeSpeed;
	
	enemyHealth m_healthController;
	enemySight m_sightController;

	List<GameObject> m_seenItems = new List <GameObject> ();
	List<GameObject> m_scavangedItems = new List <GameObject> ();

	public bool canScavenge;

	// Use this for initialization
	void Awake () {
		m_healthController = GetComponent<enemyHealth> ();
		m_sightController = GetComponent<enemySight> ();
		m_camera = FindObjectOfType<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		m_seenItems.Clear ();
		m_scavangedItems.Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		if(shake)
			CameraShake ();
	
		//if(transform.parent != null){
		//Vector3 localPos = new Vector3(transform.position.x, transform.position.y, transform.parent.position.z) + transform.localPosition;
		//transform.position = localPos;
		//}
		UpdateState (m_currentState);

	}

	public void EnterState(EnemyState state){
		ExitState (m_currentState);
		m_currentState = state;

		switch (m_currentState) {
		case EnemyState.ROAMING:
			break;
		case EnemyState.ATTACKING:

			break;
		case EnemyState.GETTINGITENS:
			break;
		case EnemyState.DEAD:

			break;
		}
	}
	
	public void ExitState(EnemyState state){
		switch (m_currentState) {
		case EnemyState.ROAMING:

			break;
		case EnemyState.ATTACKING:

			break;
		case EnemyState.GETTINGITENS:

			break;
		case EnemyState.DEAD:
			break;
		}
	}
	
	public void UpdateState(EnemyState state){
		switch (m_currentState) {
		case EnemyState.ROAMING:
			if(m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ATTACKING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			break;
		case EnemyState.ATTACKING:
			if(!m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ROAMING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			FollowPlayer ();
			break;
		case EnemyState.GETTINGITENS:
			if(m_sightController.m_isPlayerOnView)
				EnterState(EnemyState.ATTACKING);
			else if (m_seenItems != null)
				if(m_seenItems.Count == 0)
					EnterState(EnemyState.ROAMING);
			if (m_healthController.IsDead())
				EnterState(EnemyState.DEAD);
			ScavengeItems();
			break;
		case EnemyState.DEAD:
			transform.FindChild("FOV").collider.enabled = false;
			transform.collider.enabled = false;
			foreach(GameObject test in m_scavangedItems){
				test.transform.position = transform.position;
			}
			m_scavangedItems.Clear();
			Destroy (gameObject);
			break;
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag.Equals ("Projectile")) {
			if (!other.contacts [0].thisCollider.tag.Equals ("Undamagable")) {
				projectileController proj = other.gameObject.GetComponent<projectileController> ();
				if (proj.m_shooter != this.gameObject) {
					if(!canScavenge && proj.m_shooter.tag.Equals("Player"))
						m_healthController.TakeDamage (proj.m_damage);
					Vector3 dirFromProjectile = (this.transform.position - other.gameObject.transform.position);
					Vector3 shootForce = player.GetComponent<playerShoot> ().m_pushForce * dirFromProjectile;
					this.rigidbody.AddForceAtPosition (shootForce, this.transform.position, ForceMode.Impulse);
					Destroy (other.gameObject);
				}
			} else
				Destroy (other.gameObject);
		} else
			if (!other.gameObject.tag.Equals ("Player")) {
			float collisionVelocity = other.relativeVelocity.magnitude;
			if(collisionVelocity > 0.5f){
				float damage = rigidbody.velocity.magnitude * m_healthController.m_collisionDamageMultiplier * 100;
				m_healthController.TakeDamage (damage);
			}
		}
	}

	void OnCollisionStay(Collision other){
		if (other.gameObject.Equals (player)) {
			RepelPlayer (other.rigidbody);
			shake = true;
		}

	}
	

	void OnCollisionExit(Collision other){
		shake = false;
		//m_camera.GetComponent<CameraShake> ().enabled = false;
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

	void OnTriggerExit(Collider col){
		if (collider.tag.Equals ("Item"))
			if(m_seenItems.Contains(col.gameObject))
				m_seenItems.Remove (col.gameObject);
	}
	
	void RepelPlayer(Rigidbody player){
		Vector3 repelVector = -(transform.position - player.transform.position) * m_repelForce;
		player.AddForceAtPosition (repelVector, player.transform.position, ForceMode.Impulse);
	}

	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}

	void FollowPlayer(){
		Transform target = player.transform;
		Vector3 direction = (target.position - this.transform.position).normalized;
		direction.z = 0f;
		transform.Translate (direction * Time.deltaTime * m_speed);
	}

	void ScavengeItems(){
		if (m_seenItems.Count > 0) {
			GameObject target = m_seenItems[0];
			Transform targetPos = target.transform;
			Vector3 direction = targetPos.position - this.transform.position;
			transform.Translate (direction * Time.deltaTime * m_scavengeSpeed);
			float distance = (transform.position - targetPos.position).magnitude;
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
