using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadRoom : MonoBehaviour {

	public int m_room;
	public bool isLoaded;
	GameObject m_roomObject;
	List<GameObject> itens;
	List<GameObject> enemies;
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
		itens = new List<GameObject> ();
		enemies = new List<GameObject> ();
		board = FindObjectOfType<BoardManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLoaded) {
			enemies.Clear ();
			Transform roomTransform = m_roomObject.transform;
			foreach (Transform child in roomTransform) {
				if(child.gameObject.tag.Equals("Item")){
					if(child.gameObject.name.Equals("Key")){
						key = child.gameObject;
					}
					else{
						itens.Add(child.gameObject);
					}
				}
				if (child.gameObject.tag.Equals ("Enemy"))
					enemies.Add (child.gameObject);
			}
			if (enemies.Count == 0 && !cleanedRoom){
				foreach (GameObject item in itens)
					item.SetActive (true);
				cleanedRoom = true;
				board.numberOfRoomsToUnlockKey--;
			}
			if(cleanedRoom && board.numberOfRoomsToUnlockKey == 0)
				key.SetActive(true);
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
