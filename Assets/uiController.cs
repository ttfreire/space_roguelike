using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiController : MonoBehaviour {
	//public Canvas menu;
	//string scene;

	Color selectedColor = Color.white;
	Color notSelectedColor = Color.gray;

	public void TurnMenuOnOff(Canvas menu){
		menu.gameObject.SetActive (!menu.gameObject.activeSelf);
		/**
		if(menu.gameObject.activeSelf)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
			**/
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

}
