using UnityEngine;
using System.Collections;

public enum ShootState {NOTSHOOTING, SHOOTING, OVERHEAT};

public class playerShoot : MonoBehaviour {

	ShootState m_state;
	gameController m_gameController;
	Camera m_camera;
	Rigidbody m_rigidbody;

	public float m_shootForce;
	public float m_recoilForce;
	public float m_shootsPerSecond;
	public float m_MaxHeatingTime;
	public float m_MaxCooldownTime;
	float m_nextShot;
	float m_shootCooldown;
	bool m_canShoot = true;
	float m_heatingTime;
	public float m_heatingRecovery;
	public float m_heatingDecrease;

	float m_blinkingTime;
	float m_blinkingTimeMax = 0.25f;

	public GameObject m_projectile;

	public Vector3 shootDirection;
	public Vector3 joyDir = Vector3.zero;
	public Vector3 mouseDir = Vector3.zero;

	// Use this for initialization
	void Start () {
		m_state = ShootState.NOTSHOOTING;
		m_blinkingTime = 0;
		m_rigidbody = rigidbody;
		m_camera = FindObjectOfType<Camera> ();
		m_gameController = FindObjectOfType<gameController> ();
		m_heatingTime = Time.deltaTime * m_MaxHeatingTime;
		m_shootCooldown = Time.deltaTime * m_MaxCooldownTime * 60.0f;
		gameObject.renderer.material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {

		switch (m_state) {
		case ShootState.NOTSHOOTING:{
			if (Input.GetButton ("Fire1"))	
				m_state = ShootState.SHOOTING;
			else
				if(m_heatingTime <= m_MaxHeatingTime*Time.deltaTime)
					m_heatingTime += Time.deltaTime * m_heatingRecovery;
			}
			break;
		case ShootState.SHOOTING:{
			if (Time.time > m_nextShot && Input.GetButton ("Fire1")) {
				m_nextShot = Time.time + ResetShootCooldown ();
				m_heatingTime -= Time.deltaTime * m_heatingDecrease;
				Shoot ();
				
				if (m_heatingTime < 0.0f) {
					m_state = ShootState.OVERHEAT;
				}
			}
			else if(!Input.GetButton ("Fire1"))
			{
				if(m_heatingTime <= m_MaxHeatingTime*Time.deltaTime)
					m_heatingTime += Time.deltaTime * m_heatingRecovery;
				m_state = ShootState.NOTSHOOTING;
			}
		}
			break;
		case ShootState.OVERHEAT:{
			m_shootCooldown -= Time.deltaTime;
			Heating ();
			if (m_shootCooldown <= 0.0f) {
				m_shootCooldown = Time.deltaTime * m_MaxCooldownTime * 60.0f;
				m_heatingTime = Time.deltaTime * m_MaxHeatingTime;
				gameObject.renderer.material.color = Color.white;
				m_state = ShootState.NOTSHOOTING;
			}

			}
			break;
		}
		/**
		if (m_heatingTime < 0.0f)
			m_canShoot = false;

		if (m_canShoot) {
			if (Time.time > m_nextShot && Input.GetButton ("Fire1")) {
				m_nextShot = Time.time + ResetShootCooldown ();
				m_heatingTime -= Time.deltaTime;
				Shoot ();

				if (m_heatingTime < 0.0f) {
					m_canShoot = false;
				}
			}
			else if(Input.GetButtonUp("Fire1"))
				m_heatingTime = Time.deltaTime * m_MaxHeatingTime;
		} else if (m_shootCooldown <= 0.0f) {
			m_shootCooldown = Time.deltaTime * m_MaxCooldownTime * 60.0f;
			m_canShoot = true;
			m_heatingTime = Time.deltaTime * m_MaxHeatingTime;
			gameObject.renderer.material.color = Color.white;
		} else {
			m_shootCooldown -= Time.deltaTime;
			Heating ();
		}

**/
		
	}

	float ResetShootCooldown ()
	{
		return 1 / m_shootsPerSecond;
	}

	bool IsShooting ()
	{
		shootDirection = this.GetShootDirection();
		return Mathf.Round (shootDirection.sqrMagnitude) != 0;
	}

	void Shoot(){
		if (IsShooting ())
			ShootProjectile ();
	}

	Vector3 GetShootDirection(){
		Vector3 direction = Vector3.zero;

		if (m_gameController.isUsingJoystick) {
			float joyX = Input.GetAxis ("Joy X");
			float joyY = Input.GetAxis ("Joy Y");
			joyDir = new Vector3 (joyX, joyY, 0);
			direction = (this.transform.position + joyDir) - this.transform.position;
		} else {
			mouseDir = m_camera.ScreenToWorldPoint (Input.mousePosition);
			direction = mouseDir - this.transform.position;
		}

		direction.z = 0;

		return direction.normalized;
	}

	void ShootProjectile ()
	{
		GameObject proj = (GameObject)Instantiate (m_projectile, this.transform.position, Quaternion.identity);
		proj.GetComponent<projectileController>().SetTargetTag("Enemy");
		proj.GetComponent<projectileController> ().m_shooter = gameObject;
		proj.transform.TransformDirection (shootDirection);
		proj.GetComponent<Rigidbody> ().AddForce (shootDirection * m_shootForce, ForceMode.Impulse);

		//Recoil
		m_rigidbody.AddForce (-(shootDirection * m_recoilForce), ForceMode.Impulse);

		PlayerShake ();
	}

	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}

	void PlayerShake(){
		this.gameObject.GetComponent<CameraShake> ().enabled = true;
	}

	void Heating(){
		if(gameObject.renderer.material.color == Color.white)
			if(m_blinkingTime < m_blinkingTimeMax){
				m_blinkingTime += Time.deltaTime;
			}
			else{
			gameObject.renderer.material.color = Color.red;
				m_blinkingTime = 0.0f;
			}
		if(gameObject.renderer.material.color == Color.red)
			if(m_blinkingTime < m_blinkingTimeMax){
				m_blinkingTime += Time.deltaTime;
			}
			else{
				m_blinkingTime = 0.0f;
				gameObject.renderer.material.color = Color.white;
			}
	}

	public float getHeatingTime(){
		return m_heatingTime;
	}
}
