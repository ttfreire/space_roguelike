using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	playerHealth m_pHealth;

	// Use this for initialization
	void Start () {
		m_pHealth = gameObject.GetComponent<playerHealth> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other){
		if(other.tag.Equals("Projectile") && other.gameObject.GetComponent<projectileController>().m_shooter != gameObject)
		   m_pHealth.TakeDamage(10.0f);
	}

}
