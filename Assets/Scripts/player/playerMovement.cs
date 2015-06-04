using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public static playerMovement p_Movement;
	public bool isFacingRight = true;
	public float m_speed;
	public float currentSpeed;

	public void Awake(){
		p_Movement = this;
		currentSpeed = m_speed;
	}
	
	public void Move(){
		float h_translation = Input.GetAxisRaw("Horizontal") * currentSpeed;
		float v_translation = Input.GetAxisRaw("Vertical") * currentSpeed;
		Vector3 direction = new Vector3(h_translation, v_translation, 0);

		rigidbody.AddForceAtPosition(direction, this.transform.position, ForceMode.Force);
		DefiningDirectionOfMovement (h_translation, v_translation);
	}

	public void SetFacingDirection(Vector3 mousePos){
		/**
		if (h_translation > 0 && !isFacingRight) {
			transform.LookAt (transform.position - transform.forward);
			isFacingRight = true;
		} else if (h_translation < 0 && isFacingRight) {
			transform.LookAt (transform.position - transform.forward);
			isFacingRight = false;
		}

		**/

		mousePos.z = 0;
		if (mousePos.x > transform.position.x) {
			Vector3 auxScale = transform.localScale;
			auxScale.x = -1;
			transform.localScale = auxScale;
			isFacingRight = true;
		} else {
			Vector3 auxScale = transform.localScale;
			auxScale.x = 1;
			transform.localScale = auxScale;
			isFacingRight = false;
		}
	}

	void DefiningDirectionOfMovement(float horizontalMovement, float verticalMovement){
		// Moving Back
		if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0))
			playerController.p_controller.isMovingBack = true;
		else
			playerController.p_controller.isMovingBack = false;

		// Moving Forward
		if ((isFacingRight && horizontalMovement > 0) || (!isFacingRight && horizontalMovement < 0))
			playerController.p_controller.isMovingForward = true;
		else
			playerController.p_controller.isMovingForward = false;

		// Moving Up
		if (verticalMovement > 0)
			playerController.p_controller.isMovingUp = true;
		else
			playerController.p_controller.isMovingUp = false;

		// Moving Down
		if (verticalMovement < 0)
			playerController.p_controller.isMovingDown = true;
		else
			playerController.p_controller.isMovingDown = false;
	}

}
