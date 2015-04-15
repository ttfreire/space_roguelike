using UnityEngine;
using System.Collections;

public class loadRoom : MonoBehaviour {
	playerController m_pControl;
	public int m_room;
	// Use this for initialization
	void Awake () {
		m_pControl = FindObjectOfType<playerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
