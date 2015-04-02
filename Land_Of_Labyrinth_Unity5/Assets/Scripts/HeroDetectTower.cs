using UnityEngine;
using System.Collections;

public class HeroDetectTower : MonoBehaviour {

    public bool outside;
	public bool smallTower;
    private HeroController hc;
    private GameObject doorFloor1;
    private GameObject doorFloor2;
    private HeroDetectDoor heroDetectDoor1;
    private HeroDetectDoor heroDetectDoor2;

    void Start(){
        hc = GameObject.FindGameObjectWithTag( "Hero" ).GetComponent< HeroController >();
        doorFloor1 = transform.parent.parent.FindChild( "Tower_FirstFloor/House_Door" ).gameObject;
        heroDetectDoor1 = doorFloor1.GetComponent< HeroDetectDoor >();
		if ( !smallTower ){
        	doorFloor2 = transform.parent.parent.FindChild( "Tower_ThirdFloor/House_Door" ).gameObject;
        	heroDetectDoor2 = doorFloor2.GetComponent< HeroDetectDoor >();
		}
    }

	void OnTriggerStay( Collider col ){
        if ( col.gameObject.tag == "Hero" ){
            if ( outside ){
                if ( doorFloor1 && heroDetectDoor1.isOpen ){
                    doorFloor1.GetComponent<Animation>().Play( "Close" );
                    doorFloor1.GetComponent<Collider>().enabled = true;
                    doorFloor1.transform.FindChild( "House_Wall_Door/House_Door" ).GetComponent<Collider>().enabled = true;                   
                    heroDetectDoor1.isOpen = false;
                }
                if ( doorFloor2 && heroDetectDoor2.isOpen ){
                    doorFloor2.GetComponent<Animation>().Play( "Close" );
                    doorFloor2.GetComponent<Collider>().enabled = true;
                    doorFloor2.transform.FindChild( "House_Wall_Door/House_Door" ).GetComponent<Collider>().enabled = true;                   
                    heroDetectDoor2.isOpen = false;    
                }
            }
        }
    }
}
