using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadRoom : MonoBehaviour {
	public List<int> m_PossibleRoomsToSpawn;
	public int m_room;
	public bool isLoaded;
	GameObject m_roomObject;

	GameObject key;
	BoardManager board;
	bool cleanedRoom = false;
	public bool playerInRoom = false;
	playerController p_control;

	// Use this for initialization
	void Awake () {
		isLoaded = false;
		gameController game = GameObject.Find ("GameController").GetComponent<gameController> ();
		do {
			m_room = m_PossibleRoomsToSpawn [Random.Range (0, m_PossibleRoomsToSpawn.Count)];
		} while(game.spawnedRoomNumbers.Contains(m_room));

		game.spawnedRoomNumbers.Add(m_room);

		board = FindObjectOfType<BoardManager> ();

		p_control = FindObjectOfType<playerController>();


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
				//board.numberOfRoomsToUnlockKey--;
			}
		}
	}

	public IEnumerator loadRoomOnContainerPosition(string roomScene){
		Application.LoadLevelAdditive (roomScene);
		yield return 0;
		GameObject room = GameObject.Find(roomScene);
		Vector3 roomNewPosition = transform.position + new Vector3 (-15, 0, 200);
		room.transform.position = roomNewPosition;
		m_roomObject = room;
		isLoaded = true;
	}



}
