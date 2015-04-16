using UnityEngine;
using System.Collections;

public class Ogre : Enemy {
   
    private int hashHitRight;
    private int hashHitLeft;
    private int hashHitMiddle;    
    public GameObject dust;
    public AudioClip[] dustSounds;

    public override void Start()
    {
        base.Start();
        anim = gameObject.GetComponent<Animator>();        
        hashWalk = Animator.StringToHash( "Walk" );
        hashRun = Animator.StringToHash( "Run" );
        hashFightMode = Animator.StringToHash( "FightMode" );              
        hashAttack1 = Animator.StringToHash( "Base Layer.StepBackAttack1" );
        hashAttack2 = Animator.StringToHash( "Base Layer.Attack2" );
        hashAttack3 = Animator.StringToHash( "Base Layer.StepBackAttack3" );
        attacks = new ArrayList();
        attacks.Add( hashAttack1 );
        attacks.Add( hashAttack2 );
        attacks.Add( hashAttack3 );
    }
    
    public void setWalk()
    {
        anim.SetBool( hashWalk, true );
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    //Called by the WayPoint 
    public void setIdle( float time )
    {
        patrolling.canStop = true;
        patrolling.delay = time;
        anim.SetBool( hashWalk, false );        
    }

    public override void setChasingState()
    {
        base.setChasingState();
        anim.SetBool( hashRun, true );
        Debug.Log( "IM OGRE!!" );
    }

    public override void getDamage( string side, float intensity )
    {
        base.getDamage( side );        
        if ( battle.health > 0 )
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
                switch ( side )
                {
                    case "left":
                        anim.CrossFade( "HitLeft", 0.01f );
                        break;
                    case "right":
                        anim.CrossFade( "HitRight", 0.01f );
                        break;
                    case "middle":
                        anim.CrossFade( "HitMiddle", 0.01f );
                        break;
                }
            }            
        }
    }

    public override void sendDamage( string heroSide )
    {
        base.sendDamage( heroSide );
    }

    public override void Update()
    {
        base.Update();
        AnimationManager();
        HealthBarManager();
        if ( HeroDetected() && patrolling.patrolling ) this.setChasingState();
        if ( ContinuePatroll() ) this.setWalk();

        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Run" ) && !anim.IsInTransition( 0 ) )
            patrolling.pathFinder.canMove = true;
        else
            patrolling.pathFinder.canMove = false;
        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) ||
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) ||
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack3" ) )
             if ( anim.speed == 1 )
                GetComponent<CharacterController>().Move( transform.forward * anim.GetFloat( hashFixPos ) * Time.timeScale );
        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "StepBackAttack1" ) ||
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "StepBackAttack2" ) ||
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "StepBackAttack3" ) )
            if ( anim.speed == 1 )
                GetComponent<CharacterController>().Move( transform.forward * -0.02f * Time.timeScale );
    }

    public void goDust()
    {
        dust.gameObject.SetActive( false );
        dust.gameObject.SetActive( true );
        playSound( dustSounds[ Random.Range( 0, 1 ) ] );
        Camera.main.GetComponent<Animator>().Play( "Shake" );
    }

}
