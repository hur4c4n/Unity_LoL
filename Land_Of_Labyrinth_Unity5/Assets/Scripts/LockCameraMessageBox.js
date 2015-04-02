#pragma strict

@HideInInspector
var mcObj : GameObject;
@HideInInspector
var mc : MasterCamera;
@HideInInspector
var text : GUIText;

function Start () {
    mcObj = GameObject.FindGameObjectWithTag( "MasterCamera" );
    mc = mcObj.GetComponent( MasterCamera );
    text = GetComponent( GUIText );
}

function Update () {
	if ( text.text == "Open" ){
		mc.enabled = false;
	}else{
		mc.enabled = true;
	}
}