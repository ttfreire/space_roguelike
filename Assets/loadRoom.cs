using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadRoom : MonoBehaviour {
	public int[] m_PossibleRoomsToSpawn;
	public int m_room;
	public bool isLoaded;
	GameObject m_roomObject;

	GameObject key;
	BoardManager board;
	bool cleanedRoom = false;
	public bool playerInRoom = false;
	// Use this for initialization
	void Awake () {
		isLoaded = false;
		gameController game = GameObject.Find ("GameController").GetComponent<gameController> ();
		int index = Random.Range (0, game.spawnedRoomNumbers.Count);
		do {
			m_room = m_PossibleRoomsToSpawn [Random.Range (0, m_PossibleRoomsToSpawn.Length)];
		} while(game.spawnedRoomNumbers.Contains(m_room));

		game.spawnedRoomNumbers.Add(m_room);

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
				if(m_roomObject.transform.FindChild("Key").gameObject != null){
					key = m_roomObject.transform.FindChild("Key").gameObject;
					key.GetComponent<spawnGameObject>().enabled = true;
				}
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
