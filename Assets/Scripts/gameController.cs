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

	playerShoot m_pShoot;

	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 3;                                  //Current level number, expressed in game as "Day 1".

	// Use this for initialization
	void Awake () {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this)
			Destroy (gameObject);
		m_pShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<playerShoot> ();

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

		heating.text = m_pShoot.getHeatingTime().ToString();
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