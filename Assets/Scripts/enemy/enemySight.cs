using UnityEngine;
using System.Collections;

public class enemySight : MonoBehaviour {
	public bool m_isPlayerOnView = false;
	GameObject m_player;
	LayerMask ignoreLayerMask = (1 << 2 | 1 << 8 | 1 << 10 | 1 << 11 | 1 << 13 | 1 << 14) ;
	void Awake(){
		m_player = FindObjectOfType<playerController> ().gameObject;
	}

	// Use this for initialization
	void Start () {
		ignoreLayerMask = ~ignoreLayerMask;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject == m_player) {
			RaycastHit hit;
			Transform shooterPos = transform.FindChild("shooter").transform;
			m_player = FindObjectOfType<playerController> ().gameObject;
			float distance = 50;
			if(Physics.Raycast(shooterPos.position, m_player.transform.position-shooterPos.position, out hit, distance, ignoreLayerMask))
				if(hit.transform.tag.Equals("Player"))
					m_isPlayerOnView = true;
				else
					m_isPlayerOnView = false;
			Debug.DrawLine(shooterPos.position, hit.point, Color.red);

		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.gameObject == m_player)
			m_isPlayerOnView = false;
	}
}
