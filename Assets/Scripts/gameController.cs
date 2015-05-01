using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

	public static gameController control;
	public bool generateLevel;
	public bool isUsingJoystick = false;
	public Text theInput;
	public Text scrap1;
	public Text scrap2;
	public Text victory;
	public Image m_currentOxygen;

	playerShoot m_pShoot;
	playerController m_pControl;
	playerHealth m_pHealth;
	GameObject player;

	private BoardManager boardScript;                       
	private int level = 3;                                  

	public List<int> availableRoomNumbers;

	// Use this for initialization
	void Awake () {
	
		player = GameObject.FindGameObjectWithTag ("Player");
		m_pShoot = player.GetComponent<playerShoot> ();
		m_pControl = player.GetComponent<playerController> ();
		m_pHealth = player.GetComponent<playerHealth> ();
		boardScript = GetComponent<BoardManager>();
		if (generateLevel)
			boardScript.SetupScene (level);
	}
	

	void Update () {
		if (Input.GetKeyUp (KeyCode.U))
			player.GetComponent<upgradeController> ().UpgradetoLevel (m_pControl.m_level);

		if (Input.GetKeyUp (KeyCode.I)) {
			m_pControl.scrap1Quantity += 5;
			m_pControl.scrap2Quantity += 5;
		}
		if (Input.GetKeyUp (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		if (Input.GetKeyUp (KeyCode.J))
			isUsingJoystick = !isUsingJoystick;
		if (Input.GetKeyUp (KeyCode.T)){
			KillAllEnemies();
		}
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
		scrap1.text = m_pControl.scrap1Quantity.ToString ();
		scrap2.text = m_pControl.scrap2Quantity.ToString ();

		if (m_pControl.endlevel) {
			victory.text = "Victory!";
			KillAllEnemies();
		}

		if(m_pHealth.IsDead()){
			victory.text = "Defeat!";
			KillAllEnemies();
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
}