using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;


public class HeroController: MonoBehaviour {
	
	public float runSpeed = 10.0f;		
	public float walkSpeed = 6.0f;
    public float tiredSpeed = 3.0f;
	public float rotateSpeed = 500.0f;
	public float battleSpeed = 2.5f;	
	public Transform target;
	public Transform gnome;
	public GameObject bloodEffect;	        
	public string keyName;
	[ HideInInspector ]
	public bool stumbling;
	[ HideInInspector ]
	public bool stumblingFront;
	//[ HideInInspector ]
	//public bool heroWeaponTriggerActive;    
	
	private float v;
	private float h;	
    private Transform cameraTransform;
    private Vector3 forward;

	//fight
	private float fightInputV;
	private float fightInputH;
	private Vector3 fightInputDirection;	
	private float dodgeCounter;
	private bool canStartDodge;
	private int dodgeInputCounterH;
    private int dodgeInputCounterV;
    private Vector3 dodgeCrouchPosition;
	private float stateCounter;	
	[ HideInInspector ]
	public bool fightMode;	

	//speed
	private float currentSpeed;
	private float moveSpeed = 1.0f;	
	private float initialRunSpeed;	

	//transform
    [ HideInInspector ]
	public Vector3 inputDirection;
    [HideInInspector]
	public Vector3 moveDirection = Vector3.zero;
	//private bool stopRuning;
	//private Vector3 stopRunningDirection;
	//private GameObject heroWeapon;

    //health
	[ HideInInspector ]
    public float healthPoints = 100;
    public UISlider healthSliderValue;
    
    //collider
    [ HideInInspector ]
	public CharacterController heroCollider;
    private CollisionFlags collisionFlags;
	private bool dead;
	private float inputCrouch;
    public AudioClip[] footsteps;
	
	//Hash Ids		
	private int hashMoving;		    
	private int hashRuning;	
	private int hashHorizontal;
	private int hashVertical;
    private int hashKick;
    private int hashCrouch;
    private int hashFixpos;
    private int hashBattleMode;
    private int hashSwordInHands;
	
	private Animator anim;
    private float fallCounter;

    private bool falling;
    private bool canRoll;

    public AudioClip[] roll;

    void Start()
    {
        InputManager.Setup();        
    }
	
	void Awake ()
	{        
		moveDirection = transform.TransformDirection( Vector3.forward );	
		anim = GetComponent< Animator >();
		heroCollider = GetComponent< CharacterController >();
		//heroWeapon = GameObject.FindGameObjectWithTag( "HeroWeapon" );
        initialRunSpeed = runSpeed;
		hashMoving = Animator.StringToHash( "Moving" );		
		hashRuning = Animator.StringToHash( "Runing" );
		hashHorizontal = Animator.StringToHash ( "Horizontal" );
		hashVertical = Animator.StringToHash ( "Vertical" );
        hashCrouch = Animator.StringToHash( "Crouch" );
        hashFixpos = Animator.StringToHash( "FixPos" );
        hashBattleMode = Animator.StringToHash( "BattleMode" );
        hashSwordInHands = Animator.StringToHash( "SwordInHands" );
	}

    public void ResetGame()
    {
        Application.LoadLevel( Application.loadedLevelName );
    }

	void UpdateSmoothedMovementDirection ()
	{		

		cameraTransform = Camera.main.transform;
		forward = cameraTransform.TransformDirection(Vector3.forward);            
		forward.y = 0;
		forward = forward.normalized;		

		Vector3 right = new Vector3( forward.z, 0, -forward.x );
        v = Input.GetAxisRaw( "Vertical" );  
        //v = InputManager.ActiveDevice.GetControl( InputControlType.Analog1 ).Value * -1;
		h = Input.GetAxisRaw("Horizontal");
        //h = InputManager.ActiveDevice.GetControl( InputControlType.Analog0 ).Value;
		anim.SetFloat ( hashHorizontal, h );
		anim.SetFloat ( hashVertical, v );        
		inputDirection = Vector3.Lerp( inputDirection, h * right + v * forward, Time.deltaTime * rotateSpeed );
			
		if ( inputDirection != Vector3.zero &&            
            !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "RunStop" ) &&
            //!anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) &&
            !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) &&
            !anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack3" ) )
		    moveDirection = Vector3.RotateTowards( moveDirection, inputDirection, rotateSpeed * Time.deltaTime, 30 );
		
