using UnityEngine;
using System.Collections;

public class EnemyPatrolling{

	public float delay;
	public float counter;
	public bool canStop;
	//public bool returning;
	public bool patrolling;
	//public bool isChasing;
	public iMove patrollingWay;
	public AIPath pathFinder;

	public void setPathFinder( bool are ){
		pathFinder.canMove = are;
		pathFinder.canSearch = are;
	}

	public bool isChasing(){
		return !patrolling;
	}

}

public class EnemyDetection{

	public float playerDistance;
	public float current;
	public float max;
}

public class EnemyBattle{

	public float minDistanceAttack;
    public float minDistanceDamage;
	public float delayBetweenAttacks;
	public float delayCounter;
	public float damageInterval;
	public float damageCounter;
	public float damageTaken;
	public GameObject damageEffect;
	public float health;
	public int index;
	public bool dead;
    public bool storm;
}

public class HealthBar{

	public GameObject nGui;
	public GameObject bar;
	public UISlider slider;
	public UIRoot root;
}

public class Enemy : MonoBehaviour {

	public float maxDetectionDistance;
	public float minDistanceAttack;
    public float minDistanceDamage;
	public float delayBetweenAttacks;

	[ HideInInspector ]
	public HeroController hc;
    [HideInInspector]
    public HeroBattleManager heroBattle;
	public GameObject player;
	public EnemyDetection detection;
	public EnemyPatrolling patrolling;
	public EnemyBattle battle;
	public float damage;
	public GameObject hitEffect;
    public GameObject blockEffect;
	public AudioClip hitSound;
    public AudioClip fallSound;
	public Vector3 hitAdjustmentPosition;
	[ HideInInspector ]
	public bool mouseOver;
	[ HideInInspector ]
	public bool heroAttacking;	
	public GameObject healthBarGUI;
    public Vector3 healthBarAdjustment;
	public HealthBar healthBar;
	public bool food;
	public GameObject item;
	public GameObject[] foods;
	private bool canDropItem;
	//private HighlightableObject glow;
    [HideInInspector]
    public Animator anim;
    public AudioClip blockSound;
    public AudioClip deadSound;
    public AudioClip[] damageSounds;
    public AudioClip[] whooshSounds;

    [ HideInInspector ]
    public int hashFixPos;
    [HideInInspector]
    public ArrayList attacks;
    [HideInInspector]
    public int hashAttack1;
    [HideInInspector]
    public int hashAttack2;
    [HideInInspector]
    public int hashAttack3;
    [HideInInspector]
    public int hashWalk;
    [HideInInspector]
    public int hashRun;
    [HideInInspector]
    public int hashFightMode;
    [HideInInspector]
    public int hashFixHeight;
    [HideInInspector]
    public int hashDeath;

    public Transform weapon;
    private Transform heroHead;
    private Transform head;

    private bool gravity;

	public virtual void Start(){
        gravity = true;
		player = GameObject.Find( "Hero" );
        heroHead = player.transform.FindChild( "Sven Hood" ).FindChild( "Head" );
        head = transform.FindChild( "Sphere001" );
		hc = player.GetComponent< HeroController >();
        heroBattle = player.GetComponent< HeroBattleManager >();
		//glow = GetComponent< HighlightableObject >();
		detection = new EnemyDetection();
		detection.max = maxDetectionDistance;
		patrolling = new EnemyPatrolling();
		patrolling.patrollingWay = GetComponent< iMove >();
		patrolling.pathFinder = GetComponent< AIPath >();
		patrolling.patrolling = true;
		battle = new EnemyBattle();
		battle.minDistanceAttack = minDistanceAttack;
        battle.minDistanceDamage = minDistanceDamage;
		battle.delayBetweenAttacks = delayBetweenAttacks;
		battle.index = 0;
		battle.health = 100;
		battle.damageTaken = damage;
		battle.damageEffect = hitEffect;
		battle.damageInterval = 0.5f;
		canDropItem = true;
        hashFixPos = Animator.StringToHash( "FixPos" );
        hashFixHeight = Animator.StringToHash( "FixHeight" );
        hashDeath = Animator.StringToHash( "Base Layer.Death" );

		if ( healthBarGUI != null ) {
			GameObject bar = ( GameObject )Instantiate( healthBarGUI );
			healthBar = new HealthBar();
			healthBar.bar = bar.transform.Find( "Camera/Anchor/Panel/Progress Bar" ).gameObject;
			healthBar.slider = healthBar.bar.GetComponent< UISlider >();
			healthBar.root = bar.GetComponent< UIRoot >();
			healthBar.root.manualHeight = Screen.height;
		}
	}

