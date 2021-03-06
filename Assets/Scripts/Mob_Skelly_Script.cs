using UnityEngine;
using System.Collections;

public class Mob_Skelly_Script : MonoBehaviour {

	// Public vars
	public float moveSpeed;
	public GameObject psPoisonGlobe;
	public Transform GroundCheck;
	public Transform SideCheck;
	public LayerMask collisionLayer;


	Animator anim;


	//Private vars
	private bool grounded;
	private bool sideClear;
	private float castCooldown;
	private GameObject projectile;
	private GameObject target;




	// Use this for initialization
	void Awake () {
		target = null;
		castCooldown = 0;
		grounded = false;
		sideClear = true;
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		grounded = Physics2D.OverlapArea(GroundCheck.position, new Vector3(GroundCheck.position.x+(.7f*transform.localScale.x),GroundCheck.position.y-.05f,GroundCheck.position.z), collisionLayer);
		sideClear = !Physics2D.OverlapArea(SideCheck.position,new Vector3(SideCheck.position.x+(.1f*transform.localScale.x),SideCheck.position.y-.88f,SideCheck.position.z),collisionLayer);

		if( !sideClear){
			doFlip();
		}

		target = checkForAgro();
		if( grounded && transform.childCount > 2 && target != null ){
			anim.SetBool ("animAttack", true);
			anim.SetBool ("animWalk", false);
			anim.SetBool ("animIdle", false);
			anim.SetBool ("animCast", true);
		}else if( grounded && transform.childCount == 2){
			castCooldown = Time.time + .8f;
			anim.SetBool ("animAttack", false);
			anim.SetBool ("animWalk", false);
			anim.SetBool ("animIdle", false);
			anim.SetBool ("animCast", true);
		} else if(grounded && castCooldown < Time.time){
			doWalk();
			anim.SetBool ("animAttack", false);
			anim.SetBool ("animWalk", true);
			anim.SetBool ("animIdle", false);
			anim.SetBool ("animCast", false);
		} else {
			rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
			anim.SetBool ("animAttack", false);
			anim.SetBool ("animWalk", false);
			anim.SetBool ("animIdle", true);
			anim.SetBool ("animCast", false);
		}
	}

	void doFlip(){
		Vector3 tempScale = new Vector3(transform.localScale.x*-1,transform.localScale.y, transform.localScale.z);
		transform.localScale = tempScale;

	}

	void doWalk(){
		rigidbody2D.velocity = new Vector2(moveSpeed*transform.localScale.x,0);
	}

	void facePlayer(){
		if(target == null){
			Debug.Log("This should not happen! Checkit faget");
		} else if(target.transform.position.x > transform.position.x && transform.localScale.x == -1){
			doFlip();
		} else if (target.transform.position.x < transform.position.x && transform.localScale.x == 1){
			doFlip();
		}
	}

	void castSpell(){
		projectile = (GameObject) Instantiate(psPoisonGlobe,new Vector3(transform.position.x,transform.position.y+1,transform.position.z),Quaternion.identity);
		projectile.transform.parent = transform;
		projectile.rigidbody2D.isKinematic = true;

	}

	void doThrowProjectile(){
		projectile.rigidbody2D.isKinematic = false;
		projectile.transform.parent = null;
		target = null;

	}

	GameObject checkForAgro (){
		GameObject[] playersAvailable = GameObject.FindGameObjectsWithTag ("Player");
		if (playersAvailable.Length != 0){
			GameObject target = findClosestTarget();
			if (Mathf.Abs(target.transform.position.x - transform.position.x) < 3 &&
			    Mathf.Abs(target.transform.position.y - transform.position.y) < 3)
			{
				return target;
			} else {
				return null;
			}
		} else {
			return null;
		}
	}

	GameObject findClosestTarget(){
		//Find and return closest Player
		
		GameObject[] targets;
		targets = GameObject.FindGameObjectsWithTag ("Player");
		GameObject closestTarget = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject tar in targets) {
			Vector3 diff = tar.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closestTarget = tar;
				distance = curDistance;
			}
		}
		return closestTarget;
	}


}
