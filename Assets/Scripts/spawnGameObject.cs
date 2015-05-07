using UnityEngine;
using System.Collections;

public class spawnGameObject : MonoBehaviour {
	public GameObject objectToSpawn;
	// Use this for initialization
	void Start () {
		GameObject spawned = Instantiate (objectToSpawn, this.transform.position, this.transform.rotation) as GameObject;
		spawned.transform.parent = transform.parent;
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
