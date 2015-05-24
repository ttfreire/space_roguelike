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
	public Text victory;
	public Image m_currentOxygen;

	playerShoot m_pShoot;
	playerController m_pControl;
	playerHealth m_pHealth;
	GameObject player;

	private BoardManager boardScript;                       
	private int level = 3;                                  

	public List<int> spawnedRoomNumbers;

	int destroyableCount;


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
	}

	void Start(){
		//destroyableCount = boardScript.chunkSpecialRoomTiles.Count;
		destroyableCount = 1;
		if (generateLevel)
			boardScript.SetupScene (level);
	}
	

	void Update () {
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
			DisplayEndGameFeedback(GameEndTypes.VICTORY);
			break;
		case GameStates.GAMEOVER:
			Time.timeScale = 0;
			DisplayEndGameFeedback(GameEndTypes.DEFEAT);
			break;
		}
	}

	public void UpdateState(){
		switch (m_currentGameState) {
		case GameStates.RUNNING:

			
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
			Time.timeScale = 0;
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

	void DisplayEndGameFeedback(GameEndTypes endType){
		if (endType.Equals(GameEndTypes.VICTORY)) {
			//Time.timeScale = 0;
			victory.text = "Victory!";
		}
		if(endType.Equals(GameEndTypes.DEFEAT)){
			//Time.timeScale = 0;
			victory.text = "Defeat!";
			Destroy (player);
		}

		GameObject.Find("PlayerCamera").GetComponent<CameraShake>().enabled = false;
	}

	void HideEnemiesFOV(){
		GameObject[] FOVs = GameObject.FindGameObjectsWithTag("FOV");
		foreach(GameObject fov in FOVs){
			MeshRenderer fovMesh = fov.gameObject.GetComponent<MeshRenderer>();
			fovMesh.enabled = !fovMesh.enabled;
		}
	}

	public void KillAllEnemies(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies)
			Destroy (enemy.gameObject);
	}

	public void PauseUnpauseGame(){
		//Time.timeScale = 1.0f - Time.timeScale;
		if (m_currentGameState.Equals (GameStates.RUNNING))
			EnterState (GameStates.PAUSED);
		else
			EnterState (GameStates.RUNNING);
	}

	public void DebugAndTestShortcuts(){
		//if (Input.GetKeyUp (KeyCode.U))
		//	player.GetComponent<upgradeController> ().UpgradetoLevel (m_pControl.m_level);
		
		if (Input.GetKeyUp (KeyCode.I))
			m_pControl.scrap1Quantity += 5;
		
		if (Input.GetKeyUp (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		
		if (Input.GetKeyUp (KeyCode.T))
			KillAllEnemies();
		
		if (Input.GetKeyUp (KeyCode.F))
			HideEnemiesFOV();
	}

	public void UpdateDestroyablesCount(){
		if(destroyableCount > 0)
			destroyableCount--;
	}

}