using UnityEngine;
using System.Collections;

public class SwitcherTarget : MonoBehaviour {

	[ HideInInspector ]
	public bool on;
	public float height = -3;
	public float timeCountDown = 10;
	public AudioClip rockSlideAudio;
	
	private Vector3 openPosition;
	private Vector3 initialPosition;
	private float counter;
	
	void Start(){
		initialPosition = new Vector3( transform.localPosition.x, transform.localPosition.y, transform.localPosition.z );
		openPosition =  new Vector3( transform.localPosition.x, transform.localPosition.y + height, transform.localPosition.z );	
	}
	
	void Update(){
		if ( on ){
			counter += Time.deltaTime;
			transform.localPosition = Vector3.Slerp( transform.localPosition, openPosition, Time.deltaTime );
			if ( counter > timeCountDown ) {
				counter = 0;
				on = false;
				AudioSource.PlayClipAtPoint( rockSlideAudio, transform.position );
			}
		}else{
			transform.localPosition = Vector3.Slerp( transform.localPosition, initialPosition, Time.deltaTime );
		}
	}
	
	void OnGUI(){
		
		//GUI.Box( new Rect( Screen.width - 200, 0, 200, 50 ), "Wall Open: " + on );	
	}
	
}
