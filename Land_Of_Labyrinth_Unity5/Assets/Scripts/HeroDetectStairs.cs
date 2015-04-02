using UnityEngine;
using System.Collections;

public class HeroDetectStairs : MonoBehaviour {

	void OnTriggerStay( Collider col ){
        if ( col.GetComponent<Collider>().tag == "Stairs" ){
            GameObject.FindGameObjectWithTag( "Hero" ).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
