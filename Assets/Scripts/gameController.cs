using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameController : MonoBehaviour {
	public bool generateLevel;
	public bool isUsingJoystick = false;
	public Text theInput;
	public Text theDirection;
	public Text theJoy;
	public Text theMouse;
	public Text heating;

	playerShoot m_pShoot;

	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 3;                                  //Current level number, expressed in game as "Day 1".

	// Use this for initialization
	void Awake () {
		m_pShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<playerShoot> ();

		boardScript = GetComponent<BoardManager>();
		if(generateLevel)
			boardScript.SetupScene(level);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		if (Input.GetKeyUp (KeyCode.J))
			isUsingJoystick = !isUsingJoystick;
		InterfaceFeedback ();

	}

	void InterfaceFeedback(){
		if (isUsingJoystick)
			theInput.text = "Joystick";
		else
			theInput.text = "Mouse";
		heating.text = m_pShoot.getHeatingTime().ToString();
	}
}
