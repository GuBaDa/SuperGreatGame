﻿using UnityEngine;
using System.Collections;

public class WeaponMelee : MonoBehaviour {

	Animator anim;

	public float damage; 

	private bool fire1Btn;
	private bool fire1BtnDown;

	private bool attack;
	private GameObject player;

	// Use this for initialization
	void Awake () {
		// Set control script to right player
		GetComponentInParent<PlayerController>().PlayerControlNr = GetComponentInParent<PlayerScript> ().PlayerControlNr;
		//defince animator
		anim = GetComponent<Animator> ();
		//define parent plaer
		player = transform.parent.gameObject;
	}


	// Update is called once per frame
	void Update () {
		getControls();

		if (fire1BtnDown) {
			anim.SetBool("animSwing", true);
			anim.SetBool("animIdle", false);
		}
		//flip if needed
		transform.localScale = new Vector2 (player.transform.localScale.x, 1);


	}

	public void SetAttackOn (){
		attack = true;
	}
	public void SetAttackOff (){
		attack = false;
		anim.SetBool("animSwing", false);
		anim.SetBool("animIdle", true);
	}

	void OnCollisionEnter2D(Collision2D coll){
		// For players
		if (attack){
			GameObject enemy = coll.gameObject;
			if (enemy.tag == "Player" && enemy != player) {
				PlayerScript playerScript = enemy.GetComponent<PlayerScript>();
				playerScript.Health -= damage;
			} else if (enemy.tag == "Mob") {
				MobScript mobScript = enemy.GetComponent<MobScript>();
				mobScript.Health -= damage;
			}
		}
	}

	void getControls() {

		//get input
		fire1Btn = GetComponentInParent<PlayerController>().Fire1Btn;
		fire1BtnDown = GetComponentInParent<PlayerController>().Fire1BtnDown;
	}

}
