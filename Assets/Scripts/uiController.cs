using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiController : MonoBehaviour {
	public static uiController uiControl; 
	//public Canvas menu;
	//string scene;

	Color selectedColor = Color.white;
	Color notSelectedColor = Color.gray;
	AudioSource sound;
	void Awake(){
		uiControl = this;
		sound = GetComponent<AudioSource> ();
	}

	public void TurnMenuOnOff(Canvas menu){
		menu.gameObject.SetActive (!menu.gameObject.activeSelf);
		sound.Play ();
		/**
		if(menu.gameObject.activeSelf)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
			**/
	}

	public void TurnMenuOn(Canvas menu){
		menu.gameObject.SetActive (true);
		sound.Play ();
	}

	public void TurnMenuOff(Canvas menu){
		menu.gameObject.SetActive (false);
		sound.Play ();
	}

	public void LoadScene(string scene){
		Application.LoadLevel (scene);
	}

	public void Select(Text option){
		option.color = selectedColor;
	}

	public void Deselect(Text option){
		option.color = notSelectedColor;
	}

	public void FullScreen(bool isFullScreen){
		Screen.fullScreen = isFullScreen;
	}



}
