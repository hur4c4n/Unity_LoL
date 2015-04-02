using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour {

	public GameObject targetObject;
	public AudioClip switchAudio;
	public AudioClip rockSlideAudio;
	
	private SwitcherTarget st;
	private float counter;
	private float interval = 1;

	void Start(){
		if ( targetObject != null )
			st = targetObject.GetComponent< SwitcherTarget >();	
	}
	void Update(){
		counter += Time.deltaTime; 	
		if ( counter > interval ) counter = interval;
	}
	
	void OnTriggerStay( Collider col ){
		
		if ( col.tag == "Hero" && Input.GetMouseButtonDown( 0 ) && counter == interval ){
			counter = 0;
			GetComponent<Animation>().Play();	
			AudioSource.PlayClipAtPoint( switchAudio, st.gameObject.transform.position );
			AudioSource.PlayClipAtPoint( rockSlideAudio, st.gameObject.transform.position );
			if ( st.on )
				st.on = false;
			else
				st.on = true;
		}
	}

}
