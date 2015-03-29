using UnityEngine;
using System.Collections;

public class playerShoot : MonoBehaviour {

	gameController m_gameController;
	Camera m_camera;
	Rigidbody m_rigidbody;

	public float m_shootForce;
	public float m_recoilForce;
	public float m_shootsPerSecond;
	float m_shootCooldown;
		
	public GameObject m_projectile;

	public Vector3 shootDirection;
	public Vector3 joyDir = Vector3.zero;
	public Vector3 mouseDir = Vector3.zero;

	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_camera = FindObjectOfType<Camera> ();
		m_shootCooldown = ResetShootCooldown ();
		m_gameController = FindObjectOfType<gameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_shootCooldown -= Time.deltaTime;
				
		if (m_shootCooldown <= 0.0f && Input.GetButton ("Fire1")) {
			m_shootCooldown = ResetShootCooldown();
			Shoot ();
		}
	}

	float ResetShootCooldown ()
	{
		return 1 / m_shootsPerSecond;
	}

	bool IsShooting ()
	{
		shootDirection = this.GetShootDirection();
		return Mathf.Floor (shootDirection.sqrMagnitude) != 0;
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
}
