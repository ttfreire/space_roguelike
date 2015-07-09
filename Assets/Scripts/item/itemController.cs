using UnityEngine;
using System.Collections;

public enum ItemType {OXYGEN, AREA, DAMAGE, PIERCING, VELOCITY, SCRAP1};
public enum ItemShape {NONE, TRIANGLE, SQUARE, PENTAGON};

public class itemController : MonoBehaviour {

	public ItemType m_itemType;
	public ItemShape m_itemShape;
	public float m_value;
	float m_oxygen;
	int m_scrapQuantity;
	int m_damageItemQuantity;
	int m_resistanceItemQuantity;
	int m_velocityItemQuantity;
	int m_volumeItemQuantity;
	
	AudioSource collectSound;

	// Use this for initialization
	void Awake () {

		transform.rotation = Quaternion.identity;
		switch (m_itemType) {
		case ItemType.OXYGEN:
			m_oxygen = m_value;
			break;
		case ItemType.DAMAGE:
			m_damageItemQuantity = Mathf.RoundToInt(m_value);
			break;
		}
	}

	void Start(){
		collectSound = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void PlayerCollectItem(GameObject player){
		playerController p_control = player.GetComponent<playerController>();
		collectSound.Play();
		Vector3 newPos = new Vector3 (0, 0, 50);
		switch (m_itemType) {
		case ItemType.OXYGEN:
			playerHealth p_health = player.GetComponent<playerHealth>();
			p_health.m_currentOxygenValue += m_oxygen;
			if(p_health.m_currentOxygenValue > p_health.m_playerTotalOxygen)
				p_health.m_currentOxygenValue = p_health.m_playerTotalOxygen;
			this.gameObject.transform.position = (new Vector3(500, 500, 500));
			break;
		case ItemType.DAMAGE:
			p_control.m_damageItemQuantity++;
			this.gameObject.transform.position = newPos;
			gameController.control.AddItemToPowerUpItemList(this.gameObject);
			break;
		case ItemType.PIERCING:
			p_control.m_resistanceItemQuantity++;
			this.gameObject.transform.position = newPos;
			gameController.control.AddItemToPowerUpItemList(this.gameObject);
			break;
		case ItemType.AREA:
			p_control.m_velocityItemQuantity++;
			this.gameObject.transform.position = newPos;
			gameController.control.AddItemToPowerUpItemList(this.gameObject);
			break;
		case ItemType.VELOCITY:
			p_control.m_volumeItemQuantity++;
			this.gameObject.transform.position = newPos;
			gameController.control.AddItemToPowerUpItemList(this.gameObject);
			break;
		}


		//Destroy (gameObject);

	}

	public void EnemyCollectItem(GameObject enemy){
		Destroy (gameObject);
	}
}
