using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public float distance = -10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z+distance);
	}
}
