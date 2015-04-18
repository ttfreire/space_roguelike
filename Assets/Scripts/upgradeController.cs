using UnityEngine;
using System.Collections;
using System.Text;

public class upgradeController : MonoBehaviour {
	public TextAsset upgradeTable;
	string[] parsed;
	char[] separatorchars;
	playerShoot m_pShoot;
	playerController m_pControl;

	// Use this for initialization
	void Awake () {
		separatorchars = new char[]{'\t','\n', '\r'};
		parsed = upgradeTable.text.Split (separatorchars, System.StringSplitOptions.RemoveEmptyEntries);
		m_pShoot = gameObject.GetComponent<playerShoot> ();
		m_pControl = gameObject.GetComponent<playerController> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpgradetoLevel(int level){
		Debug.Log ("level: " + level);
		int item1QuantityIndex = (level-1) * 3;
		int item2QuantityIndex = item1QuantityIndex + 1;
		int upgradeFactorIndex = item2QuantityIndex + 1;

		int item1Quantity = int.Parse(parsed[item1QuantityIndex]);
		int item2Quantity = int.Parse(parsed[item2QuantityIndex]);

		if (item1Quantity <= m_pControl.scrap1Quantity && item2Quantity <= m_pControl.scrap2Quantity) {
			m_pControl.scrap1Quantity -= item1Quantity;
			m_pControl.scrap2Quantity -= item2Quantity;
			m_pShoot.m_damage += float.Parse (parsed [upgradeFactorIndex].Replace (",", "."));
			m_pControl.m_level++;
		}

	}
}
