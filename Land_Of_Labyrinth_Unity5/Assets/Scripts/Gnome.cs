using UnityEngine;
using System.Collections;

public class Gnome : Enemy {

	public Transform runAwayPoint;
	public GameObject masterCamera;
	public Vector3 seePlayerCameraPosition;
	public Vector3 seePlayerCameraRotation;
	public float playerDistanceToKick;
	public GameObject disappearEffect;
	public GameObject dustParticle;

	private Animator anim;
	private int hashWalk;
	private int hashRun;
	private int hashRunClip;
	private int hashHit;
	private bool canDisappear;
	private Vector3 worldCutScenePosition;
	private GUIText lockCameraAux;
	private bool canFocus;
	private bool canKick;

	public override void Start () {
		base.Start();
		canDisappear = true;
		canKick = true;
		anim = GetComponent< Animator >();
		lockCameraAux = player.GetComponent< GUIText >();
		hashWalk = Animator.StringToHash( "Walk" );
		hashRun = Animator.StringToHash( "Run" );
		hashRunClip = Animator.StringToHash( "Base Layer.Run" );
		hashHit = Animator.StringToHash( "Hit" );
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

	public virtual void setChasingState(){
		setFocus( true, 0 );
		patrolling.patrolling = false;
		patrolling.patrollingWay.Stop();
		patrolling.pathFinder.target = runAwayPoint;
		anim.SetBool( hashRun, true );
		Invoke( "startRun" , 2.5f );
		player.SetActive( false );
		//setHeroEnabled (false);
		Debug.Log( "YOU WILL NEVER CATCH ME!!" );		
	}

	void setHeroEnabled( bool b ){
		player.transform.GetChild (4).GetComponent< MeshRenderer > ().enabled = b;
		player.transform.GetChild (5).GetComponent< MeshRenderer > ().enabled = b;
		player.transform.GetChild (6).GetComponent< MeshRenderer > ().enabled = b;
		player.transform.GetChild (7).GetComponent< MeshRenderer > ().enabled = b;
		player.transform.GetChild (8).GetComponent< MeshRenderer > ().enabled = b;
		player.GetComponent< HeroController > ().enabled = b;
	}

	void setFocus( bool onOff, float offSet ){
		if ( onOff ){
			canFocus = true;
			lockCameraAux.text = "Gnome";
			worldCutScenePosition = transform.TransformPoint( seePlayerCameraPosition + new Vector3( 0, 0, offSet ) );
		}else{
			canFocus = false;
			lockCameraAux.text = "Normal";
		}
	}

	private void startRun(){
		setFocus( false, 0 );
		player.SetActive( true );
		patrolling.setPathFinder( true );
		//dustParticle.SetActive ( true );
	}

	private void AnimationManager(){
		if ( detection.playerDistance < playerDistanceToKick && canKick ){
			canKick = false;
			hc.kickGnome();
			hc.gnome = gameObject.transform;
			Invoke( "playHitAnimation", 0.3f );
			Invoke( "dropGnomeItem", 3f );
			patrolling.patrolling = false;
			patrolling.patrollingWay.Stop();
			patrolling.pathFinder.canMove = false;
			//dustParticle.GetComponent< ParticleEmitter >().enabled = false;
			//setFocus( true, 3 );
			//Invoke( "enableCamera", 0 );
		}
		if ( Vector3.Distance( transform.position, runAwayPoint.position ) <= 1.5f && canDisappear ){
			canDisappear = false;
			disappear();
		}
	}

	void playHitAnimation(){
		anim.SetBool( hashHit, true );
	}

	void dropGnomeItem(){
		dropItem();
	}

	void enableCamera(){
		patrolling.setPathFinder( true );
		setFocus( false, 0 );
	}

	public void disappear(){
		Instantiate( disappearEffect, transform.position, Quaternion.identity );
		Destroy( gameObject, 1 );
	}

	public override void Update () {
		base.Update();
		AnimationManager();
		if ( HeroDetected() && patrolling.patrolling ) this.setChasingState();
		if ( ContinuePatroll() ) this.setWalk();
		if ( canFocus ){
			masterCamera.transform.position = Vector3.Lerp( masterCamera.transform.position, worldCutScenePosition, Time.deltaTime * 5 );
			masterCamera.transform.rotation = Quaternion.LookRotation( ( transform.position + transform.up * 0.5f ) - masterCamera.transform.position );
		}else{
			masterCamera.transform.localPosition = Vector3.Lerp( masterCamera.transform.localPosition, Vector3.zero, Time.deltaTime * 5 );
			masterCamera.transform.localRotation = Quaternion.Lerp( masterCamera.transform.localRotation, Quaternion.Euler( Vector3.zero ), Time.deltaTime * 5 );
		}
	}

	void OnGUI(){
		
		//if ( GUI.Button ( new Rect( 0, 0, 100, 100 ), "Reset" ) )
		//	Application.LoadLevel( Application.loadedLevelName );
	}
}
		