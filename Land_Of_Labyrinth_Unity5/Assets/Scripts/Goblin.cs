using UnityEngine;
using System.Collections;

public class Goblin : Enemy {

	private int hashDamage;
	private int hashDamageClip;
	private int hashQuickDamageClip;
	private int hashQuickHit;	
	private int hashSendDamage;
    private int hashRandomIdle;
    private int hashCanHit;        
	private EnemyWeapon goblinWeapon;    

	public override void Start() {

		base.Start();                
		anim = gameObject.GetComponent< Animator >();	
		goblinWeapon = transform.GetChild( 0 ).GetComponent< EnemyWeapon >();		
		hashDamage = Animator.StringToHash( "Damage" );
		hashDamageClip = Animator.StringToHash( "Base Layer.Hit" );
		hashQuickDamageClip = Animator.StringToHash( "Base Layer.QuickHit" );
		hashQuickHit = Animator.StringToHash( "QuickHit" );		
		hashSendDamage = Animator.StringToHash( "SendDamage" );		
        hashCanHit = Animator.StringToHash( "CanHit" );
        hashRandomIdle = Animator.StringToHash( "RandomIdle" );
        hashAttack1 = Animator.StringToHash( "Base Layer.Attack1" );
        hashAttack2 = Animator.StringToHash( "Base Layer.Attack2" );
        hashAttack3 = Animator.StringToHash( "Base Layer.Attack3" );
        hashWalk = Animator.StringToHash( "Walk" );
        hashRun = Animator.StringToHash( "Run" );        
        attacks = new ArrayList();
        attacks.Add( hashAttack1 );
        attacks.Add( hashAttack2 );
        attacks.Add( hashAttack3 );
        anim.Play( "Idle", 0, Random.Range( 0f, 1f ) );
	}
	void ChangeHeroDamageAnimation(){
		switch ( battle.index ){
		case 0:
			goblinWeapon.heroDamage = heroDamages.front;
			break;
		case 1:
			goblinWeapon.heroDamage = heroDamages.front;
			break;
		case 2:
			goblinWeapon.heroDamage = heroDamages.left;
			break;
		}
	}
		
	public void setWalk(){
		anim.SetBool( hashWalk, true );
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
	}

	//Called by the WayPoint 
	public void setIdle( float time ){
		patrolling.canStop = true;
		patrolling.delay = time;
		anim.SetBool( hashWalk, false );		
	}

	public override void setChasingState(){
		base.setChasingState();
        anim.CrossFade( "Alert", 0.01f );
		anim.SetBool( hashRun, true );
		Debug.Log( "IM GOBLIN!!" );
	}

	public override void getDamage( string side, float intensity )
    {
		base.getDamage( side );
        if ( battle.health > 0 )
        {          
            if ( Random.Range( 0f, 1f ) > 0.7f && getCurretnAngle() < 90 && getCurretnAngle() > 270 ) 
            {
                anim.CrossFade( "Block", 0.01f );
                playSound( blockSound );
                GameObject clone = ( GameObject )Instantiate( blockEffect, transform.position + hitAdjustmentPosition, Quaternion.identity );
                Destroy( clone, 0.5f );
            }
            else
            {
                battle.health -= battle.damageTaken;
                if ( battle.damageEffect != null )
                {
                    GameObject clone = ( GameObject )Instantiate( battle.damageEffect, transform.position + hitAdjustmentPosition, Quaternion.identity );
                    Destroy( clone, 0.5f );
                }
                if ( battle.health <= 0 )
                {
                    battle.dead = true;
                    anim.CrossFade( hashDeath, 0.1f );
                    playSound( damageSounds[ ( int )Random.Range( 0, damageSounds.Length ) ] );
                    playSound( deadSound );
                    patrolling.setPathFinder( false );
                }
                else
                {
                    setChasingState();
                    playSound( damageSounds[ ( int )Random.Range( 0, damageSounds.Length ) ] );
                    if ( anim.GetFloat( hashCanHit ) == 0 )
                    {
                        switch ( side )
                        {
                            case "left":
                                if ( intensity == 0 )
                                    anim.CrossFade( "HitLeft", 0.01f );
                                else
                                    anim.CrossFade( "HitHardLeft", 0.01f );
                                break;
                            case "right":
                                anim.CrossFade( "HitRight", 0.01f );
                                break;
                            case "middle":
                                if ( intensity == 0 )
                                    anim.CrossFade( "HitMiddle", 0.01f );
                                else
                                    anim.CrossFade( "HitHardMiddle", 0.01f );
                                break;
                            case "leftHard":
                                anim.CrossFade( "HitMiddle", 0.01f );
                                break;
                        }
                    }
                }
            }		
        }
	}

	void cancelQuickHit(){
		anim.SetBool ( hashQuickHit, false );
	}

	public override void sendDamage( string heroSide ){
		base.sendDamage( heroSide );
	}
	
	public override void Update(){
		base.Update();        
		AnimationManager();
		HealthBarManager();
		if ( HeroDetected() && patrolling.patrolling ) this.setChasingState();
		if ( ContinuePatroll() ) this.setWalk();
        //if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitMiddle" ) && anim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime < 0.75f )
            //GetComponent<CharacterController>().Move( ( -transform.forward + transform.right ) * 0.01f );
	}

	void OnGUI(){
	//	if (GUI.Button (new Rect (0, 0, 200, 100), "HIT"))
	//	anim.CrossFade (hashDamageClip, 0.1f);
	}




}
