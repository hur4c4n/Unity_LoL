using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {

	[ HideInInspector ]
	public bool canOpen;
	public Transform messageObj;
	private UILabel message;
	private UIButton button;
	
	private UIPanel messagePanel;
	private GUIText text;
	private UIButtonMessage bm;
	
	void Start(){
		text = GetComponent< GUIText >();	
		message = messageObj.transform.GetChild( 1 ).GetComponent< UILabel >();
		button = messageObj.transform.GetChild( 0 ).GetComponent< UIButton >();
		bm = button.GetComponent< UIButtonMessage >();
		messagePanel = messageObj.transform.GetComponent< UIPanel >();
	}
	
	void close(){
		canOpen = false;	
		text.text = "Close";
	}
	
	public void SetText( string text ){
		message.text = text;
	}
	
	public void Open( float delay ){
		Invoke( "setOpen", delay );
	}
	
	void setOpen(){
		canOpen = true;	
	}
	
	void enableButton(){
		button.enabled = true;	
		bm.enabled = true;		
	}
	
	void Update(){
		if ( canOpen ){
			text.text = "Open";	
			Invoke( "enableButton", 1 );
			messagePanel.alpha = Mathf.Lerp( messagePanel.alpha, 1, Time.deltaTime * 5 );
		}else{
			//text.text = "Close";
			if ( button ) button.enabled = false;
			if ( bm ) bm.enabled = false;
			if ( messagePanel ) messagePanel.alpha = Mathf.Lerp( messagePanel.alpha, 0, Time.deltaTime * 5 );
		}
	}
	
}
