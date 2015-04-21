using UnityEngine;
using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	playerHealth m_pHealth;
	public bool IsInsideRoom = false;
	Camera p_camera;
	float cameraSizeinRoom = 10;
	float cameraSizeinSpace = 15;

	public int scrap1Quantity;
	public int scrap2Quantity;
	public bool hasKey = false;
	public int m_level = 1;
	// Use this for initialization
	void Start () {
		scrap1Quantity = 0;
		scrap2Quantity = 0;
		p_camera = transform.GetChild (0).camera;
		m_pHealth = gameObject.GetComponent<playerHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasKey) {
			//Time.timeScale = 0;
			gameController.control.victory.text = "Victory!";
			gameController.control.KillAllEnemies();
		}
		if(m_pHealth.IsDead()){
			//Time.timeScale = 0;
			gameController.control.victory.text = "Defeat!";
		}
	}

	void OnCollisionStay(Collision other){
		if (other.gameObject.tag.Equals ("Projectile") && other.gameObject.GetComponent<projectileController> ().m_shooter != gameObject) {
			m_pHealth.TakeDamage (10.0f);
			Destroy (other.gameObject);
		}
		if(other.gameObject.tag.Equals ("Enemy"))
			m_pHealth.TakeDamage (0.05f);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Door")) {
			this.collider.isTrigger = false;
			if (!IsInsideRoom) {
				EnterRoom ();
				loadRoom load = other.gameObject.GetComponent<loadRoom>();
				if(!load.isLoaded){
					load.isLoaded = true;
					string roomScene = (load.m_room < 10) ? "sala" + "0" + load.m_room.ToString() : "sala" + load.m_room.ToString();
					StartCoroutine(load.loadRoomOnContainerPosition(roomScene));
				}
			} else
				LeaveRoom ();
		}
		if (other.tag.Equals ("Item")) {
			itemController i_control = other.gameObject.GetComponent<itemController>();
			i_control.PlayerCollectItem(gameObject);
		}
	}

	public void EnterRoom(){
		if (!IsInsideRoom) {
			IsInsideRoom = true;
			p_camera.orthographicSize = cameraSizeinRoom;
			Vector3 pos = new Vector3(gameObject.transform.position.x+10, gameObject.transform.position.y, 200);
			gameObject.transform.position = pos;
		}
	}

	public void LeaveRoom(){
		if (IsInsideRoom) {
			IsInsideRoom = false;
			//GameObject sceneRoot = GameObject.Find("_ROOT");
			//Destroy(sceneRoot);
			p_camera.orthographicSize = cameraSizeinSpace;
			Vector3 pos = new Vector3(gameObject.transform.position.x-10, gameObject.transform.position.y, 0);
			gameObject.transform.position = pos;
		}
	}

}
