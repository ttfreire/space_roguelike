using UnityEngine;
using System.Collections;

public class MineAnimation : MonoBehaviour {
	private Animator animator;
		
	void Awake(){
		animator = GetComponent <Animator>();}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals("Player"))
		{animator.SetBool("IsNear",true);}
	}

}
