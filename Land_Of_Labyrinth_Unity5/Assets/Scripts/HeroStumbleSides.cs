using UnityEngine;
using System.Collections;

public class HeroStumbleSides : MonoBehaviour {

	private HeroStumble hs;
	
	void Start () {
		hs = transform.parent.GetComponent< HeroStumble >();
	}
	
	void Update () {
		if ( hs.isTouchingWall )
			Invoke( "cancelTouch", 0.1f );
	}
	
	void cancelTouch(){
		hs.isTouchingWall = false;	
		hs.rightTouchWall = false;
		hs.leftTouchWall = false;
		hs.frontTouchWall = false;
	}
	
	void OnTriggerStay( Collider col ){
		Debug.Log( col.gameObject.name );
		if ( col.gameObject.tag == "Wall" ){
			hs.isTouchingWall = true;	
			if ( gameObject.name == "HeroLeft" )
				hs.leftTouchWall = true;
			if ( gameObject.name == "HeroRight" )
				hs.rightTouchWall = true;
			if ( gameObject.name == "HeroFront" )
				hs.frontTouchWall = true;
		}
	}
	
	void OnTriggerExit( Collider col ){
		cancelTouch();	
	}
	
}
