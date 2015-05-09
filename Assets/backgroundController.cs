using UnityEngine;
using System.Collections;

public class backgroundController : MonoBehaviour {


	void OnTriggerEnter(Collider other){
		Debug.Log ("entrou");
		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Enemy"))
			other.gameObject.transform.SetParent (gameObject.transform.parent);
		
		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Player")) {
			//other.gameObject.transform.SetParent (gameObject.transform);
			transform.parent.gameObject.GetComponent<chunkController>().hasPlayer = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if (gameObject.transform.parent.tag.Equals ("Respawn") && other.gameObject.tag.Equals ("Player"))
			transform.parent.gameObject.GetComponent<chunkController>().hasPlayer = false;
	}
}
