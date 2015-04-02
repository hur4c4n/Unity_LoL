using UnityEngine;
using System.Collections;

public class HeroWeapon : MonoBehaviour {
	
	private HeroController hero;
	
	void Start(){
		hero = transform.root.GetComponent< HeroController >();
	}
		
	void OnTriggerStay( Collider col ){
		
		if ( col.gameObject.tag == "Enemy" ){
			Enemy enemy = col.gameObject.GetComponent< Enemy >();
			if ( enemy != null )
			enemy.setHeroAttacking( true );
		}
	}
	
}
