using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameStates {RUNNING, PAUSED, VICTORY, GAMEOVER};

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

	int destroyableCount;

	public Canvas vitoria;
	public Canvas derrota;

	public List<GameObject> PowerUpItemList = new List<GameObject>();
	bool powerupOn = false;

	Animator powerupAnimator;
	bool isEngaging = false;

	// Use this for initialization
	void Awake () {
		control = this;
		Time.timeScale = 1;
		player = GameObject.FindGameObjectWithTag ("Player");
		m_pShoot = player.GetComponent<playerShoot> ();
		m_pControl = player.GetComponent<playerController> ();
		m_pHealth = player.GetComponent<playerHealth> ();
		boardScript = GetComponent<BoardManager>();
		m_currentGameState = GameStates.RUNNING;
		powerupAnimator = FindObjectOfType<powerupDisplayController> ().transform.GetChild(0).GetComponent<Animator> ();
	}

	void Start(){
		//destroyableCount = boardScript.chunkSpecialRoomTiles.Count;
		destroyableCount = boardScript.specialRoomsQuantity;
		if (generateLevel)
			boardScript.SetupScene (level);
		EnterState (m_currentGameState);
	}
	

	void Update () {
		powerupAnimator.SetBool ("isEngaging", isEngaging);
		UpdateState ();
		DebugAndTestShortcuts ();
	}

	public void EnterState(GameStates state){
		ExitState (m_currentGameState);
		m_currentGameState = state;
		
		switch (m_currentGameState) {
		case GameStates.RUNNING:
			Time.timeScale = 1;
			break;
		case GameStates.PAUSED:
			Time.timeScale = 0;
			break;
		case GameStates.VICTORY:
			Time.timeScale = 0;
			//DisplayEndGameFeedback(GameEndTypes.VICTORY);
			vitoria.gameObject.SetActive(true);
			GameObject.Find("PlayerCamera").GetComponent<CameraShake>().enabled = false;
			break;
		case GameStates.GAMEOVER:
			//Time.timeScale = 0;
			//DisplayEndGameFeedback(GameEndTypes.DEFEAT);
			derrota.gameObject.SetActive(true);
			GameObject.Find("PlayerCamera").GetComponent<CameraShake>().enabled = false;
			break;
		}
	}

	public void UpdateState(){
		switch (m_currentGameState) {
		case GameStates.RUNNING:

			VerifyPowerUpItemList();
			OxygenDisplayFeedback ();

			if (destroyableCount == 0)
				EnterState(GameStates.VICTORY);

			if(m_pHealth.IsDead())
				EnterState(GameStates.GAMEOVER);
			break;
		case GameStates.PAUSED:
			break;
		case GameStates.VICTORY:
			break;
		case GameStates.GAMEOVER:
			break;
		}
	}
	
	public void ExitState(GameStates state){
		switch (m_currentGameState) {
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



	public void PauseUnpauseGame(){
		//Time.timeScale = 1.0f - Time.timeScale;
		if (m_currentGameState.Equals (GameStates.RUNNING))
			EnterState (GameStates.PAUSED);
		else
			EnterState (GameStates.RUNNING);
	}

	public void DebugAndTestShortcuts(){
		
		if (Input.GetKeyUp (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);


	}

	public void UpdateDestroyablesCount(){
		if(destroyableCount > 0)
			destroyableCount--;
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
		List<int> powerupNumber = new List<int>();
		/**
		if ((PowerUpItemList.Count == 0 && !powerupOn) || PowerUpItemList.Count == 1 || PowerUpItemList.Count == 2)
			playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.NORMAL;
			**/

			if (!powerupOn)
				Removepowerups ();
				
		if (PowerUpItemList.Count == 3){
			if (AllItensFromType (ItemType.DAMAGE)){
				powerupNumber.Add(0);
				powerupOn = true;
				isEngaging = true;
			}
			if (AllItensFromType (ItemType.VELOCITY)){
				powerupNumber.Add(1);
				powerupOn = true;
				isEngaging = true;
			}
			if (AllItensFromType (ItemType.PIERCING)){
				powerupNumber.Add(2);
				powerupOn = true;
				isEngaging = true;
			}
			if (AllItensFromType (ItemType.AREA)){
				powerupNumber.Add(3);
				powerupOn = true;
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

			if(!powerupOn){
				Removepowerups();
			}
		if (PowerUpItemList.Count == 3){
			if (AllItensFromShape (ItemShape.TRIANGLE)){
				powerupNumber.Add(4);
				powerupOn = true;
				isEngaging = true;
			}
			if (AllItensFromShape (ItemShape.SQUARE)){
				powerupNumber.Add(5);
				powerupOn = true;
				isEngaging = true;
			}
			if (AllItensFromShape (ItemShape.PENTAGON)){
				powerupNumber.Add(6);
				powerupOn = true;
				isEngaging = true;
			}
		}

		if (powerupOn && PowerUpItemList.Count == 3 && Input.GetKey (KeyCode.E)) {
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
		yield return new WaitForSeconds (15.0f);
		powerupOn = false;
		playerController.p_controller.SelectCorrectArmFromPowerUp (0);
	}

	void ActivatePowerup(List<int> powerup){
		for (int i = 0; i < powerup.Count; i++) {
			switch (powerup[i]) {
			case 0:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.DAMAGE;
				break;
			case 1:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.VELOCITY;
				break;
			case 2:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.PIERCING;
				break;
			case 3:
				playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.AREA;
				break;
			case 4:
				playerMovement.p_Movement.currentSpeed = playerMovement.p_Movement.m_speed + 1.0f;
				playerController.p_controller.SelectCorrectArmFromPowerUp (1);
				break;
			case 5:
				playerHealth.p_Health.m_currentOxygenLossRate = 0f;
				break;
			case 6:
				playerHealth.p_Health.damageReductionDivider = 2;
				playerController.p_controller.SelectCorrectArmFromPowerUp (2);
				break;
			}
		}
	}

	void Removepowerups(){
		playerShoot.p_Shoot.currentAmmoType = playerShoot.ProjectileType.NORMAL;
		playerMovement.p_Movement.currentSpeed = playerMovement.p_Movement.m_speed;
		playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OxygenLossRate;
		playerHealth.p_Health.damageReductionDivider = 1;
	}
	
}