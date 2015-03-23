using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	[HideInInspector] public bool facingRight = true;
	bool isUsingJoystick = false;
	public float speed;
	public float airResistance;
	public float shootForce;
	public float recoilForce;
	public float shootsPerSecond;
	float shootCooldown;
	float rotationSpeed = 10;
	bool hasAirResistance = true;
	Rigidbody m_rigidbody;
	public GameObject projectile;
	Camera m_camera;

	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody> ();
		m_camera = FindObjectOfType<Camera> ();
		shootCooldown = 1 / shootsPerSecond;
	}
	
	// Update is called once per frame
	void Update () {
		shootCooldown -= Time.deltaTime;
		if (Input.GetKeyUp (KeyCode.J))
			isUsingJoystick = !isUsingJoystick;
		if (Input.GetKeyUp (KeyCode.R)) {
			Debug.Log ("R");
			hasAirResistance = !hasAirResistance;
		}
		if (shootCooldown <= 0.0f & Input.GetButtonUp ("Fire1")) {
			shootCooldown = 1 / shootsPerSecond;
			Shoot ();
		}
	}

	void FixedUpdate(){
		if (hasAirResistance) {
			MoveWithAirResistance ();
		} else {
			MoveWithoutAirResistance ();	
		}
		//Debug.Log ("Facing right?: " + facingRight);
	}

	void MoveWithoutAirResistance(){
		m_rigidbody.drag = 0.0F;
		float h_translation = Input.GetAxisRaw("Horizontal") * speed;
		float v_translation = Input.GetAxisRaw("Vertical") * speed;
		if (h_translation > 0 && !facingRight)
			Flip ();
		else if (h_translation < 0 && facingRight)
			Flip ();

		Vector3 direction = new Vector3 (h_translation, v_translation, 0);
		m_rigidbody.AddForceAtPosition(direction, this.transform.position, ForceMode.Acceleration);

	}

	void MoveWithAirResistance(){
		float h_translation = Input.GetAxisRaw("Horizontal") * speed;
		float v_translation = Input.GetAxisRaw("Vertical") * speed;
		if (h_translation > 0 && !facingRight)
			Flip ();
		else if (h_translation < 0 && facingRight)
			Flip ();
		Vector3 direction = new Vector3(h_translation, v_translation, 0);
		Vector3 to_direction = new Vector3 (0, 0, direction.y);

		if (h_translation.Equals (0.0f) && v_translation.Equals (0.0f))
			m_rigidbody.drag = airResistance;
		else {
			m_rigidbody.AddForceAtPosition(direction, this.transform.position, ForceMode.Acceleration);
			m_rigidbody.drag = 0.0F;
		}
	}

	void Shoot(){

		Vector3 direction = Vector3.zero;
		Vector3 joyDir = Vector3.zero;
		if (isUsingJoystick) {
			float joyX = Input.GetAxis ("Joy X");
			float joyY = Input.GetAxis ("Joy Y");
			joyDir = new Vector3 (joyX, joyY, 0);
			direction = transform.position + joyDir * 50;
		}
		else
			direction = m_camera.ScreenToWorldPoint (Input.mousePosition);

		direction.z = 0;
		Vector3 shootDirection = (direction - transform.position).normalized;
		if (Mathf.Floor(direction.magnitude) != 0) {
			GameObject proj = (GameObject)Instantiate (projectile, this.transform.position, Quaternion.identity);
			proj.transform.TransformDirection (shootDirection);
			proj.GetComponent<Rigidbody> ().AddForce (shootDirection * shootForce, ForceMode.Impulse);
			m_rigidbody.AddForce(-(shootDirection*recoilForce), ForceMode.Impulse);
			Debug.Log (-(shootDirection*shootForce));
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