		//moveDirection = moveDirection.normalized;
		//inputDirection = inputDirection.normalized;        
		float curSmooth = 10 * Time.deltaTime;
		float targetSpeed = Mathf.Min( inputDirection.magnitude, 1.0f );
		targetSpeed *= currentSpeed;	
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth );
			
	}

    void AnimationManager()
    {
        if ( v != 0 || h != 0 )        
            anim.SetBool( hashMoving, true );            
        else
            anim.SetBool( hashMoving, false );
        if ( Input.GetButton( "Run" ) || 
            InputManager.ActiveDevice.GetControl( InputControlType.Button6 ) ||
            InputManager.ActiveDevice.RightTrigger
            )
        {
            currentSpeed = runSpeed;
            anim.SetBool( hashRuning, true );
            //anim.SetBool( hashBattleMode, false ); 
        }else{
            currentSpeed = walkSpeed;
            anim.SetBool( hashRuning, false );
        }
         
        if ( Input.GetButton( "Crouch" ) )        
            anim.SetBool( hashCrouch, true );                    
        else
            anim.SetBool( hashCrouch, false );

        if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Run" ) && Input.GetKeyDown( KeyCode.Space ) )
            anim.CrossFade( "Jump", 0.01f );

    }
    
    void GravityFix(){        
		
        if ( falling ){
            collisionFlags = heroCollider.Move( ( new Vector3( 0, anim.GetFloat( "FixPos" ), 0 ) + transform.forward * 0.02f ) * Time.timeScale );
            if ( canRoll && heroCollider.isGrounded )
            {
                falling = false;
                anim.CrossFade( "Roll", 0.01f );
                playSound( roll[ 0 ] );
                playSound( roll[ 1 ] );
            }                
        }else
            collisionFlags = heroCollider.Move( new Vector3( 0, -10, 0 ) );
    }        		

	public void kickGnome(){
		anim.CrossFade( "Kick", 0.01f );
		//Invoke( "cancelKick", 1f );
	}

	void cancelKick(){
		anim.SetBool( hashKick, false );
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

    public void playFootstep()
    {
        playSound( footsteps[ ( int )Random.Range( 0, footsteps.Length ) ] );        
    }

    public void setSpeed( float value )
    {
        walkSpeed = value;
    }

    void OnControllerColliderHit( ControllerColliderHit hit )
    {
        if ( hit.gameObject.name == "FallTrigger" )
        {
            falling = true;
            anim.CrossFade( "Fall", 0.01f );
            //Invoke( "setCanRoll", 1f );
        }

    }

    public void setCanRoll()
    {
        canRoll = true;
    }

    public void cancelRoll()
    {        
        canRoll = false;
    }

    public void checkSwordInHands()
    {
        if ( transform.GetChild( 0 ).GetChild( 0 ).gameObject.activeSelf || transform.GetChild( 0 ).GetChild( 1 ).gameObject.activeSelf )
            anim.SetBool( hashSwordInHands, true );
        else
            anim.SetBool( hashSwordInHands, false );
    }

	void Update() {

        InputManager.Update();
		UpdateSmoothedMovementDirection();		
		AnimationManager();		        
        GravityFix();		
		if ( dead == false ){
            if ( ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "RunStop" ) ||
                   anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack1" ) ||
                   anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack2" ) ||
                   anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Attack3" ) ) && Time.timeScale == 1 )
                collisionFlags = heroCollider.Move( transform.forward * anim.GetFloat( hashFixpos ) * Time.timeScale );
            else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Death" ) && anim.GetCurrentAnimatorStateInfo( 0 ).normalizedTime < 0.9f )
                collisionFlags = heroCollider.Move( -transform.forward * Time.deltaTime );
            else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitRight" ) ||                 
                      anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitLeft" ) ||                 
                      anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "HitMiddle" ) )
                      collisionFlags = heroCollider.Move( -transform.forward * anim.GetFloat( hashFixpos ) * Time.timeScale );
            else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Roll" ) )
                collisionFlags = heroCollider.Move( transform.forward * anim.GetFloat( hashFixpos ) );
            else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "BlockHit" ) )
                collisionFlags = heroCollider.Move( transform.forward * -anim.GetFloat( hashFixpos ) );
            else if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Jump" ) )
                collisionFlags = heroCollider.Move( transform.up * anim.GetFloat( hashFixpos ) + transform.forward * 0.05f * Time.timeScale );
            else{
                if ( stumbling ){
                    if ( !stumblingFront )
                        collisionFlags = heroCollider.Move( transform.forward * moveSpeed * Time.deltaTime );
                }else{
                    if ( anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Run" ) || 
                        anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "Walk" ) || 
                        anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "WalkBattle" ) ||
                        anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "CrouchWalk" ) ||
                        anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "CrouchIdle" ) ||
                        anim.GetCurrentAnimatorStateInfo( 0 ).IsName( "RunWithSword" ) )                    
                        collisionFlags = heroCollider.Move( moveDirection * moveSpeed * Time.deltaTime );
                }                
                transform.rotation = Quaternion.LookRotation( moveDirection );                
            } 
		}
        if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
            Time.timeScale = 0.1f;
        if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            Time.timeScale = 1f;  

	}

	void OnGUI(){
		
		//if ( GUI.Button( new Rect( 0, 20, 100, 20 ), "Reset" ) ){
		//	Application.LoadLevel( Application.loadedLevelName );
        //}

       	//if ( GUI.Button( new Rect( 0, 40, 100, 20 ), " +Health " ) ){
	      //  healthPoints -= 5;
        //}  
		/*if ( GUI.Button( new Rect( 0, 60, 100, 20 ), "Battle" ) ){
			if ( target ){
				target = null;	
			}else{
				target = GameObject.Find( "Skeleton" ).transform;	
			}
		}
		GUI.Box( new Rect( 0, 80, 140, 20 ), "skeleton attack: " + enemyIsAttacking );
		*/	
	}

    /*void ClimbManager(){
		
    RaycastHit hit;
    Ray r = new Ray( transform.position + new Vector3( 0, 0.1f, 0 ), transform.forward );
    Debug.DrawRay( transform.position + new Vector3( 0, 0.1f, 0 ), transform.forward * 0.5f, Color.green );
    if ( Physics.Raycast( r.origin, r.direction, out hit, 0.5f ) ){
        if ( hit.collider.tag == "Climb" ){
            if ( Input.GetMouseButton( 0 ) ){
                anim.SetBool( hashClimb, true );	
            }
        }
    }
    if ( anim.GetBool( hashClimb ) ){
        climbCounter += Time.deltaTime;
        heroCollider.enabled = false;
        if ( climbCounter > 1f ){
            climbCounter = 0;
            heroCollider.enabled = true;
            anim.SetBool( hashClimb, false );
        }
    }
}*/
	

    /*void DodgeManager(){
        
        if ( Input.GetButtonDown( "DodgeRight" ) )
            dodgeInputCounterH += 1;			
        if ( Input.GetButtonDown( "DodgeLeft" ) )
            dodgeInputCounterH -= 1;
        if ( Input.GetButtonDown( "DodgeUp" ) )
            dodgeInputCounterV += 1;
        if ( Input.GetButtonDown( "DodgeDown" ) )            
            dodgeInputCounterV -= 1;	
        if ( dodgeInputCounterH != 0 || dodgeInputCounterV != 0 ){
            dodgeCounter += Time.deltaTime;
            if ( dodgeCounter > dodgeCounterTime ){
                if ( dodgeInputCounterH > 1 || dodgeInputCounterH < -1 ) {
                    anim.SetInteger( hashDodgeHorizontal, dodgeInputCounterH );
                }
                if ( dodgeInputCounterV > 1 || dodgeInputCounterV < -1 ) {
                    anim.SetInteger( hashDodgeVertical, dodgeInputCounterV );
                }
                dodgeCrouchPosition = transform.position;
                dodgeCounter = 0;
                dodgeInputCounterH = 0;
                dodgeInputCounterV = 0;
                Invoke( "cancelDodge", dodgeCounterTime );
            }
        }
        if ( currentStateInfo.nameHash == hashDodgeBackClip && anim.GetFloat( hashDodgeEffective ) > 0 ){
            heroCollider.Move( -transform.forward * 0.1f );
        }
        if ( currentStateInfo.nameHash == hashDodgeLeftClip && anim.GetFloat( hashDodgeEffective ) > 0 ){
            heroCollider.Move( -transform.right * 0.05f );
        }
        if ( currentStateInfo.nameHash == hashDodgeRightClip && anim.GetFloat( hashDodgeEffective ) > 0 ){
            heroCollider.Move( transform.right * 0.05f );
        }
        if ( currentStateInfo.nameHash == hashDodgeCrouchClip ){
            transform.position = dodgeCrouchPosition;
        }
    }*/

    /*void cancelDodge(){
        anim.SetInteger( hashDodgeHorizontal, 0 );
        anim.SetInteger( hashDodgeVertical, 0 );
    }

    void cancelJump(){
           anim.SetBool( hashRunJump, false );
    }
	
    void TrailManager(){
        if ( trail != null ){
            if ( anim.GetFloat( hashTrail ) > 0 ){
                trail.enabled = true;
            }else{
                trail.enabled = false;	
            }
        }
    }
	
    public bool isCrouching(){

        if ( inputCrouch > 0 )
            return true;
        else
            return false;
    }*/


    /*void AimingManager(){

        RaycastHit hit = new RaycastHit();
        Vector3 throwPosition = Vector3.zero;

        if ( Camera.main ){
            anim.SetBool( hashThrow, false );
            crossHair.transform.FindChild( "CrossHair" ).transform.Rotate( Vector3.up, 20 );                
            if ( Input.GetMouseButton( 0 ) && !anim.GetBool( hashFightMode ) ){
                if ( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out hit, Mathf.Infinity ) ){
                    if ( hit.collider.tag == "Terrain" ){
                        crossHair.transform.FindChild( "CrossHair" ).renderer.enabled = true;
                        crossHair.transform.position = hit.point;
                        isAiming = true;
                    }else if ( hit.collider.tag == "Enemy" ){
                        crossHair.transform.position = hit.transform.position;
                        isAiming = true;
                    }else{
                        crossHair.transform.FindChild( "CrossHair" ).renderer.enabled = false;
                        isAiming = false;
                    }
                }   
            }else{            
                crossHair.transform.FindChild( "CrossHair" ).renderer.enabled = false;
            }
            if ( isAiming && Input.GetMouseButton( 0 ) == false ){
                isAiming = false;
                anim.SetBool( hashThrow, true );
            }
            if ( currentStateInfo.nameHash == hashThrowClip )
                moveDirection = Vector3.Lerp( moveDirection, crossHair.transform.position - transform.position,Time.deltaTime * 5 );           
            if ( anim.GetFloat( hashReleaseRock ) > 0 ){
                if ( canInstantiateRock ) {
                    canInstantiateRock = false;
                    rock = ( GameObject )Instantiate( rockPrefab );                
                    rockPosition = GameObject.FindGameObjectWithTag( "RockPosition" ).transform;
                    rock.transform.position = rockPosition.position;                
                }     
                if ( rock ){
                    Vector3 dir = ( crossHair.transform.position - rock.transform.position ) + new Vector3( 0, 0.2f, 0 );
                    rock.rigidbody.AddForce( dir * rockForce, ForceMode.Acceleration ); 
                    Destroy( rock, 10 );
                }
            }else{
                canInstantiateRock = true;
            }
        }
    }*/

}
