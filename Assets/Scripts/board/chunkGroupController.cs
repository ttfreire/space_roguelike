using UnityEngine;
using System.Collections;

public class chunkGroupController : MonoBehaviour {
	public int columns;
	public int rows;
	BoardManager board;
	// Use this for initialization
	void Awake () {
		board = FindObjectOfType<BoardManager> ();
		DetachChildrenfromParent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void DetachChildrenfromParent(){
		while (transform.childCount > 0)
			transform.GetChild (0).parent = board.boardHolder;
		Destroy (transform.gameObject);
	}
}