	public bool HeroDetected(){

        float ang = 0;
        if ( head != null ) 
            ang = Vector3.Angle( head.transform.forward, heroHead.transform.position - head.transform.position );
        else
            ang = Vector3.Angle( transform.forward, heroHead.transform.position - transform.position );

        if ( detection.playerDistance < detection.max )
            if ( head != null )
                detection.current = Vector3.Distance( head.transform.position, heroHead.transform.position );
            else
                detection.current = Vector3.Distance( transform.position, heroHead.transform.position );
        else
            detection.current = detection.max;

		if ( detection.playerDistance < detection.max && ang < 90 && hc.healthPoints > 0 && nothingBetween() )        
            return true;        
		else
			return false;
	}

    bool nothingBetween()
    {
        RaycastHit hit;
        bool cast = false;
        Ray r = new Ray();
        if ( head != null )
            r = new Ray( head.transform.position, heroHead.transform.position - head.transform.position );
        else
            r = new Ray( transform.position + transform.up, heroHead.transform.position - ( transform.position + transform.up ) );
        if ( detection.current > 0 )
            cast = Physics.Raycast( r, out hit, detection.current );
        Debug.DrawRay( r.origin, r.direction * detection.current, Color.red );
        // Debug.DrawRay( head.transform.position, head.transform.forward );
        if ( cast && hit.collider.tag == "Hero" )
            return true;
        else
            return false;
    }
	
	public virtual void setChasingState(){
		heroBattle.target = transform;
		//patrolling.returning = false;
		//patrolling.isChasing = true;
		patrolling.patrolling = false;
		patrolling.patrollingWay.Stop();
		patrolling.pathFinder.target = player.transform;
		patrolling.setPathFinder( true );
	}

