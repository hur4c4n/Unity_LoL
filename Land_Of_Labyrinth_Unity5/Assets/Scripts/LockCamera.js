#pragma strict

@HideInInspector
var anim : Animator;
@HideInInspector
var hashIdle : int;
@HideInInspector
var hashFightMode : int;
@HideInInspector
var currentState : AnimatorStateInfo;
@HideInInspector
var mcObj : GameObject;
@HideInInspector
var mc : MasterCamera;
@HideInInspector
var text : GUIText;
var BattleCamera : GameObject;


function Start () {
    anim = GetComponent( Animator );
    mcObj = GameObject.FindGameObjectWithTag( "MasterCamera" );
    mc = mcObj.GetComponent( MasterCamera );
    hashIdle = Animator.StringToHash( "Base.Idle" );
    hashFightMode = Animator.StringToHash( "FightMode" );
    text = GetComponent( GUIText );
}

function Update () {
    currentState = anim.GetCurrentAnimatorStateInfo( 0 );
    //if ( Input.GetMouseButton( 0 ) && currentState.nameHash == hashIdle ){
    //   mc.enabled = false;  
    //}else{
    //   mc.enabled = true;
    //}
   // if ( anim.GetBool( hashFightMode ) ){
    //	mc.enabled = false;
    //	Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, BattleCamera.transform.position, Time.deltaTime * 2 );
    //	text.text = "Battle";
    //}else{   
     	//Camera.main.transform.localPosition = Vector3.Lerp( Camera.main.transform.localPosition, Vector3.zero, Time.deltaTime * 2 );
		if ( text.text == "Open" ){
			mc.enabled = false;
		}else
		if ( text.text == "Close" ){
			mc.enabled = true;
		}else
		if ( text.text == "Gnome" ){
			mc.enabled = false;
		}else
		if ( text.text == "Normal" ){
			mc.enabled = true;
		}
    //}
}