using UnityEngine;
using System.Collections;

public class enemySight : MonoBehaviour {
	[HideInInspector] public bool m_isPlayerOnView = false;
	GameObject m_player;
	void Awake(){
		m_player = FindObjectOfType<playerController> ().gameObject;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject == m_player) {
			m_isPlayerOnView = true;
		}
		else
			m_isPlayerOnView = false;
	}
	
	void OnTriggerExit(Collider other){
		if (other.gameObject == m_player)
			m_isPlayerOnView = false;
	}
}
