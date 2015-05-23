using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

	public static gameController control;
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

	// Use this for initialization
	void Awake () {
		control = this;
		Time.timeScale = 1;
		player = GameObject.FindGameObjectWithTag ("Player");
		m_pShoot = player.GetComponent<playerShoot> ();
		m_pControl = player.GetComponent<playerController> ();
		m_pHealth = player.GetComponent<playerHealth> ();
		boardScript = GetComponent<BoardManager>();

	}

	void Start(){
		if (generateLevel)
			boardScript.SetupScene (level);
	}
	

	void Update () {
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

		if (Input.GetKeyUp (KeyCode.L)){
			GameObject sceneRoot = GameObject.Find("_ROOT");
			Destroy(sceneRoot);
		}
		InterfaceFeedback ();

	}

	void InterfaceFeedback(){
		m_currentOxygen.fillAmount = m_pHealth.m_currentOxygenValue/m_pHealth.m_playerTotalOxygen;

		if (m_pControl.endlevel) {
			Time.timeScale = 0;
			victory.text = "Victory!";
			//KillAllEnemies();
		}

		if(m_pHealth.IsDead()){
			Time.timeScale = 0;
			victory.text = "Defeat!";
			//KillAllEnemies();
			GameObject.Find("PlayerCamera").GetComponent<CameraShake>().enabled = false;
			Destroy (player);
		}



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
		Time.timeScale = 1.0f - Time.timeScale;
	}

}