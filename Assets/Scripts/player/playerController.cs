using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour {
	public static playerController p_controller;
	GameObject body;
	public GameObject frontArm;
	public Sprite frontArmNormal;
	public Sprite frontArmVeloc;
	public Sprite frontArmResist;

	public bool isInsideRoom = true;
	public Camera p_camera;
	float cameraSizeinRoom = 15;
	float cameraSizeinSpace = 15;
	bool paredetocar = false;

	[HideInInspector] public int m_damageItemQuantity;
	[HideInInspector] public int m_resistanceItemQuantity;
	[HideInInspector] public int m_velocityItemQuantity;
	[HideInInspector] public int m_volumeItemQuantity;
	[HideInInspector] public int m_weaponApiece1Quantity;
	[HideInInspector] public int m_weaponApiece2Quantity;
	[HideInInspector] public int m_weaponApiece3Quantity;
	[HideInInspector] public int m_weaponBpiece1Quantity;
	[HideInInspector] public int m_weaponBpiece2Quantity;
	[HideInInspector] public int m_weaponBpiece3Quantity;

	public string room;
	protected Animator animBody;
	protected Animator animFrontArm;
	public bool isMovingBack = false;
	public bool isMovingForward = false;
	public bool isMovingUp = false;
	public bool isMovingDown = false;
	public int powerupType = 0;
	public bool BackToMainTree = false;
	float health;
	public bool hasDiamond = false;
	GameObject rotatingAxis;

	AudioSource[] audios;
	public AudioSource shot;
	public AudioSource lowOxy;
	public AudioSource hit2;
	public AudioSource death;

	void Awake(){
		p_controller = this;
		body = transform.FindChild ("Body").gameObject;
		audios = GetComponents<AudioSource> ();
		shot = audios [0];
		lowOxy = audios [1];
		hit2 = audios [2];
		death = audios [3];
	}

	// Use this for initialization
	void Start () {

		animBody = GetComponent<Animator>();
		animFrontArm = transform.FindChild("Body").GetChild(0).GetComponent<Animator>();
		rotatingAxis = body.transform.GetChild (1).gameObject;
	}
	
	void Update () {
		//BackToMainTree = false;
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING)) {
			rotatingAxis.transform.RotateAround(new Vector3(0,0,1), Time.deltaTime);
			if (Input.GetButtonUp ("Fire1"))
				playerShoot.p_Shoot.Shoot ();
			AimAtMouse ();
		}
			if (animBody != null) {
				animBody.SetBool ("isMovingBack", isMovingBack);
				animBody.SetBool ("isMovingForward", isMovingForward);
				animBody.SetBool ("isMovingUp", isMovingUp);
				animBody.SetBool ("isMovingDown", isMovingDown);
				animBody.SetBool ("BackToMainTree", BackToMainTree);
				animBody.SetFloat ("health", playerHealth.p_Health.m_currentOxygenValue);
				animBody.SetBool ("deathByDamage", playerHealth.p_Health.deathByDamage);
				animBody.SetInteger ("powerupType", powerupType);
				BackToMainTree = false;
			}
		
			if (playerHealth.p_Health.IsDead ()) {
				body.SetActive (false);
				if(!death.isPlaying && !paredetocar){
					death.Play();
					paredetocar = true;
				}
			}
		
	}

	void FixedUpdate(){
		if (gameController.control.m_currentGameState.Equals (GameStates.RUNNING) && gameController.control.canMove)
			playerMovement.p_Movement.Move ();
	}

	void OnCollisionStay(Collision other){
		//if (other.gameObject.tag.Equals ("Projectile") && other.gameObject.GetComponent<projectileController> ().m_shooter != gameObject) {
		//	playerHealth.p_Health.TakeDamage (other.gameObject.GetComponent<projectileController>().m_damage);
		//	Destroy (other.gameObject);
		//}
		if(other.gameObject.tag.Equals ("Enemy"))
			playerHealth.p_Health.TakeDamage (0.1f);

	}

	void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Projectile") && other.gameObject.GetComponent<projectileController> ().m_shooter != gameObject) {
			playerHealth.p_Health.TakeDamage (other.gameObject.GetComponent<projectileController>().m_damage);
			Destroy(other.gameObject);
		}
		if (other.tag.Equals ("Door") || other.tag.Equals ("Porta")) {
			this.collider.isTrigger = false;
			if (!isInsideRoom) {
				EnterRoom ();
				loadRoom load = other.gameObject.GetComponent<loadRoom>();
				room = (load.m_room < 10) ? "sala" + "0" + load.m_room.ToString() : "sala" + load.m_room.ToString();
				if(!load.isLoaded)
					StartCoroutine(load.loadRoomOnContainerPosition(room));
			} else if(other.isTrigger)
				LeaveRoom ();

			if (other.tag.Equals ("Space"))
				playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OxygenLossRate;
			if (other.tag.Equals ("Outer Space"))
				playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OuterSpaceOxygenLossRate;
		}
		if (other.tag.Equals ("Item")) {
			itemController i_control = other.gameObject.GetComponent<itemController>();
			i_control.PlayerCollectItem(gameObject);
		}

		if (other.tag.Equals ("Finish") && hasDiamond) {
			gameController.control.isDiamondCollected = true;
			Destroy(gameController.control.DiamondObject.GetComponent<FixedJoint>());
			gameController.control.DiamondObject.transform.position = other.transform.position;
		}
	}

	public void EnterRoom(){
		if (!isInsideRoom) {
			isInsideRoom = true;
			p_camera.orthographicSize = cameraSizeinRoom;
			Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -200);
			gameObject.transform.position = pos;

		}
	}

	public void LeaveRoom(){
		if (isInsideRoom) {
			isInsideRoom = false;
			p_camera.orthographicSize = cameraSizeinSpace;
			Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
			gameObject.transform.position = pos;
			//board.gridPositions[board.FindPlayerChunkindex()].SetActive(true);	
		}
	}

	void AimAtMouse(){
		Vector3 mousePos = p_camera.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		Vector3 frontArmVector = mousePos-frontArm.transform.position;
		Vector3 backArmVector = mousePos-frontArm.transform.position;
		float zRotationFront = Mathf.Atan2 (frontArmVector.y, frontArmVector.x) * Mathf.Rad2Deg;
		float zRotationBack = Mathf.Atan2 (backArmVector.y, backArmVector.x) * Mathf.Rad2Deg;
		if (playerMovement.p_Movement.isFacingRight) {
			Vector3 auxScale = transform.localScale;
			auxScale.x = 1;
			auxScale.y = 1;
			frontArm.transform.localScale = auxScale;
			frontArm.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -zRotationFront));
		}
		else {
			Vector3 auxScale = transform.localScale;
			auxScale.x = -1;
			auxScale.y = -1;
			frontArm.transform.localScale = auxScale;
			frontArm.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, zRotationFront));
		}

	}

	public void SelectCorrectArmFromPowerUp(int powerup){
		switch (powerup) {
		case 0:
			frontArm.GetComponent<SpriteRenderer>().sprite = frontArmNormal;
			powerupType = 0;
			BackToMainTree = true;
			break;
		case 1:
			frontArm.GetComponent<SpriteRenderer>().sprite = frontArmVeloc;
			powerupType = 1;
			BackToMainTree = true;
			break;
		case 2:
			frontArm.GetComponent<SpriteRenderer>().sprite = frontArmResist;
			powerupType = 2;
			BackToMainTree = true;
			break;
		}
	}

}
