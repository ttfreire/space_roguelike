﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerHealth : MonoBehaviour {
	public static playerHealth p_Health;

	public float m_OxygenLossRate;
	public float m_OuterSpaceOxygenLossRate;
	public float m_currentOxygenLossRate;
	public float m_OxygenDamageLoss;
	public float m_currentOxygenValue;
	public float m_playerTotalOxygen;
	public float damageReductionDivider;
	playerController p_control;
	Camera m_camera;

	void Awake () {
		p_Health = this;
		m_currentOxygenValue = m_playerTotalOxygen;
		m_camera = FindObjectOfType<Camera> ();
		p_control = GetComponent<playerController> ();
		m_currentOxygenLossRate = m_OxygenLossRate;
		damageReductionDivider = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(!p_control.isInsideRoom)
			ConsumeOxygenperSecond (m_currentOxygenLossRate);
	}

	public void ConsumeOxygenperSecond(float rate){
		m_currentOxygenValue -= Time.deltaTime * rate * 60.0f;
	}

	public void TakeDamage(float damageTaken){
		m_currentOxygenValue -= damageTaken / damageReductionDivider;
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}

	public bool IsDead(){
		if (m_currentOxygenValue <= 0.0f)
			return true;
		else
			return false;
	}
}
