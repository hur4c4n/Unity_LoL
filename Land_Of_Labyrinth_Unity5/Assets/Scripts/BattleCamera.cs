using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour {

	//private GameObject hero;
	private HeroController hc;
	private int hashFightMode;
	private Animator anim;

	void Start(){
		//hero = GameObject.Find( "Hero" );
		hc = GetComponent< HeroController >();
		hashFightMode = Animator.StringToHash( "FightMode" );
		anim = GetComponent< Animator >();
	}
	
	void Update () {
		
		if ( anim.GetBool( hashFightMode ) ){
			//transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.LookRotation( ( hc.target.position + new Vector3( 0, 1, 0 ) - transform.position ) ), Time.deltaTime * 2 );
			Camera.main.fieldOfView = Mathf.Lerp( Camera.main.fieldOfView, 90, 0.1f );
		}else{
			//transform.localRotation = Quaternion.Lerp( transform.localRotation, Quaternion.Euler( Vector3.zero ), Time.deltaTime * 2 );
			Camera.main.fieldOfView = Mathf.Lerp( Camera.main.fieldOfView, 60, 1 );
		}
	}
}
