using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour {

	public float m_health;
	float m_blinkingTime = 0.0f ;
	float m_blinkingTimeMax =21.25f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void TakeDamage(float damageTaken){
		m_health -= damageTaken;
		DamageFeedback ();
	}

	public bool IsDead(){
		if (m_health <= 0.0f)
			return true;
		else
			return false;
	}

	void DamageFeedback(){
		gameObject.renderer.material.color = Color.red;
		while (m_blinkingTime < m_blinkingTimeMax)
			m_blinkingTime += Time.deltaTime;
		m_blinkingTime = 0.0f;
		gameObject.renderer.material.color = Color.white;
	}
}
