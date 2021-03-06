﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class powerupDisplayController : MonoBehaviour {
	public Sprite blankSprite;
	public List<Sprite> areaSprites;
	public List<Sprite> damageSprites;
	public List<Sprite> piercingSprites;
	public List<Sprite> velocitySprites;
	public List<Image> powerupDisplay;
	float powerupCounter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(playerController.p_controller)
		if (gameController.control.powerupIsActive) {
			transform.GetChild (1).GetComponent<Image>().enabled = true;
			powerupCounter -= Time.deltaTime;
			transform.GetChild (1).GetComponent<Image> ().fillAmount = powerupCounter / gameController.control.m_powerupDurationSeconds;
		} else {
			transform.GetChild (1).GetComponent<Image>().enabled = false;
			powerupCounter = gameController.control.m_powerupDurationSeconds;
		}
		ClearPowerupListSprites ();
		if (gameController.control.PowerUpItemList.Count > 0) {
			for (int i = gameController.control.PowerUpItemList.Count-1; i >=0 ; i--) {
				itemController iControl = gameController.control.PowerUpItemList [i].GetComponent<itemController> ();
				switch (iControl.m_itemType) {
				case ItemType.AREA:
					powerupDisplay [gameController.control.PowerUpItemList.Count-1-i].sprite = areaSprites [(int)iControl.m_itemShape - 1];
					break;
				case ItemType.DAMAGE:
					powerupDisplay [gameController.control.PowerUpItemList.Count-1-i].sprite = damageSprites [(int)iControl.m_itemShape - 1];
					break;
				case ItemType.PIERCING:
					powerupDisplay [gameController.control.PowerUpItemList.Count-1-i].sprite = piercingSprites [(int)iControl.m_itemShape - 1];
					break;
				case ItemType.VELOCITY:
					powerupDisplay [gameController.control.PowerUpItemList.Count-1-i].sprite = velocitySprites [(int)iControl.m_itemShape - 1];
					break;
				}
			}
		}
	}

	void ClearPowerupListSprites(){
		foreach (Image powerupItem in powerupDisplay)
			powerupItem.sprite = blankSprite;
	}
}
