using UnityEngine;
using System.Collections;

public class HeroDetectDoor : MonoBehaviour {
    
    [ HideInInspector ]
    public bool isOpen;

	void OnTriggerStay( Collider col ){
        if ( col.gameObject.tag == "Hero" && Input.GetMouseButtonDown( 0 ) ){
            GetComponent<Animation>().Play( "Open" );
            GetComponent<Collider>().enabled = false;
            isOpen = true;
            transform.FindChild( "House_Wall_Door/House_Door" ).GetComponent<Collider>().enabled = false;
        }
    }
}
