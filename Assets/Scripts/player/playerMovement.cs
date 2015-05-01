using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public static playerMovement p_Movement;
	[HideInInspector] public bool isFacingRight = true;
	public float m_speed;

	public void Awake(){
		p_Movement = this;
	}

	public void Move(){
		float h_translation = Input.GetAxisRaw("Horizontal") * m_speed;
		float v_translation = Input.GetAxisRaw("Vertical") * m_speed;

		Vector3 direction = new Vector3(h_translation, v_translation, 0);
		rigidbody.AddForceAtPosition(direction, this.transform.position, ForceMode.Force);
	}

	void SetFacingDirection(float h_translation){
		if (h_translation > 0)
			isFacingRight = true;
		else if (h_translation < 0)
			isFacingRight = false;
	}
}
