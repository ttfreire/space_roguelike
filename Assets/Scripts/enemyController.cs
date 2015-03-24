using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {
	public GameObject projectile;
	public float shootForce;
	public float shootsPerSecond;
	float shootCooldown;
	Transform targetToShoot;
	bool isPlayerOnView = false;
	// Use this for initialization
	void Start () {
		shootCooldown = 1 / shootsPerSecond;
	}
	
	// Update is called once per frame
	void Update () {
		shootCooldown -= Time.deltaTime;
		if (shootCooldown <= 0.0f && isPlayerOnView) {
			shootCooldown = 1 / shootsPerSecond;
			ShootTarget (targetToShoot.transform);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag.Equals ("Player")) {
			isPlayerOnView = true;
			targetToShoot = other.transform;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag.Equals ("Player"))
			isPlayerOnView = false;
	}

	void ShootTarget(Transform target){
		GameObject proj = (GameObject)Instantiate (projectile, this.transform.position, Quaternion.identity);
		Vector3 shootDir = (target.position - transform.position).normalized;
		proj.transform.TransformDirection (shootDir);
		proj.GetComponent<Rigidbody> ().AddForce (shootDir * shootForce, ForceMode.Impulse);
	}
}
