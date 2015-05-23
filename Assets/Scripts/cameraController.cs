using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public float distance = -10;
	GameObject room;
	// Use this for initialization
	void Start () {
		room = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING)) {
			if (player.GetComponent<playerController> ().isInsideRoom) {
				if (room == null)
					room = GameObject.Find (player.GetComponent<playerController> ().room);
				if (room != null)
					this.transform.position = new Vector3 (room.transform.position.x + 15.3f, room.transform.position.y, room.transform.position.z + distance);
			} else {
				room = null;
				this.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z + distance);
			}
		}
	}
}
