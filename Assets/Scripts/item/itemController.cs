using UnityEngine;
using System.Collections;

public enum ItemType {OXYGEN, SCRAP1, SCRAP2, KEY};

public class itemController : MonoBehaviour {

	public ItemType m_itemType;
	public float m_points;
	public float m_value;
	float m_oxygen;
	int m_scrapQuantity;
	bool m_gotKey = false;

	// Use this for initialization
	void Awake () {
		switch (m_itemType) {
		case ItemType.OXYGEN:
			m_oxygen = m_value;
			break;
		case ItemType.SCRAP1:
			m_scrapQuantity = Mathf.RoundToInt(m_value);
			break;
		case ItemType.SCRAP2:
			m_scrapQuantity = Mathf.RoundToInt(m_value);
			break;
		case ItemType.KEY:
			if(m_value > 0.0)
				m_gotKey = true;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerCollectItem(GameObject player){
		switch (m_itemType) {
		case ItemType.OXYGEN:
			playerHealth p_health = player.GetComponent<playerHealth>();
			p_health.m_currentOxygenValue += m_oxygen;
			break;
		case ItemType.SCRAP1:

			break;
		case ItemType.SCRAP2:

			break;
		case ItemType.KEY:

			break;
		}
		Destroy (gameObject);
	}

	public void EnemyCollectItem(GameObject enemy){
		Destroy (gameObject);
	}
}
