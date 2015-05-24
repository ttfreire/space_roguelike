using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	public static playerController p_controller;
	public GameObject frontArm;
	public GameObject backArm;

	public bool isInsideRoom = false;
	public Camera p_camera;
	float cameraSizeinRoom = 15;
	float cameraSizeinSpace = 15;

	public int scrap1Quantity;
	public int damageItemQuantity;
	public bool hasKey = false;
	public bool endlevel = false;
	public int m_level = 1;

	public int currentChunkRow = 0;
	public int currentChunkColumn = 0;

	BoardManager board;
	public string room;

	void Awake(){
		p_controller = this;
	}

	// Use this for initialization
	void Start () {
		scrap1Quantity = 0;
		p_camera = transform.parent.FindChild("PlayerCamera").camera;
		currentChunkColumn = 0;
		currentChunkRow = 0;
		board = FindObjectOfType<BoardManager> ();
	}
	
	void Update () {
		if (Input.GetButtonUp("Fire1"))
			playerShoot.p_Shoot.Shoot ();
		AimAtMouse ();
	}

	void FixedUpdate(){
		playerMovement.p_Movement.Move ();
	}

	void OnCollisionStay(Collision other){
		if (other.gameObject.tag.Equals ("Projectile") && other.gameObject.GetComponent<projectileController> ().m_shooter != gameObject) {
			playerHealth.p_Health.TakeDamage (other.gameObject.GetComponent<projectileController>().m_damage);
			Destroy (other.gameObject);
		}
		if(other.gameObject.tag.Equals ("Enemy"))
			playerHealth.p_Health.TakeDamage (0.05f);

		if (other.gameObject.tag.Equals ("FinalRoom") && hasKey) {
			endlevel = true;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Door")) {
			this.collider.isTrigger = false;
			if (!isInsideRoom) {
				EnterRoom ();
				loadRoom load = other.gameObject.GetComponent<loadRoom>();
				room = (load.m_room < 10) ? "sala" + "0" + load.m_room.ToString() : "sala" + load.m_room.ToString();
				if(!load.isLoaded)
					StartCoroutine(load.loadRoomOnContainerPosition(room));
			} else{
				LeaveRoom ();

			}
		}
		if (other.tag.Equals ("Item")) {
			itemController i_control = other.gameObject.GetComponent<itemController>();
			i_control.PlayerCollectItem(gameObject);
		}
	}

	public void EnterRoom(){
		if (!isInsideRoom) {
			isInsideRoom = true;
			p_camera.orthographicSize = cameraSizeinRoom;
			Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 200);
			gameObject.transform.position = pos;

		}
	}

	public void LeaveRoom(){
		if (isInsideRoom) {
			isInsideRoom = false;
			p_camera.orthographicSize = cameraSizeinSpace;
			Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
			gameObject.transform.position = pos;
			board.gridPositions[board.FindPlayerChunkindex()].SetActive(true);	
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
			backArm.transform.localScale = auxScale;
			frontArm.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -zRotationFront));
			backArm.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -zRotationBack));
		}
		else {
			Vector3 auxScale = transform.localScale;
			auxScale.x = -1;
			auxScale.y = -1;
			frontArm.transform.localScale = auxScale;
			backArm.transform.localScale = auxScale;
			frontArm.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, zRotationFront));
			backArm.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, zRotationBack));
		}

	}


}
