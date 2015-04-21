using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadRoom : MonoBehaviour {

	public int m_room;
	public bool isLoaded;
	// Use this for initialization
	void Awake () {
		isLoaded = false;
		gameController game = GameObject.Find ("GameController").GetComponent<gameController> ();
		int index = Random.Range (0, game.availableRoomNumbers.Count);
		m_room = game.availableRoomNumbers[index];
		game.availableRoomNumbers.RemoveAt (index);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator loadRoomOnContainerPosition(string roomScene){
		Application.LoadLevelAdditive (roomScene);
		yield return 0;
		GameObject room = GameObject.Find(roomScene);
		Vector3 roomNewPosition = transform.position + new Vector3 (0, 0, 200);
		room.transform.position = roomNewPosition;
	}


}
