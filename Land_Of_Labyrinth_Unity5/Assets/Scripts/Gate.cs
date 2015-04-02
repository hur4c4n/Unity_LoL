using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public string gateKey;
	public AudioClip gatesOpen;
	
	private HeroController hc;
	private bool canPLay;
	private MessageBox mb;
	private bool gateOpened;
	
	void Start(){
		hc = GameObject.FindGameObjectWithTag( "Hero" ).GetComponent< HeroController >();	
		mb = GameObject.FindGameObjectWithTag( "Hero" ).GetComponent< MessageBox >();
		gateOpened = false;
	}
	
	void Update(){
		if ( canPLay ){
			canPLay = false;
			GetComponent<Animation>().Play();			
			AudioSource.PlayClipAtPoint( gatesOpen, transform.position );
		}
	}
	
	void OnTriggerEnter( Collider col ){
		
		if ( col.gameObject.tag == "Hero" ){
			if ( Input.GetMouseButtonDown( 0 ) ){
				if ( gateOpened == false ){
					if ( hc.keyName == gateKey ){
						canPLay = true;
						hc.keyName = "";
						gateOpened = true;
						//mb.SetText( "You used the'" + gateKey + "' Key." );
					}else if ( gateOpened == false ){
						mb.Open( 0 );
						mb.SetText( "You need the '" + gateKey + "' Key\nto open this gate." );
					}
				}
			}
		}
	}
	
}
