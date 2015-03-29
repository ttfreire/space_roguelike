using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameController : MonoBehaviour {
	public bool isUsingJoystick = false;
	public Text theInput;
	public Text theDirection;
	public Text theJoy;
	public Text theMouse;

	playerShoot m_pShoot;

	// Use this for initialization
	void Awake () {
		m_pShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<playerShoot> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		if (Input.GetKeyUp (KeyCode.J))
			isUsingJoystick = !isUsingJoystick;
		InterfaceFeedback ();

		theDirection.text = m_pShoot.shootDirection.ToString()+" (sqrMagnitude: )"+m_pShoot.shootDirection.sqrMagnitude.ToString();
		theJoy.text = m_pShoot.joyDir.ToString();
		theMouse.text = m_pShoot.mouseDir.ToString();
	}

	void InterfaceFeedback(){
		if (isUsingJoystick)
			theInput.text = "Joystick";
		else
			theInput.text = "Mouse";
	}
}
