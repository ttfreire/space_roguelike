﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GameStates {INTRO, RUNNING, PAUSED, VICTORY, GAMEOVER};

public class gameController : MonoBehaviour {


	enum GameEndTypes {VICTORY, DEFEAT};

	public static gameController control;
	public GameStates m_currentGameState;
	public bool generateLevel;
	public Image m_currentOxygen;

	playerShoot m_pShoot;
	playerController m_pControl;
	playerHealth m_pHealth;
	GameObject player;

	private BoardManager boardScript;                       
	private int level = 3;                                  

	public List<int> spawnedRoomNumbers;

	public bool isDiamondCollected = false;
	public GameObject DiamondObject;

	public Canvas vitoria;
	public Canvas derrota;
	public Canvas missao;
	public Canvas bossCanvas;

	public List<GameObject> PowerUpItemList = new List<GameObject>();
	public bool canActivatePowerup = false;
	public bool powerupIsActive = false;

	Animator powerupAnimator;
	bool isEngaging = false;

	public GameObject teleportobj;
	Animator teleportAnimator;

	SpriteRenderer body;
	SpriteRenderer arm;

	public GameObject m_boss;
	public Image m_bossHealth;

	public Canvas powerupAnimationCanvas;
	public float m_powerupDurationSeconds;

	public List<Text> powerupFeedbacks;
	List<int> powerupNumber;

	uiController UIManager;
	public Canvas menuMissao;
	public Canvas menuGeral;

	public bool canMove;
	float timer;
	AudioSource[] audios;
	AudioSource teleportSound;
	AudioSource weaponUpgradeSound;
	AudioSource armorUpgradeSound;
	// Use this for initialization
	void Awake () {
		//EnterState (GameStates.INTRO);
		control = this;
		Time.timeScale = 1;
		player = GameObject.FindGameObjectWithTag ("Player");
		m_pShoot = player.GetComponent<playerShoot> ();
		m_pControl = player.GetComponent<playerController> ();
		m_pHealth = player.GetComponent<playerHealth> ();
		boardScript = GetComponent<BoardManager>();
		//m_currentGameState = GameStates.RUNNING;
		powerupAnimator = FindObjectOfType<powerupDisplayController> ().transform.GetChild(0).GetComponent<Animator> ();
		teleportAnimator = teleportobj.GetComponent<Animator> ();
		body = m_pControl.GetComponent<SpriteRenderer> ();
		arm = m_pControl.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer> ();
		missao.gameObject.SetActive(false);
		m_boss = null;
		audios = GetComponents<AudioSource> ();
		teleportSound = audios[0];
		weaponUpgradeSound = audios[1];
		armorUpgradeSound = audios[2];

	}

	void Start(){
		UIManager = GameObject.FindObjectOfType<uiController> ();
		if (generateLevel)
			boardScript.SetupScene (level);
	}
	

	void Update () {
		powerupAnimator.SetBool ("isEngaging", isEngaging);
		UpdateState ();
		DebugAndTestShortcuts ();
		if (Input.GetKeyUp (KeyCode.Escape)) {
			UIManager.TurnMenuOnOff (menuGeral);
			UIManager.TurnMenuOnOff (menuMissao);
			PauseUnpauseGame();
		}
	}

	public void EnterState(GameStates state){
		ExitState (m_currentGameState);
		m_currentGameState = state;
		
		switch (m_currentGameState) {
		case GameStates.INTRO:
			canMove = false;
			body.enabled = false;
			arm.enabled = false;
			Time.timeScale = 1;
			teleportSound.Play();
			break;
		case GameStates.RUNNING:
			canMove = true;
			Time.timeScale = 1;
			break;
		case GameStates.PAUSED:
			Time.timeScale = 0;
			break;
		case GameStates.VICTORY:
			//Time.timeScale = 0;
			//DisplayEndGameFeedback(GameEndTypes.VICTORY);
			teleportSound.Play();
			timer = 2f;
			canMove = false;
			vitoria.gameObject.SetActive(true);
			GameObject myCam = GameObject.Find("PlayerCamera");
			myCam.GetComponent<cameraController>().music.Stop();
			myCam.GetComponent<cameraController>().completeAudio.Play();
			myCam.GetComponent<CameraShake>().enabled = false;
			break;
		case GameStates.GAMEOVER:
			 GameObject.Find("PlayerCamera").GetComponent<cameraController>().music.Stop();
			canMove = false;
			//Time.timeScale = 0;
			//DisplayEndGameFeedback(GameEndTypes.DEFEAT);
			derrota.gameObject.SetActive(true);
			GameObject.Find("PlayerCamera").GetComponent<CameraShake>().enabled = false;
			break;
		}
	}

