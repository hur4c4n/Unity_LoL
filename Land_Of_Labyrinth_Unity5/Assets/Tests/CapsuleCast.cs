using UnityEngine;
using System.Collections;

public class CapsuleCast : MonoBehaviour {
	public GameObject reference;
	public float radius;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit[] hit = Physics.CapsuleCastAll( transform.position, reference.transform.position, radius, reference.transform.position - transform.position );
		foreach( RaycastHit h in hit ){
			h.collider.gameObject.SetActive( false );
		}
	}
}
