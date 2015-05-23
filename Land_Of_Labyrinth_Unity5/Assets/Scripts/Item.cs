using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public string name;
	public string spriteName;
	public float restoreAmount;
	public float radius;

	private Transform label;
	private bool canRestore;
	private bool showHUD;

	void Start(){
		canRestore = true;
		//label = GameObject.FindGameObjectWithTag( "ItemLabel" ).transform;
	}

	void Update () {
		Ray r = new Ray( transform.position, transform.forward );
		RaycastHit hit = new RaycastHit();
		if ( Physics.SphereCast( r, radius, out hit ) ){
			if ( hit.collider.tag == "Hero" ){
				if ( canRestore ){
					GameObject invObj = GameObject.FindGameObjectWithTag( "Inventory" );
					Inventory inv = invObj.GetComponent< Inventory >();
					if ( inv.checkFreeSlots( this as Item ) ){
						showHUD = true;
						canRestore = false;
						gameObject.GetComponent<Renderer>().enabled = false;
						gameObject.GetComponent<Collider>().enabled = false;
						Destroy( gameObject, 5 );
						//label.GetComponent< UILabel >().text = name;
						//label.GetComponent< UIWidget >().color = new Color( 1, 1, 1, 1.5f );
						//label.transform.localPosition = Vector3.zero;
					}
				}
			}
		}
		if ( showHUD ){
			//label.transform.localPosition += new Vector3( 0, 1, 0 );
			//label.GetComponent< UIWidget >().color = Color.Lerp( label.GetComponent< UIWidget >().color, new Color( 1, 1, 1, 0 ), Time.deltaTime * 1 );
		}
	}

}
