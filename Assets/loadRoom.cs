using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadRoom : MonoBehaviour {

	public int m_room;
	public bool isLoaded;
	GameObject m_roomObject;

	GameObject key;
	BoardManager board;
	bool cleanedRoom = false;
	// Use this for initialization
	void Awake () {
		isLoaded = false;
		gameController game = GameObject.Find ("GameController").GetComponent<gameController> ();
		int index = Random.Range (0, game.availableRoomNumbers.Count);
		m_room = game.availableRoomNumbers[index];
		game.availableRoomNumbers.RemoveAt (index);

		board = FindObjectOfType<BoardManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLoaded) {
			Transform enemies = m_roomObject.transform.FindChild("enemies");
			int enemiesQuantity = enemies.childCount;
			if (enemiesQuantity == 0 && !cleanedRoom){
				Transform items = m_roomObject.transform.FindChild("items");
				for(int i = 0; i < items.childCount; i++)
					items.GetChild(i).gameObject.SetActive (true);
				cleanedRoom = true;
				board.numberOfRoomsToUnlockKey--;
			}
			if(cleanedRoom && board.numberOfRoomsToUnlockKey == 0){
				key = m_roomObject.transform.FindChild("Key").GetChild(0).gameObject;
				key.SetActive(true);
			}
		}
	}

	public IEnumerator loadRoomOnContainerPosition(string roomScene){
		Application.LoadLevelAdditive (roomScene);
		yield return 0;
		GameObject room = GameObject.Find(roomScene);
		Vector3 roomNewPosition = transform.position + new Vector3 (0, 0, 200);
		room.transform.position = roomNewPosition;
		m_roomObject = room;
		isLoaded = true;
	}


}
