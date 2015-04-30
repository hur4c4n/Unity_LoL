using UnityEngine;
using System.Collections;

public class HeroBattleManager : MonoBehaviour {

    public Transform target;
    private Animator anim;
    private Enemy enemySendDamage;
    public AudioClip[] whooshSounds;
    public AudioClip[] damageSounds;
    public AudioClip blockSound;
    public AudioClip gruntSound;
    public AudioClip deathSound;
    public GameObject blood;
    public GameObject blockImpact;
    [ HideInInspector ]
    public bool blocking;
    public UIProgressBar healthBar;
    public UISprite manaBar;
    public float health = 100;
    private HeroController hc; 

    private int hashBattleMode;
    private int hashBlock;
    private int hashTired;
    private int hashTwoHands;
    private int hashQuickAttack;
    private int hashComboIndex;
    private int hashFixpos;

    private bool twoHands;
    public UILabel currentSword;
    public UILabel currentElemental;

    public GameObject[] lighting;
    public GameObject ice;
    public GameObject fire;
    public GameObject storm;
    private float initialCameraView;

    private float endOfBattleCounter;

    public Transform handSwords;
    public Transform backSwords;
    private int elementalIndex;
    private int blastIndex;

	void Start () {
        anim = GetComponent<Animator>();
        hc = GetComponent<HeroController>();
        hashBattleMode = Animator.StringToHash( "BattleMode" );
        hashBlock = Animator.StringToHash( "Blocking" );
        hashTired = Animator.StringToHash( "Tired" );
        hashTwoHands = Animator.StringToHash( "TwoHands" );
        hashQuickAttack = Animator.StringToHash( "QuickAttack" );
        hashComboIndex = Animator.StringToHash( "ComboIndex" );
        hashFixpos = Animator.StringToHash( "FixPos" );
        initialCameraView = Camera.main.fieldOfView;
        handSwords = transform.GetChild( 0 ).GetChild( 0 );
        backSwords = transform.GetChild( 5 ).GetChild( 0 );
        blastIndex = Animator.StringToHash( "ElementalIndex" );
	}