	public bool ContinuePatroll(){
		if ( patrolling.canStop ){
			patrolling.counter += Time.deltaTime;
			if ( patrolling.counter >= patrolling.delay ){
				patrolling.counter = 0;
				patrolling.canStop = false;
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}

	public void TargetLook(){
		transform.rotation = Quaternion.LookRotation( player.transform.position - transform.position );
	}

	public void setHeroAttacking( bool are ){
		heroAttacking = are;
	}

	public bool getHeroAttacking(){
		return heroAttacking;
	}

	public bool IsDead(){
        return battle.health <= 0;
	}
		
	public virtual void getDamage( string side ){        
        goTrail( 0 );                         
  	}

    public virtual void getDamage( string side, float intensity )
    {
        goTrail( 0 );            
    }

    public void playSound( AudioClip clip )
    {
        if ( GetComponent<AudioSource>().isPlaying )
        {
            if ( transform.GetChild( 0 ) )
            {
                transform.GetChild( 0 ).GetComponent<AudioSource>().clip = clip;
                transform.GetChild( 0 ).GetComponent<AudioSource>().Play();
            }else{
                GetComponent<AudioSource>().clip = clip;
                GetComponent<AudioSource>().Play();
            }            
        }else{
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
    }

    public void playSoundFall()
    {
        if ( fallSound != null )
            playSound( fallSound );       
    }

	public virtual void sendDamage( string side ){

        if ( anim.speed == 1 )
        {
            playSound( whooshSounds[ ( int )Random.Range( 0, whooshSounds.Length ) ] );
            if ( Vector3.Distance( player.transform.position, transform.position ) < 1.75f )
            {
                if ( heroBattle.health > 0 )
                {
                    if ( getCurretnAngle() > 90 && getCurretnAngle() < 270 && heroBattle.blocking )
                        player.GetComponent<HeroBattleManager>().getDamage( transform, side, true );
                    else
                        player.GetComponent<HeroBattleManager>().getDamage( transform, side, false );
                }
            }
            GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
            foreach ( GameObject enemy in enemies )
            {
                if ( Vector3.Distance( transform.position, enemy.transform.position ) < 5 && Vector3.Distance( transform.position, enemy.transform.position ) > 0 )
                {
                    this.battle.delayBetweenAttacks = 5;
                    return;
                }
                else
                    this.battle.delayBetweenAttacks = 2.5f;
            }        
        }        
	}
	
	public void HealthBarManager(){
		if ( detection.playerDistance < battle.minDistanceAttack + 2 )	
			healthBar.bar.SetActive( true );	
		else
			healthBar.bar.SetActive( false );
		healthBar.bar.transform.localPosition = Camera.main.WorldToScreenPoint( transform.position + healthBarAdjustment );
		healthBar.bar.transform.localPosition -= new Vector3( 30, 0, 0 );
		healthBar.slider.sliderValue = Mathf.MoveTowards( healthBar.slider.sliderValue, battle.health/100, 1 );
	}

	public void dropItem(){
		if ( canDropItem ){
			canDropItem = false;
			if ( food )
				Instantiate( dropRamdomFood(), transform.position + transform.up, Quaternion.identity );
			else if ( item != null )
				Instantiate( item, transform.position + transform.up, Quaternion.identity );
		}
	}

	private GameObject dropRamdomFood(){
		Random.Range( 0, 1 ); 
		float foodFloat = Random.value;
		Debug.Log( foodFloat );
		if ( foodFloat < 0.14f )
			return foods[ 0 ];
		if ( foodFloat < 0.28f && foodFloat > 0.14f )
			return foods[ 1 ];
		if ( foodFloat < 0.42f && foodFloat > 0.28f )
			return foods[ 2 ];
		if ( foodFloat < 0.56f && foodFloat > 0.42f )
			return foods[ 3 ];
		if ( foodFloat < 0.7f && foodFloat > 0.56f )
			return foods[ 4 ];
		if ( foodFloat < 0.84f && foodFloat > 0.7f )
			return foods[ 5 ];
		if ( foodFloat < 1f && foodFloat > 0.84f )
			return foods[ 6 ];
		return null;
	}

	public float getCurretnAngle(){        
		return Vector3.Angle( transform.forward, player.transform.forward );
	}

    public IEnumerator pauseAnim( float time )
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds( time );
        Time.timeScale = 1f;
    }

    public float test;

    public void AnimationManager()
    {

        if ( patrolling.isChasing() && battle.health > 0 && ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlownAwayStart" ) &&
                                                      !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlownAwayIdle" ) &&
                                                      !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlownAwayLanding" ) &&
                                                      !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "StandUp" ) ) )
        {
            if ( detection.playerDistance < battle.minDistanceAttack && heroBattle.health > 0 && nothingBetween() )
            {                
                hc.target = transform;
                patrolling.setPathFinder( false );
                anim.SetBool( hashFightMode, true );
                battle.delayCounter += Time.deltaTime;
                if ( anim.speed == 1 )
                    TargetLook();
                if ( battle.delayCounter >= battle.delayBetweenAttacks )
                {
                    anim.CrossFade( ( int )attacks[ battle.index ], 0.1f );
                    battle.delayCounter = 0;
                    battle.index++;
                    if ( battle.index >= attacks.Count )
                        battle.index = 0;
                }
            }
            else
            {
                if ( heroBattle.health > 0 ){
                    patrolling.setPathFinder( true );
                }                    
                else
                {
                    patrolling.setPathFinder( false );
                    anim.SetBool( hashRun, false );
                    transform.rotation = Quaternion.identity;
                }
                anim.SetBool( hashFightMode, false );
                anim.SetBool( hashWalk, false );
            }
        }
        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitHardLeft" ) || 
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitLeft" ) || 
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitRight" ) ||
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitMiddle" ) )
            if ( anim.speed == 1 )
                GetComponent<CharacterController>().Move( transform.forward * -anim.GetFloat( hashFixPos ) * Time.timeScale );

        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlownAwayStart" ) ||
           anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlownAwayIdle" ) ||
           anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlownAwayLanding" ) )
            if ( anim.speed == 1 )
            {
                if ( battle.storm ){
                    gravity = false;
                    GetComponent<CharacterController>().Move( transform.up * anim.GetFloat( hashFixPos ) * Time.timeScale );
                }                    
                else
                    GetComponent<CharacterController>().Move( transform.forward * -anim.GetFloat( hashFixPos ) * Time.timeScale );
            }          
        
    }

    void gravityFix()
    {
        GetComponent<CharacterController>().Move( new Vector3( 0, -10, 0 ) );
    }

    public void slowMotion( float time )
    {
        Time.timeScale = time;
    }

    public void standUp( float delay )
    {
        if ( battle.health > 0 )
            StartCoroutine( standUpDelay( delay ) );
    }

    IEnumerator standUpDelay( float delay )
    {
        yield return new WaitForSeconds( delay );
        anim.CrossFade( "StandUp", 0.01f );
    }

    public void setDeath(){
        battle.dead = true;
        anim.CrossFade( hashDeath, 0.1f );        
        playSound( deadSound );
        patrolling.setPathFinder( false );
    }

	public virtual void Update(){       
		detection.playerDistance = Vector3.Distance( transform.position, player.transform.position );		
		battle.damageCounter += Time.deltaTime;
        if ( gravity )
            gravityFix();
        if ( IsDead() )
            heroBattle.target = null;      
	}

    public void cancelGravity()
    {
        gravity = false;
    }

    public void restoreGravity()
    {
        gravity = true;
    }

    public void goTrail( int i )
    {
        if ( weapon.GetComponent<MeleeWeaponTrail>() != null )
        {
            MeleeWeaponTrail trail = weapon.GetComponent<MeleeWeaponTrail>();
            if ( i == 1 )
                trail.Emit = true;
            else
                trail.Emit = false; 
        }        
    }

}
