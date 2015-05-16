using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chunkController : MonoBehaviour {

	public int chunkRow;
	public int chunkColumn;
	public bool hasPlayer; 

	void Awake(){
		hasPlayer = false;
	}

	public void SetChunkRow(int row){
		chunkRow = row;
	}

	public void SetChunkColumn(int column){
		chunkColumn = column;
	}

	void OnTriggerEnter(Collider other){
		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Enemy"))
			other.gameObject.transform.SetParent (gameObject.transform.parent);

		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Movable"))
			other.gameObject.transform.SetParent (gameObject.transform.parent);
		
		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Player")) {
			hasPlayer = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Player"))
			hasPlayer = false;
	}

	public bool isplayeronChunk(){
		return hasPlayer;
	}

}
