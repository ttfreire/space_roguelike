using UnityEngine;
using System.Collections;

public class projectileController : MonoBehaviour {
	public float m_maxSpeed;
	Rigidbody m_rigidbody;
	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnBecameInvisible()
	{
		DestroyObject(gameObject);
	}
}
