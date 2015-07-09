using UnityEngine;
using System.Collections;

public class droidController : enemyBaseController {

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
	}

	void Start(){
		GetComponent<enemyShoot> ().shotEnemySound = shootsound;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}


}
