using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	
	[HideInInspector] public bool facingRight = true;
	Rigidbody m_rigidbody;
	public float m_speed;
	public float m_maxSpeed;
	public float m_airResistance;

	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
	}

	void Update(){
		if (Input.GetButton ("Stop")) {
			HardStop ();
		}
	}

	void FixedUpdate(){
		Move ();
		CapOnMaxSpeed ();
	}

	void Move(){
		float h_translation = Input.GetAxisRaw("Horizontal") * m_speed;
		float v_translation = Input.GetAxisRaw("Vertical") * m_speed;


		Vector3 direction = new Vector3(h_translation, v_translation, 0);
		Vector3 to_direction = new Vector3 (0, 0, direction.y);
		
		if (h_translation.Equals (0.0f) && v_translation.Equals (0.0f))
			m_rigidbody.drag = m_airResistance;
		else {
			m_rigidbody.AddForceAtPosition(direction, this.transform.position, ForceMode.Acceleration);
			m_rigidbody.drag = 0.0F;
		}
	}

	void HardStop(){
		m_rigidbody.velocity = Vector3.zero;
	}
	
	void CapOnMaxSpeed(){
		if (m_rigidbody.velocity.magnitude > m_maxSpeed)
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxSpeed;
	}
}
