using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameController : MonoBehaviour {

	public static gameController control;
	public bool generateLevel;
	public bool isUsingJoystick = false;
	public Text theInput;
	public Text heating;
	public Text scrap1;
	public Text scrap2;
	public Text victory;
	public Image m_currentOxygen;

	playerShoot m_pShoot;
	playerController m_pControl;
	playerHealth m_pHealth;
	GameObject player;

	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 3;                                  //Current level number, expressed in game as "Day 1".

	// Use this for initialization
	void Awake () {
		/**
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this)
			Destroy (gameObject);
		**/
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		m_pShoot = player.GetComponent<playerShoot> ();
		m_pControl = player.GetComponent<playerController> ();
		m_pHealth = player.GetComponent<playerHealth> ();
		boardScript = GetComponent<BoardManager>();
		if (generateLevel)
			boardScript.SetupScene (level);
	}
	
	// Update is called once per frame
	void Update () {
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
		heating.text = m_pShoot.getHeatingTime().ToString();
		scrap1.text = m_pControl.scrap1Quantity.ToString ();
		scrap2.text = m_pControl.scrap2Quantity.ToString ();
		if (m_pControl.hasKey) {
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