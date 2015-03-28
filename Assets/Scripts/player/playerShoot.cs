using UnityEngine;
using System.Collections;

public class playerShoot : MonoBehaviour {

	bool isUsingJoystick = true;
	Camera m_camera;
	Rigidbody m_rigidbody;

	public float m_shootForce;
	public float m_recoilForce;
	public float m_shootsPerSecond;
	float m_shootCooldown;
		
	public GameObject m_projectile;

	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_camera = FindObjectOfType<Camera> ();
		m_shootCooldown = ResetShootCooldown ();
	}
	
	// Update is called once per frame
	void Update () {
		m_shootCooldown -= Time.deltaTime;
		if (Input.GetKeyUp (KeyCode.J))
			isUsingJoystick = !isUsingJoystick;
		
		if (m_shootCooldown <= 0.0f & Input.GetButtonUp ("Fire1")) {
			m_shootCooldown = 1 / m_shootsPerSecond;
			Shoot ();
		}
	}

	float ResetShootCooldown ()
	{
		return 1 / m_shootsPerSecond;
	}

	bool IsShooting ()
	{
		Vector3 shootDirection = this.GetShootDirection();
		return Mathf.Floor (shootDirection.magnitude) != 0;
	}

	void Shoot(){
		if (IsShooting ())
			ShootProjectile ();
	}

	Vector3 GetShootDirection(){
		Vector3 direction = Vector3.zero;
		Vector3 joyDir = Vector3.zero;

		if (isUsingJoystick) {
			float joyX = Input.GetAxis ("Joy X");
			float joyY = Input.GetAxis ("Joy Y");
			joyDir = new Vector3 (joyX, joyY, 0);
			direction = joyDir;
		}
		else
			direction = m_camera.ScreenToWorldPoint (Input.mousePosition);

		direction.z = 0;


		return direction.normalized;
	}

	void ShootProjectile ()
	{
		Vector3 shootDirection = this.GetShootDirection ();

		GameObject proj = (GameObject)Instantiate (m_projectile, this.transform.position, Quaternion.identity);
		proj.transform.TransformDirection (shootDirection);
		proj.GetComponent<Rigidbody> ().AddForce (shootDirection * m_shootForce, ForceMode.Impulse);

		//Recoil
		m_rigidbody.AddForce (-(shootDirection * m_recoilForce), ForceMode.Impulse);
	}
}
