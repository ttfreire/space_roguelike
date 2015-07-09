using UnityEngine;
using System.Collections;

public class projectileController : MonoBehaviour {
	public float m_maxSpeed;
	string m_targetTag;
	public GameObject m_shooter;
	public float m_damage;

	Rigidbody m_rigidbody;

	public bool isPiercing = false;

	AudioSource[] audios;
	public AudioSource hit;
	public AudioSource thud;

	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
		audios = GetComponents<AudioSource> ();
		hit = audios [0];
		thud = audios [1];
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

		//if(!other.gameObject.transform.tag.Equals("Enemy") && !other.gameObject.transform.tag.Equals("Player"))
		//	Destroy(gameObject);
	}
			
	void OnTriggerEnter(Collider other){
		hit.Play ();
		Rigidbody r_body = other.gameObject.rigidbody;
		if(r_body != null && !other.tag.Equals ("Player"))
			r_body.AddForceAtPosition(5*(r_body.transform.position-m_shooter.transform.position), r_body.transform.position);
		if (!other.tag.Equals ("Enemy") && !other.tag.Equals ("Player") && !other.tag.Equals ("Space")) {
			if (!isPiercing)
				Destroy (gameObject);
		}

		if (!other.tag.Equals ("Enemy") && !other.tag.Equals ("Player"))
			thud.Play ();

	}
	
}
