using UnityEngine;
using System.Collections;

public class externalDoorController : MonoBehaviour {
	Animator anim;
	bool isClosed = true;
	public Sprite closedSprite;
	public Sprite openedSprite;
	SpriteRenderer doorSprite;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		doorSprite = GetComponent<SpriteRenderer> ();
		doorSprite.sprite = closedSprite;
	}
	
	// Update is called once per frame
	void Update () {
		if(isClosed)
			doorSprite.sprite = closedSprite;
		else
			doorSprite.sprite = openedSprite;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Player")) {
			anim.SetBool ("closeToDoor", true);
			isClosed = false;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag.Equals ("Player")) {
			anim.SetBool ("closeToDoor", false);
			isClosed = true;
		}
	}
}
