using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	
	public AudioClip chestOpen;	
	public string keyName;
	public UIPanel messageBox;
	
	private GameObject itemSlots;
	private UIItemStorage itemStorage;
	private bool canOpen;
	private bool canPlay = true;
	private MessageBox mb;
	private bool callAddKeyAfterOpen;
	private GameObject hero;
	private HeroController hc;
	
	void Start(){
		itemSlots = GameObject.FindGameObjectWithTag( "ItemsSlots" );
	//	itemStorage = itemSlots.GetComponent< UIItemStorage >();
		hero = GameObject.FindGameObjectWithTag( "Hero" );
		mb = hero.GetComponent< MessageBox >();
		hc = hero.GetComponent< HeroController >();
	}
	
	void Update(){
		if ( callAddKeyAfterOpen && mb.canOpen == false ){
			hc.keyName = keyName;
			callAddKeyAfterOpen = false;	
			InvBaseItem getKey = InvDatabase.FindByName( "Key" );
			InvGameItem keyItem = new InvGameItem( 1, getKey );
//			itemStorage.mItems[ 0 ] = keyItem;
		}
	}
	
	void OnTriggerStay( Collider col ){
		
		if ( col.gameObject.tag == "Hero" ){
			if ( Input.GetMouseButtonDown( 0 ) && canPlay ){
				if ( hc.keyName == "" ){
					canPlay = false;				
					mb.Open( 2 );
					mb.SetText( "You got '" + keyName + "' key." );					
					callAddKeyAfterOpen = true;
					GetComponent<Animation>().Play();
					AudioSource.PlayClipAtPoint( chestOpen, transform.position );
				}else{
					canPlay = false;	
					mb.Open( 2 );
					mb.SetText( "You can't carry \nmore than one key" );
					GetComponent<Animation>().Play();
					AudioSource.PlayClipAtPoint( chestOpen, transform.position );
				}
			}
		}
	}
	
}
