using UnityEngine;
using System.Collections;

public class HeroStumble : MonoBehaviour {
	
	[ HideInInspector ]
	public bool isTouchingWall;
	[ HideInInspector ]
	public bool leftTouchWall;
	[ HideInInspector ]
	public bool rightTouchWall;
	[ HideInInspector ]
	public bool frontTouchWall;
	public float raycastDistanceStumble = 0.5f;
	public float raycastDistanceSideStumble = 0.5f;
	private Animator anim;
	private int hashStumble;
	private int hashStumbleSide;
	private HeroController hc;
	private Transform heroLeft;
	private Transform heroRight;
	private float stumbleCounter;
	
	void Start(){
		
		anim = transform.root.GetComponent< Animator >();
		hc = transform.root.GetComponent< HeroController >();
		hashStumble = Animator.StringToHash( "Stumble" );
		hashStumbleSide = Animator.StringToHash( "StumbleSide" );
		heroLeft = transform.FindChild( "HeroLeft" );
		heroRight = transform.FindChild( "HeroRight" );
	}
	
	void Update(){
		
		raycastManager();
		if ( anim.GetBool( hashStumble ) ){
			stumbleCounter += Time.deltaTime;
			hc.stumbling = true;
			if ( stumbleCounter > 1 ){
				stumbleCounter = 0;
				hc.stumbling = false;
				hc.stumblingFront = false;
				anim.SetBool( hashStumble, false );
				//anim.SetFloat( hashStumbleSide, 0 );
			}
		}	
	}
	
	void raycastManager(){
		
		RaycastHit hit;
		Ray rFront = new Ray( transform.position + new Vector3( 0, 0.5f, 0 ), transform.forward );
		Ray rLeft = new Ray( transform.position + new Vector3( 0, 0.5f, 0 ), -transform.right );
		Ray rRight = new Ray( transform.position + new Vector3( 0, 0.5f, 0 ), transform.right );
		Debug.DrawRay( rFront.origin + new Vector3( 0, 0.5f, 0 ), rFront.direction * raycastDistanceStumble , Color.red );
		Debug.DrawRay( rRight.origin + new Vector3( 0, 0.5f, 0 ), rRight.direction * raycastDistanceSideStumble , Color.red );
		Debug.DrawRay( rLeft.origin + new Vector3( 0, 0.5f, 0 ), rLeft.direction * raycastDistanceSideStumble , Color.red );

		if ( Physics.Raycast( rFront, out hit, raycastDistanceStumble ) ){
			if ( hit.collider.tag == "Wall" ){ 
				//stumble front
				hc.stumblingFront = true;
				anim.SetBool( hashStumble, true );
				anim.SetFloat( hashStumbleSide, -0.5f );
			}
		}	
		if ( Physics.Raycast( rLeft, out hit, raycastDistanceSideStumble ) ){
			if ( hit.collider.tag == "Pillar" ){
				//stumble right
				anim.SetBool( hashStumble, true );
				anim.SetFloat( hashStumbleSide, 0.5f );
			}
		}	
		if ( Physics.Raycast( rRight, out hit, raycastDistanceSideStumble ) ){
			if ( hit.collider.tag == "Pillar" ){
				//stumble left
				anim.SetFloat( hashStumbleSide, -1.5f );
				anim.SetBool( hashStumble, true );	
			}
		}	

	}
	
}