	void Update () {        
        if ( Input.GetButtonDown( "Fire1" ) )
        {
            if ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitMiddle" ) &&
                 !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitLeft" ) &&
                 !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitRight" ) )
            {
                if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) )
                {
                    if ( twoHands )
                    {
                        int random = ( int )Random.Range( 0, 4 );
                        switch ( random )
                        {
                            case 0:
                                anim.SetFloat( hashComboIndex, 1 );
                                break;
                            case 1:
                                anim.SetFloat( hashComboIndex, 0.4f );
                                break;
                            case 2:
                                anim.SetFloat( hashComboIndex, 0.6f );
                                break;
                            case 3:
                                anim.SetFloat( hashComboIndex, 0.8f );
                                break;
                        }
                    }else
                        anim.SetFloat( hashComboIndex, 0 );
                    
                }                
                if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) )
                     anim.SetTrigger( "Combo" );
                else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) && anim.GetFloat( hashComboIndex ) != 0.6f )
                     anim.SetTrigger( "Combo2" );                    
                else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) ||
                          anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "WalkBattle" ) ||
                          anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlockIdle" ) ||
                          anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlockToStand" ) )
                    anim.CrossFade( "Attack1", 0.01f );                                                    
            }            
        }   
    
        if ( Input.GetKeyDown( KeyCode.Q ) ){
            //if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Idle" ) ||
            //     anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Run" ) ||
            //     anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Walk" ) ||
            //     anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Roll" ) ||
            //     anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "RunStop" ) )
            //{
                anim.SetBool( hashBattleMode, !anim.GetBool( hashBattleMode ) );
            //}else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) ) 
            //    anim.SetBool( hashBattleMode, false );                                                          
        }

        if ( Input.GetMouseButtonDown( 1 ) ){
            if ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) &&
                 !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) &&
                 !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack3" ) ){
                     anim.CrossFade( "StandToBlock", 0.01f );
            }   
        }             
        if ( Input.GetMouseButton( 1 ) ) 
        {
            blocking = true;
            anim.SetBool( hashBlock, true );
        }else{
            blocking = false;
            anim.SetBool( hashBlock, false );        
        }
        if ( Input.GetKeyDown( KeyCode.Tab ) )
        {
            twoHands = !twoHands;                   
            if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) || anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "WalkBattle" ) )
                anim.CrossFade( "HideSword", 0.01f );
        }

        if ( Input.GetKeyDown( KeyCode.E ) )
        {
            if ( elementalIndex < handSwords.childCount - 1 )
                elementalIndex++;
            else
                elementalIndex = 0;
            switch ( elementalIndex )
            {
                case 0:
                    currentElemental.text = "Elemental: None";                    
                    break;
                case 1:
                    currentElemental.text = "Elemental: Lighting";
                    anim.SetFloat( blastIndex, 0 );
                    break;
                case 2:
                    currentElemental.text = "Elemental: Ice";
                    anim.SetFloat( blastIndex, 0.3f );
                    break;
                case 3:
                    currentElemental.text = "Elemental: Fire";
                    anim.SetFloat( blastIndex, 0.6f );
                    break;
                case 4:
                    currentElemental.text = "Elemental: Storm";
                    anim.SetFloat( blastIndex, 1f );
                    break;
            }
            for ( int i = 0; i < handSwords.childCount; i++ )
                handSwords.GetChild( i ).gameObject.SetActive( false );                    
            for ( int i = 0; i < backSwords.childCount; i++ )
                backSwords.GetChild( i ).gameObject.SetActive( false );
            handSwords.GetChild( elementalIndex ).gameObject.SetActive( true );
            backSwords.GetChild( elementalIndex ).gameObject.SetActive( true );
                           
        }   
            

        /*if ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) && 
             !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) &&
             !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "DrawSword" ) && 
             !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HideSword" ) &&            
             !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlockToStand" ) &&
             !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlockIdle" ) &&
             !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "StandToBlock" ))
        {*/
            if ( twoHands )
            {
                currentSword.text = "Weapon: Sword";
                if ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HideSword" ) &&
                     !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) )
                     anim.SetFloat( hashTwoHands, Mathf.MoveTowards( anim.GetFloat( hashTwoHands ), 1, 0.1f ) );
            }else{
                currentSword.text = "Weapon: Dagger";
                if ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HideSword" ) &&
                     !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) )
                     anim.SetFloat( hashTwoHands, Mathf.MoveTowards( anim.GetFloat( hashTwoHands ), 0, 0.1f ) );
            }              

        //}
        
        if ( Input.GetKeyDown( KeyCode.B ) && 
            anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) && 
            anim.GetFloat( hashTwoHands ) == 1 &&
            manaBar.fillAmount == 1 &&
            elementalIndex > 0 )
            anim.CrossFade( "Blast", 0.01f );
        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Blast" ) ){
            Camera.main.fieldOfView = Mathf.Lerp( Camera.main.fieldOfView, 80, Time.deltaTime * 3 );               
        }else{
            Camera.main.fieldOfView = Mathf.Lerp( Camera.main.fieldOfView, initialCameraView, Time.deltaTime * 3 ); ;            
        }
        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) || 
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) || 
             anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack3" ) )
        GetComponent<CharacterController>().Move( transform.forward * anim.GetFloat( hashFixpos ) );

        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BattleIdle" ) )
            endOfBattleCounter += Time.deltaTime;
        else
            endOfBattleCounter = 0;

        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlockIdle" ) && 
            Input.GetMouseButtonDown( 0 ) && anim.IsInTransition( 0 ) == false )
            anim.CrossFade( "Kick", 0.01f );

        manaBar.fillAmount = Mathf.MoveTowards( manaBar.fillAmount, 1, 0.001f );

	}

    IEnumerator changeWeaponType()
    {        
        while( true ){
            if ( !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HideSword" ) )
            {
                twoHands = !twoHands;
                yield return 0;
            }
        }     
        yield return 0;            
    }

    public void playSound( AudioClip clip )
    {
        if ( GetComponent<AudioSource>().isPlaying )
        {
            transform.GetChild( 0 ).GetComponent<AudioSource>().clip = clip;
            transform.GetChild( 0 ).GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
    }

    public void playWhoosh()
    {
        playSound( whooshSounds[ ( int )Random.Range( 0, whooshSounds.Length ) ] );
    }

    public void sendDamage( string side )
    {        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach ( GameObject enemy in enemies )
        {
            enemySendDamage = enemy.GetComponent<Enemy>();
            if ( Vector3.Distance( transform.position, enemy.transform.position ) < enemySendDamage.minDistanceDamage )
            {
                if ( getAngle( enemy.transform ) < 60 && getAngle( enemy.transform ) < 300 ) 
                {                    
                    if ( enemySendDamage != null )
                    {
                        enemySendDamage.getDamage( side, anim.GetFloat( hashTwoHands ) );
                        //StartCoroutine( pauseAnim( enemy.transform ) );
                        StartCoroutine( cameraShine() );
                    }
                }
            }
        }
    }

    public void rotateNearest()
    {
        float nearest = 9999;
        GameObject nearestEnemy = null;
        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) )
        {              
            GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
            foreach ( GameObject enemy in enemies )
            {   
                float dist = Vector3.Distance( transform.position, enemy.transform.position );
                if ( dist < nearest ) 
                {
                    nearest = dist;
                    nearestEnemy = enemy;
                }        
            }
            if ( Vector3.Distance( nearestEnemy.transform.position, transform.position ) < 2 )
            {
                transform.rotation = Quaternion.LookRotation( nearestEnemy.transform.position - transform.position );
                GetComponent<HeroController>().inputDirection = nearestEnemy.transform.position - transform.position;
            }                            
        }

    }

    /*IEnumerator pauseAnim( Transform enemy )
    {        
        anim.speed = 0;
        enemy.GetComponent<Enemy>().anim.speed = 0;
        Camera.main.transform.GetChild( 0 ).renderer.material.color = new Color( 1, 1, 1, 0.075f );
        yield return new WaitForSeconds( 0.1f );
        anim.speed = 1;
        if ( enemy )
            enemy.GetComponent<Enemy>().anim.speed = 1;
        Camera.main.transform.GetChild( 0 ).renderer.material.color = new Color( 1, 1, 1, 0 );
    }*/

    IEnumerator cameraShine()
    {
        Camera.main.transform.GetChild( 0 ).GetComponent<Renderer>().material.color = new Color( 1, 1, 1, 0.075f );
        yield return new WaitForSeconds( 0.1f );
        Camera.main.transform.GetChild( 0 ).GetComponent<Renderer>().material.color = new Color( 1, 1, 1, 0 );
    }

    public IEnumerator pauseAnim( float time )
    {        
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds( time );        
        Time.timeScale = 1f;
    }

    public void getDamage( Transform enemy, string side, bool block )
    {
        goTrail( 0 );
        //StartCoroutine( pauseAnim( enemy.transform ) );
        StartCoroutine( cameraShine() );
        if ( block )
        {
            playSound( blockSound );
            anim.CrossFade( "BlockHit", 0.01f );
            GameObject clone = ( GameObject )Instantiate( blockImpact, transform.position + new Vector3( 0, 1, 0 ) + transform.forward * 0.5f, Quaternion.identity );
            Destroy( clone, 2 );
        }else if ( enemy.GetComponent< Animator >().speed == 1 ){
            health -= 5;
            healthBar.value = health / 100;
            playSound( damageSounds[ ( int )Random.Range( 0, damageSounds.Length ) ] );
            GameObject clone = ( GameObject )Instantiate( blood, transform.position + new Vector3( 0, 1, 0 ), Quaternion.identity );
            Destroy( clone, 2 );            
            if ( health <= 0 )            {
                anim.CrossFade( "Death", 0.01f );
                playSound( deathSound );
            }else{                
                playSound( gruntSound );
                if ( health < 20 )
                {
                    anim.SetFloat( hashTired, 1 );
                    hc.setSpeed( 0.75f );
                }else{
                    anim.SetFloat( hashTired, 0 );
                    hc.setSpeed( 1f );
                }
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

    //void cancelBattleMode()
    //{
    //    anim.SetBool( hashBattleMode, false );
    //}

    public void checkEndOfBattle()
    {
        //Debug.Log( "end of battle " + endOfBattleCounter );
        if ( endOfBattleCounter >= 8 )
            anim.SetBool( hashBattleMode, false );
        else
            anim.SetBool( hashBattleMode, true );
        /*GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach ( GameObject enemy in enemies )
        {            
            if ( Vector3.Distance( transform.position, enemy.transform.position ) < 5f ){
                if ( enemy.GetComponent<Enemy>().battle.health <= 0 )
                {
                    anim.SetBool( hashBattleMode, false );
                }else{
                    anim.SetBool( hashBattleMode, true );
                }
            }             
        }*/
    }

    float getAngle( Transform obj )
    {
        return Vector3.Angle( obj.transform.position - transform.position, transform.forward );
    }

    float getDistance( Transform obj )
    {
        return Vector3.Distance( obj.position, transform.position );
    }

    public void drawWeapon( int type )
    {      
        transform.GetChild( 0 ).GetChild( type ).gameObject.SetActive( true );
        if ( type == 0 )
            transform.FindChild( "Sven Hood" ).GetChild( 0 ).gameObject.SetActive( false );
    }

    public void hideWeapon( int index )
    {        
        transform.GetChild( 0 ).GetChild( index ).gameObject.SetActive( false );        
        transform.FindChild( "Sven Hood" ).GetChild( 0 ).gameObject.SetActive( true );
    }

    public void startBlast(){
        setEnemiesNearbySlow( 0.1f );
    }

    public void blast()
    {
        if ( elementalIndex > 0 && manaBar.fillAmount == 1 )
        {            
            manaBar.fillAmount = 0;
            setEnemiesNearbySlow( 1 );
            if ( elementalIndex == 1 )
            {
                blastEnemies( false );
                Camera.main.GetComponent<Animator>().Play( "Shake" );
                for ( int i = 0; i < lighting.Length; i++ )
                    Instantiate( lighting[ i ], transform.position, Quaternion.identity );
            }else if ( elementalIndex == 2 )
                Instantiate( ice, transform.position + new Vector3( 0, 1f, 0 ) + transform.forward, Quaternion.identity );
            else if ( elementalIndex == 3 )
                Instantiate( fire, transform.position + new Vector3( 0, 5f, 0 ) + transform.forward, Quaternion.identity );
            else if ( elementalIndex == 4 )
            {
                blastEnemies( true );
                Camera.main.GetComponent<Animator>().Play( "Shake" );
                Instantiate( storm, transform.position, Quaternion.identity );
            }                
        }       
        
    }

    void setEnemiesNearbySlow( float speed )
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach ( GameObject enemy in enemies )             
            enemy.GetComponent<Animator>().speed = speed;                    
    }

    void blastEnemies( bool storm )
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach ( GameObject enemy in enemies )
        {
            if ( Vector3.Distance( transform.position, enemy.transform.position ) < 5 )
            {
                enemy.GetComponent<Animator>().CrossFade( "BlownAwayStart", 0.01f );
                enemy.GetComponent<Enemy>().battle.health -= 50;
                if ( storm )
                    enemy.GetComponent<Enemy>().battle.storm = true;
                else
                    enemy.GetComponent<Enemy>().battle.storm = false;
            }
        }               
    }

    Transform getActiveSword()
    {
        for ( int i = 0; i < handSwords.childCount; i++ ){
            if ( handSwords.GetChild( i ).gameObject.activeSelf )
                return handSwords.GetChild( i ); 
        }
        return null;        
    }

    public void goTrail( int i )
    {        
        if ( handSwords.gameObject.activeSelf )
        {
            MeleeWeaponTrail trail = getActiveSword().GetChild( 0 ).GetComponent<MeleeWeaponTrail>();
            if ( i == 0 )
                trail.Emit = false;
            else
                trail.Emit = true;
        }       
            
    }

    public void goTrailDagger( int i )
    {
        //Debug.Log( "yey " + i );
        MeleeWeaponTrail trail = transform.GetChild( 0 ).GetChild( 1 ).GetChild( 0 ).GetComponent<MeleeWeaponTrail>();
        if ( i == 0 )
            trail.Emit = false;
        else
            trail.Emit = true;

    }

}
