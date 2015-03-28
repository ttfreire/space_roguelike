using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour {

	public Text theInput;
	bool isUsingJoystick = true;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		InterfaceFeedback ();
		if (Input.GetKeyUp (KeyCode.J))
			isUsingJoystick = !isUsingJoystick;
	}

	void InterfaceFeedback(){
		if (isUsingJoystick)
			theInput.text = "Joystick";
		else
			theInput.text = "Mouse";
	}
}
