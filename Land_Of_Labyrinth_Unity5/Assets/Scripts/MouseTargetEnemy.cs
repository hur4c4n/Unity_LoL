using UnityEngine;
using System.Collections;

public class MouseTargetEnemy : MonoBehaviour {

	private GameObject lastHighlighted;
	private Enemy enemy;

	void Update () {

		RaycastHit hit;
		Ray vRay = Camera.main.ScreenPointToRay( Input.mousePosition );
		if ( Physics.Raycast( vRay, out hit, 1000 ) ){
			if ( hit.collider.tag == "Enemy" ){
				lastHighlighted = hit.collider.gameObject;
				enemy = lastHighlighted.GetComponent< Enemy >();
				if ( enemy != null ) 
					enemy.mouseOver = true;
			}else{
				if ( enemy != null )
					enemy.mouseOver = false;
			}
		}else{
			if ( enemy != null )
				enemy.mouseOver = false;
		}
	}
}
