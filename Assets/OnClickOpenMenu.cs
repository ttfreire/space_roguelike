using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClickOpenMenu : MonoBehaviour {

	public Canvas menu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void TurnMenuOnOff(){
		menu.gameObject.SetActive (!menu.gameObject.activeSelf);
		if(menu.gameObject.activeSelf)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}

}
