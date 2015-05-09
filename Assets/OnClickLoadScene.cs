using UnityEngine;
using System.Collections;

public class OnClickLoadScene : MonoBehaviour {
	public string scene;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadScene(){
		Application.LoadLevel (scene);
	}
}
