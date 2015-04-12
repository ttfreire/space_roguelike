using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerHealth : MonoBehaviour {


	public float m_OxygenLossRate;
	public float m_OxygenDamageLoss;


	Camera m_camera;
	public float m_currentOxygenValue;
	public float m_playerTotalOxygen;
	// Use this for initialization
	void Awake () {
		m_currentOxygenValue = m_playerTotalOxygen;
		m_camera = FindObjectOfType<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {

		ConsumeOxygenperSecond (m_OxygenLossRate);
	}

	public void ConsumeOxygenperSecond(float rate){
		m_currentOxygenValue -= Time.deltaTime * rate * 60.0f;
	
	}

	public void TakeDamage(float damageTaken){
		m_currentOxygenValue -= damageTaken;
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}

	public bool IsDead(){
		if (m_currentOxygenValue <= 0.0f)
			return true;
		else
			return false;
	}
}
