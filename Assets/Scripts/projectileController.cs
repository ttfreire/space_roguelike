using UnityEngine;
using System.Collections;

public class projectileController : MonoBehaviour {
	public float m_maxSpeed;
	string m_targetTag;
	public GameObject m_shooter;
	public float m_damage;

	Rigidbody m_rigidbody;
	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
	}

	public void SetTargetTag(string tag){
		m_targetTag = tag;
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnBecameInvisible()
	{
		DestroyObject(gameObject);
	}
			

}
