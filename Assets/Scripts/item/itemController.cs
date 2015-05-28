using UnityEngine;
using System.Collections;

public enum ItemType {OXYGEN, SCRAP1, SCRAP2, DAMAGE, RESISTANCE, VOLUME, VELOCITY};

public class itemController : MonoBehaviour {

	public ItemType m_itemType;
	public float m_points;
	public float m_value;
	float m_oxygen;
	int m_scrapQuantity;
	int m_damageItemQuantity;
	int m_resistanceItemQuantity;
	int m_velocityItemQuantity;
	int m_volumeItemQuantity;
	


	// Use this for initialization
	void Awake () {
		switch (m_itemType) {
		case ItemType.OXYGEN:
			m_oxygen = m_value;
			break;
		case ItemType.SCRAP1:
			m_scrapQuantity = Mathf.RoundToInt(m_value);
			break;
		case ItemType.DAMAGE:
			m_damageItemQuantity = Mathf.RoundToInt(m_value);
			break;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void PlayerCollectItem(GameObject player){
		playerController p_control = player.GetComponent<playerController>();
		switch (m_itemType) {
		case ItemType.OXYGEN:
			playerHealth p_health = player.GetComponent<playerHealth>();
			p_health.m_currentOxygenValue += m_oxygen;
			break;
		case ItemType.SCRAP1:
			p_control.scrap1Quantity++;
			//gameController.control.scrap1.text = p_control.scrap1Quantity.ToString();
			break;
		case ItemType.DAMAGE:
			p_control.m_damageItemQuantity++;
			break;
		case ItemType.RESISTANCE:
			p_control.m_resistanceItemQuantity++;
			break;
		case ItemType.VOLUME:
			p_control.m_velocityItemQuantity++;
			break;
		case ItemType.VELOCITY:
			p_control.m_volumeItemQuantity++;
			break;
		}
		Destroy (gameObject);
	}

	public void EnemyCollectItem(GameObject enemy){
		Destroy (gameObject);
	}
}
