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
		Vector3 mouseDir = Vector3.zero;

		if (isUsingJoystick) {
			float joyX = Input.GetAxis ("Joy X");
			float joyY = Input.GetAxis ("Joy Y");
			joyDir = new Vector3 (joyX, joyY, 0);
			direction = (this.transform.position + joyDir) - this.transform.position;
		} else {
			mouseDir = m_camera.ScreenToWorldPoint (Input.mousePosition);
			direction = mouseDir - this.transform.position;
		}

		direction.z = 0;
		Debug.Log ("Joy: " + joyDir + "\n" + "Mouse: " + mouseDir + "\n" + "Dir: " + direction + "\n" + "Pos: " + transform.position);

		return direction.normalized;
	}

	void ShootProjectile ()
	{
		Vector3 shootDirection = this.GetShootDirection ();

		GameObject proj = (GameObject)Instantiate (m_projectile, this.transform.position, Quaternion.identity);
		proj.GetComponent<projectileController>().SetTargetTag("Enemy");
		proj.GetComponent<projectileController> ().m_shooter = gameObject;
		proj.transform.TransformDirection (shootDirection);
		proj.GetComponent<Rigidbody> ().AddForce (shootDirection * m_shootForce, ForceMode.Impulse);

		//Recoil
		m_rigidbody.AddForce (-(shootDirection * m_recoilForce), ForceMode.Impulse);

		CameraShake ();
	}

	void CameraShake(){
		m_camera.GetComponent<CameraShake> ().enabled = true;
	}
}
