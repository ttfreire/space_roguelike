using UnityEngine;
using System.Collections;

public class projectileController : MonoBehaviour {
	public float m_maxSpeed;
	string m_targetTag;
	public GameObject m_shooter;

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

	void OnTriggerStay(Collider other){

		if (other.tag.Equals (m_targetTag) && !other.tag.Equals("Player"))
			Destroy (other.gameObject);
		if(other.gameObject != m_shooter && other.gameObject.tag != "FOV")
			Destroy (this.gameObject);
	}
}
