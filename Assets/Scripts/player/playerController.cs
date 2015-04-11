using UnityEngine;
using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	playerHealth m_pHealth;
	bool IsInsideRoom = false;
	Camera p_camera;
	float cameraSizeinRoom = 10;
	float cameraSizeinSpace = 15;

	// Use this for initialization
	void Awake () {
		p_camera = transform.GetChild (0).camera;
		m_pHealth = gameObject.GetComponent<playerHealth> ();
	}
	
	// Update is called once per frame
	void Update () {

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
				Application.LoadLevelAdditive ("sala02");
			} else
				LeaveRoom ();
		} else if (other.tag.Equals ("Item")) {
			itemController i_control = other.gameObject.GetComponent<itemController>();
			i_control.PlayerCollectItem(gameObject);
		}
	}

	public void EnterRoom(){
		if (!IsInsideRoom) {
			IsInsideRoom = true;
			p_camera.orthographicSize = cameraSizeinRoom;
			Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 50);
			gameObject.transform.position = pos;
		}
	}

	public void LeaveRoom(){
		if (IsInsideRoom) {
			IsInsideRoom = false;
			GameObject sceneRoot = GameObject.Find("_ROOT");
			Destroy(sceneRoot);
			p_camera.orthographicSize = cameraSizeinSpace;
			Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
			gameObject.transform.position = pos;
		}
	}

}