	public void UpdateState(){
		switch (m_currentGameState) {
		case GameStates.INTRO:
			if (teleportAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Paused")) {
				EnterState(GameStates.PAUSED);
			}
			break;
		case GameStates.RUNNING:

			VerifyPowerUpItemList();
			OxygenDisplayFeedback ();
			if(m_boss!= null){
				if(!m_boss.GetComponent<pecaController>().IsDead()){
					bossCanvas.gameObject.SetActive(true);
					BossHealthDisplayFeedback();
				}
				else
					bossCanvas.gameObject.SetActive(false);
			}
			if (isDiamondCollected)
				EnterState(GameStates.VICTORY);

			if(m_pHealth.IsDead())
				EnterState(GameStates.GAMEOVER);
			break;
		case GameStates.PAUSED:
			break;
		case GameStates.VICTORY:
			teleportAnimator.SetBool("victory", true);
			if (teleportAnimator.GetCurrentAnimatorStateInfo (0).IsName ("end")) {
				if(timer > 0f){
					timer -= Time.deltaTime;
				} 
				else{
				DiamondObject.SetActive(false);
				body.enabled = false;
				arm.enabled = false;
				}
			}
			break;
		case GameStates.GAMEOVER:
			break;
		}
	}
	
	public void ExitState(GameStates state){
		switch (m_currentGameState) {
		case GameStates.INTRO:
			body.enabled = true;
			arm.enabled = true;
			missao.gameObject.SetActive(true);
			break;
		case GameStates.RUNNING:
			//Time.timeScale = 0;
			break;
		case GameStates.PAUSED:
			break;
		case GameStates.VICTORY:
			break;
		case GameStates.GAMEOVER:
			break;
		}
	}

	void OxygenDisplayFeedback(){
		m_currentOxygen.fillAmount = m_pHealth.m_currentOxygenValue/m_pHealth.m_playerTotalOxygen;
	}


	void BossHealthDisplayFeedback(){
		m_bossHealth.fillAmount = m_boss.GetComponent<pecaController>().m_currentHealth/m_boss.GetComponent<pecaController>().m_health;
	}


	public void PauseUnpauseGame(){
		//Time.timeScale = 1.0f - Time.timeScale;
		if (m_currentGameState.Equals (GameStates.RUNNING))
			EnterState (GameStates.PAUSED);
		else
			EnterState (GameStates.RUNNING);
	}

	public void PauseGame(){
		EnterState (GameStates.PAUSED);
	}

	public void UnpauseGame(){
		EnterState (GameStates.RUNNING);
	}

	public void DebugAndTestShortcuts(){
		
		if (Input.GetKeyUp (KeyCode.L))
			Application.LoadLevel (Application.loadedLevel);


	}
	

	public void AddItemToPowerUpItemList(GameObject item){
		if (PowerUpItemList.Count == 3) {
			GameObject itemToRemove = PowerUpItemList[0];
			PowerUpItemList.RemoveAt (0);
			Destroy(itemToRemove);
		}
		PowerUpItemList.Add(item);
	}

