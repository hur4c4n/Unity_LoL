using UnityEngine;
using System.Collections;

public class Skeleton : Enemy {

	private Animator anim;

	//Hash Ids
	private int hashWalk;
	private int hashRun;
	private int hashDamage;
	private int hashFightMode;
	private int hashSwordHeavy;
	private int hashSwordHeavyClip;
	private int hashWalkFightMode;
	private int hashSendDamage;
	private int hashDeath;
	private int hashDamageClip;

	public override void Start(){
		base.Start();
		anim = gameObject.GetComponent< Animator >();	
		hashWalk = Animator.StringToHash( "Walk" );
		hashRun = Animator.StringToHash( "Run" );
		hashDamage = Animator.StringToHash( "Damage" );
		hashFightMode = Animator.StringToHash( "FightMode" );
		hashSwordHeavy = Animator.StringToHash( "SwordHeavy" );
		hashSwordHeavyClip = Animator.StringToHash( "Base Layer.Sword_Heavy" );
		hashWalkFightMode = Animator.StringToHash( "WalkFightMode" );
		hashDeath = Animator.StringToHash( "Base Layer.PreDeath" );
		hashDamageClip = Animator.StringToHash( "Base Layer.Damage" );
	}

	private void AnimationManager(){

		if ( patrolling.isChasing() && !IsDead() ){
			if ( detection.playerDistance < battle.minDistanceAttack ){
				hc.target = transform;
				patrolling.setPathFinder( false );
				anim.SetBool( hashFightMode, true );
				TargetLook();
				battle.delayCounter += Time.deltaTime;				
				if ( battle.delayCounter >= battle.delayBetweenAttacks ){
					anim.SetBool( hashSwordHeavy, true );
					//anim.CrossFade( hashSwordHeavyClip, 0.1f );
					battle.delayCounter = 0;
					Invoke( "cancelSwordHeavy", 0.2f );
				}
			}else{
				patrolling.setPathFinder( true );
				anim.SetBool( hashFightMode, false );
				anim.SetBool( hashWalk, false );
			}
		}
	}

	void cancelSwordHeavy(){
		anim.SetBool( hashSwordHeavy, false );
	}

	public override void getDamage( string side ){
		base.getDamage( side );
		if ( battle.health <= 0 ){ 
			battle.dead = true;
			anim.CrossFade( hashDeath, 0.1f );
		}
	}

	void cancelHit(){
		anim.SetBool ( hashDamage, false );
	}

	public void setWalk(){
		anim.SetBool( hashWalk, true );
	}

	//Called by the WayPoint 
	public void setIdle( float time ){
		patrolling.canStop = true;
		patrolling.delay = time;
		anim.SetBool( hashWalk, false );
	}
	
	public override void setChasingState(){
		base.setChasingState();
		anim.SetBool( hashRun, true );
		Debug.Log( "IM SKELETON!!" );
	}

	public override void Update(){
		base.Update();
		AnimationManager();
		//HitManager();
		HealthBarManager();
		if ( HeroDetected() && patrolling.patrolling ) this.setChasingState();
		if ( ContinuePatroll() ) this.setWalk();
	}

	void OnGUI(){
	//	if ( gameObject.name == "Skeleton18" )
	//	GUI.Box ( new Rect( 0, 0, 200, 50 ), "counter: " + counter );
	}

}
