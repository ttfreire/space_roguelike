using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject m_player;
	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position.Set(m_player.transform.position.x, m_player.transform.position.y, -10);
	}
}
