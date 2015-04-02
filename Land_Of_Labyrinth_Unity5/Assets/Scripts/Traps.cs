using UnityEngine;
using System.Collections;

public class Traps : MonoBehaviour {
	
	public bool groundTrap;
	
	private GameObject hero;
	private HeroController hc;
	private GameObject heroLeft;
	private GameObject heroRight;
	private float distanceHeroLeft;
	private float distanceHeroRight;
	private Animator anim;
	private float stumbleCounter;
	private float stumbleCounterTime = 1.5f;
	
	private int hashStumble;
	private int hashStumbleSide;
	
	void Start(){
		hero = GameObject.FindGameObjectWithTag( "Hero" );
		heroLeft = hero.transform.FindChild( "HeroSides/HeroLeft" ).gameObject;
		heroRight = hero.transform.FindChild( "HeroSides/HeroRight" ).gameObject;
		hc = hero.GetComponent< HeroController >();
		anim = hero.GetComponent< Animator >();
		hashStumble = Animator.StringToHash( "Stumble" );
		hashStumbleSide = Animator.StringToHash( "StumbleSide" );
	}
	
	void Update(){
		
		if ( anim.GetBool( hashStumble ) ){
			stumbleCounter += Time.deltaTime;
			hc.stumbling = true;
			if ( stumbleCounter > anim.GetCurrentAnimatorStateInfo( 0 ).length ){
				stumbleCounter = 0;
				hc.stumbling = false;
				hc.stumblingFront = false;
				anim.SetBool( hashStumble, false );
				anim.SetFloat( hashStumbleSide, 0 );
			}
		}
	}
	
	void OnTriggerStay( Collider col ){
		
		distanceHeroLeft = Vector3.Distance( transform.position, heroLeft.transform.position );
		distanceHeroRight = Vector3.Distance( transform.position, heroRight.transform.position );
		
		if ( col.transform.root.tag  == "Hero" && !anim.GetBool( hashStumble ) ){
			if ( groundTrap ){
				hc.stumblingFront = true;
				anim.SetBool( hashStumble, true );
				anim.SetFloat( hashStumbleSide, 1.5f );
			}else{
				if ( distanceHeroLeft > distanceHeroRight && ( distanceHeroLeft - distanceHeroRight ) > 0.2f ){
					//stumble left	
					anim.SetBool( hashStumble, true );
					anim.SetFloat( hashStumbleSide, -1.5f );
				}else if ( distanceHeroRight > distanceHeroLeft && ( distanceHeroRight - distanceHeroLeft ) > 0.2f ){
					//stumble right
					anim.SetBool( hashStumble, true );
					anim.SetFloat( hashStumbleSide, 0.5f );
				}else{
					//stumble front
					hc.stumblingFront = true;
					anim.SetBool( hashStumble, true );
					anim.SetFloat( hashStumbleSide, -0.5f );	
				}
			}
		}
	}
	
}