	void VerifyPowerUpItemList(){
		powerupNumber = new List<int>();
		isEngaging = false;
		/**
		if ((PowerUpItemList.Count == 0 && !powerupOn) || PowerUpItemList.Count == 1 || PowerUpItemList.Count == 2)
			playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.NORMAL;
			**/

			if (!canActivatePowerup)
				Removepowerups ();
				
		if (PowerUpItemList.Count == 3 && !powerupIsActive){
			if (AllItensFromType (ItemType.DAMAGE)){
				powerupNumber.Add(0);
				canActivatePowerup = true;
				isEngaging = true;
			}
			if (AllItensFromType (ItemType.VELOCITY)){
				powerupNumber.Add(1);
				canActivatePowerup = true;
				isEngaging = true;
			}
			if (AllItensFromType (ItemType.PIERCING)){
				powerupNumber.Add(2);
				canActivatePowerup = true;
				isEngaging = true;
			}
			if (AllItensFromType (ItemType.AREA)){
				powerupNumber.Add(3);
				canActivatePowerup = true;
				isEngaging = true;
			}
		}
		/**
		if ((PowerUpItemList.Count == 0 && !powerupOn) || PowerUpItemList.Count == 1 || PowerUpItemList.Count == 2) {
			playerMovement.p_Movement.currentSpeed = playerMovement.p_Movement.m_speed;
			playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OxygenLossRate;
			playerHealth.p_Health.damageReductionDivider = 1;
		}
		**/

			if(!canActivatePowerup){
				Removepowerups();
			}
		if (PowerUpItemList.Count == 3 && !powerupIsActive){
			if (AllItensFromShape (ItemShape.TRIANGLE)){
				powerupNumber.Add(4);
				canActivatePowerup = true;
				isEngaging = true;
			}
			if (AllItensFromShape (ItemShape.SQUARE)){
				powerupNumber.Add(5);
				canActivatePowerup = true;
				isEngaging = true;
			}
			if (AllItensFromShape (ItemShape.PENTAGON)){
				powerupNumber.Add(6);
				canActivatePowerup = true;
				isEngaging = true;
			}
		}

		if (canActivatePowerup && PowerUpItemList.Count == 3 && Input.GetKey (KeyCode.E) && !powerupIsActive) {
			Removepowerups();
			PowerUpItemList.Clear ();
			ActivatePowerup(powerupNumber);

			StartCoroutine (powerupOff ());
		}

			

	}

	bool AllItensFromType(ItemType type){
		if (PowerUpItemList.Count == 0)
			return false;
		bool allItensAreEqual = true;
		foreach (GameObject item in PowerUpItemList)
			if (!item.GetComponent<itemController> ().m_itemType.Equals (type))
				allItensAreEqual = false;
		return allItensAreEqual;
	}

	bool AllItensFromShape(ItemShape shape){
		if (PowerUpItemList.Count == 0)
			return false;
		bool allItensAreEqual = true;
		foreach (GameObject item in PowerUpItemList)
			if (!item.GetComponent<itemController> ().m_itemShape.Equals (shape))
				allItensAreEqual = false;
		return allItensAreEqual;
	}

	IEnumerator powerupOff ()
	{
		isEngaging = false;
		yield return new WaitForSeconds (m_powerupDurationSeconds);
		canActivatePowerup = false;
		powerupIsActive = false;
		playerController.p_controller.SelectCorrectArmFromPowerUp (0);

		foreach (Text feedback in powerupFeedbacks)
			feedback.gameObject.SetActive (false);
	}

	void ActivatePowerup(List<int> powerup){
		powerupAnimationCanvas.gameObject.SetActive (true);
		powerupIsActive = true;
		for (int i = 0; i < powerup.Count; i++) {
			switch (powerup[i]) {
			case 0:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.DAMAGE;
				powerupFeedbacks[0].gameObject.SetActive(true);
				weaponUpgradeSound.Play();
				break;
			case 1:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.VELOCITY;
				powerupFeedbacks[1].gameObject.SetActive(true);
				weaponUpgradeSound.Play();
				break;
			case 2:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.PIERCING;
				powerupFeedbacks[2].gameObject.SetActive(true);
				weaponUpgradeSound.Play();
				break;
			case 3:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.AREA;
				powerupFeedbacks[3].gameObject.SetActive(true);
				weaponUpgradeSound.Play();
				break;
			case 4:
				playerMovement.p_Movement.currentSpeed = playerMovement.p_Movement.m_speed + 1.0f;
				playerController.p_controller.SelectCorrectArmFromPowerUp (1);
				powerupFeedbacks[4].gameObject.SetActive(true);
				armorUpgradeSound.Play();
				break;
			case 5:
				playerHealth.p_Health.m_currentOxygenLossRate = 0f;
				powerupFeedbacks[5].gameObject.SetActive(true);
				armorUpgradeSound.Play();
				break;
			case 6:
				playerHealth.p_Health.damageReductionDivider = 2;
				playerController.p_controller.SelectCorrectArmFromPowerUp (2);
				powerupFeedbacks[6].gameObject.SetActive(true);
				armorUpgradeSound.Play();
				break;
			}
		}
	}

	void Removepowerups(){
		powerupAnimationCanvas.gameObject.SetActive (false);
		playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.NORMAL;
		playerMovement.p_Movement.currentSpeed = playerMovement.p_Movement.m_speed;
		playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OxygenLossRate;
		playerHealth.p_Health.damageReductionDivider = 1;
	}

	public void Exitgame(){
		Application.Quit ();
	}
}