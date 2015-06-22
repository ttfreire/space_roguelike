using UnityEngine;
using System.Collections;

public class chunkManager : MonoBehaviour {
	public enum chunkType {SPACE, OUTERSPACE};
	public chunkType chunktype;
	public bool revealedByPlayer;
	// Use this for initialization
	void Start () {
		revealedByPlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other){
		if (other.tag.Equals ("Player")) {
			revealedByPlayer = true;
			if(chunktype == chunkType.SPACE){
				playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OxygenLossRate; 
				Debug.Log ("Space");
			}
				else if (chunktype == chunkType.OUTERSPACE){
				playerHealth.p_Health.m_currentOxygenLossRate = playerHealth.p_Health.m_OuterSpaceOxygenLossRate; 
				Debug.Log ("Outer Space");
			}
		}
	}
}
