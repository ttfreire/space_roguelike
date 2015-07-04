using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class loadRoom : MonoBehaviour {
	public List<int> m_PossibleRoomsToSpawn;
	public int m_room;
	public bool isLoaded;
	GameObject m_roomObject;
	int enemiesQuantity;
	GameObject key;
	BoardManager board;
	bool cleanedRoom = false;
	public bool playerInRoom = false;
	playerController p_control;

	List<Animator> doorAnimators;
	List<BoxCollider> doorsColliders;

	// Use this for initialization
	void Awake () {
		gameController game = GameObject.Find ("GameController").GetComponent<gameController> ();
		do {
			m_room = m_PossibleRoomsToSpawn [Random.Range (0, m_PossibleRoomsToSpawn.Count)];
		} while(game.spawnedRoomNumbers.Contains(m_room));

		game.spawnedRoomNumbers.Add(m_room);

		board = FindObjectOfType<BoardManager> ();

		p_control = FindObjectOfType<playerController>();

		doorAnimators = new List<Animator> ();
		doorsColliders = new List<BoxCollider> ();

	}

	void Start(){

	}
	
	// Update is called once per frame
	void Update () {
		if (isLoaded && m_roomObject != null) {
			if(doorAnimators.Count == 0)
				GetDoorsAnimators (m_roomObject);
			Transform enemies = m_roomObject.transform.FindChild("enemies");
			if(enemies != null){
				enemiesQuantity = enemies.childCount;
				if (enemiesQuantity == 0 && !cleanedRoom){
					Transform items = m_roomObject.transform.FindChild("items");
					for(int i = 0; i < items.childCount; i++)
						items.GetChild(i).gameObject.SetActive (true);
					cleanedRoom = true;
					foreach(BoxCollider col in doorsColliders)
						col.isTrigger = true;
					//board.numberOfRoomsToUnlockKey--;
				}
				if(!cleanedRoom)
					foreach(BoxCollider col in doorsColliders)
						col.isTrigger = false;
			}
				else enemiesQuantity = 0;
			if(doorAnimators.Count > 0)
				foreach(Animator animatorDoor in doorAnimators)
					animatorDoor.SetInteger("numEnemies", enemiesQuantity);
		}
	}

	public IEnumerator loadRoomOnContainerPosition(string roomScene){
		Application.LoadLevelAdditive (roomScene);
		yield return 0;
		GameObject room = GameObject.Find(roomScene);
		Vector3 roomNewPosition = transform.position + new Vector3 (-14.3f, 0, -200);
		room.transform.position = roomNewPosition;
		m_roomObject = room;
		isLoaded = true;
	}

	void GetDoorsAnimators(GameObject parent){
		Debug.Log ("So much recursion");
		if (parent.transform.childCount > 0) {
			for(int i = 0; i < parent.transform.childCount; i++)
				GetDoorsAnimators(parent.transform.GetChild(i).gameObject);
		}
		if (parent.tag.Equals ("Porta")) {
			doorAnimators.Add (parent.GetComponent<Animator> ());
			doorsColliders.Add(parent.GetComponent<BoxCollider>());
		}
	}


}
