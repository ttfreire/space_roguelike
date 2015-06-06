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

	void OnCollisionStay(Collision other){
		/**
		if(other.transform.parent != null)
			if(other.gameObject.transform.parent.tag.Equals("LevelLimit") || other.gameObject.transform.tag.Equals("LevelLimit") || 
			   other.gameObject.transform.tag.Equals("FinalRoom") || other.gameObject.transform.tag.Equals("Door") 
			   || other.gameObject.transform.tag.Equals("Container"))
			   Destroy(gameObject);
			   **/

		if(!other.gameObject.transform.tag.Equals("Enemy") && !other.gameObject.transform.tag.Equals("Player"))
			Destroy(gameObject);
	}
			

}
