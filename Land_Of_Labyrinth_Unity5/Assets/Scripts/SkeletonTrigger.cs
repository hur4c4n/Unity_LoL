using UnityEngine;
using System.Collections;

public class SkeletonTrigger : MonoBehaviour {

    private Skeleton skeletonScript;

	void Start () {
	    skeletonScript = transform.parent.GetComponent< Skeleton >();
	}
	
	void OnTriggerStay( Collider col ){
        if ( col.gameObject.tag == "Hero" )
            skeletonScript.setChasingState();
    }
}
