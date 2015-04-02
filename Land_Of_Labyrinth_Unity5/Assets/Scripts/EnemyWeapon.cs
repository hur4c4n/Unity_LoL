using UnityEngine;
using System.Collections;

public enum heroDamages{
	left,
	front,
	right
}

public class EnemyWeapon : MonoBehaviour {

	private int hashSendDamage;
	private Animator anim;
	private GameObject hero;
	//private HeroController hc;
	private float heroDamageValue;

	public GameObject parent;
	public heroDamages heroDamage;

	void Start(){
		hero = GameObject.Find( "Hero" );
		//hc = hero.GetComponent< HeroController >();
		anim = parent.GetComponent< Animator >();	
		hashSendDamage = Animator.StringToHash( "SendDamage" );
		switch ( heroDamage ){
		case heroDamages.front:
			heroDamageValue = 0.5f;
			break;
		case heroDamages.left:
			heroDamageValue = 0f;
			break;
		case heroDamages.right:
			heroDamageValue = 1;
			break;
		}
	}
	
	void OnTriggerStay( Collider col ){
		if ( col.gameObject.tag == "Hero" || col.gameObject.tag == "HeroDetectCollider" ){
			//if ( anim.GetFloat( hashSendDamage ) > 0 ){
			//	hc.setEnemyIsAttacking( true, heroDamageValue );
			//}else
			//	hc.setEnemyIsAttacking( false );
		}else{
			//hc.setEnemyIsAttacking( false );
		}
	}
	
	void OnGUI(){
		
		//if ( anim.GetFloat( hashSendDamage ) > 0 )
		//GUI.Box( new Rect( 0, 0, 100, 20 ), "effective!" );	
		//if ( curCollider )
		//GUI.Box( new Rect( 0, 120, 150, 20 ), "col " + curCollider.gameObject.name );
	}
		
	
}
