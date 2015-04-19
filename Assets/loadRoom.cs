using UnityEngine;
using System.Collections;

public class loadRoom : MonoBehaviour {
	public int[] m_rooms;
	[HideInInspector]public int m_room;
	public bool isLoaded;
	// Use this for initialization
	void Awake () {
		isLoaded = false;
		m_room = m_rooms[Random.Range(0, m_rooms.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
