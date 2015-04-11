using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour {

	public float m_health;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void TakeDamage(float damageTaken){
		m_health -= damageTaken;
	}

	public bool IsDead(){
		if (m_health <= 0.0f)
			return true;
		else
			return false;
	}
}
