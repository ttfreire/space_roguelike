using UnityEngine;
using System.Collections;

public class projectileController : MonoBehaviour {
	public float m_maxSpeed;
	string m_targetTag;

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

	void OnTriggerEnter(Collider other){
		Debug.Log ("Tag: "+m_targetTag);
		if (other.tag.Equals (m_targetTag)) {
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}
	}
}
