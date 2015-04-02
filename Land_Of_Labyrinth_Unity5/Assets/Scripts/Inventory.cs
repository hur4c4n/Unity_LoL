using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private Transform[] slots;
	private Vector3[] initialPositions;
	private int index;
	private Vector3 bigScale;
	private Vector3 smallScale;

	void Start () {
		index = 0;
		slots = new Transform[ 4 ];
		initialPositions = new Vector3[ 4 ];
		bigScale = new Vector3( 1, 1, 0 );
		smallScale = new Vector3( 0.5f, 0.5f, 0 );
		for ( int i = 0; i < 4; i++ ){
			slots[ i ] = transform.GetChild( i );
			initialPositions[ i ] = transform.GetChild( i ).localPosition;
		}
	}

	public bool checkFreeSlots( Item item ){
		for( int i = 0; i < 4; i++ ){
			InventorySlot invSlot = transform.GetChild( i ).GetComponent< InventorySlot >();
			if ( invSlot.active == false ){
				invSlot.item = item;
				invSlot.active = true;
				UISprite sprite = transform.GetChild( i ).GetChild( 0 ).GetComponent< UISprite >();
				sprite.spriteName = item.spriteName;
				return true;
			}
		}
		return false;
	}

	void Update () {
		if ( Input.GetKeyDown( KeyCode.E ) ){
			if ( index < 3 )
				index++;
			else
				index = 0;
		}
		if ( Input.GetKeyDown( KeyCode.R ) ){
			if ( slots[ index ].GetComponent< InventorySlot >().item != null ){
				float health = slots[ index ].GetComponent< InventorySlot >().item.restoreAmount;
				HeroController hc = GameObject.FindGameObjectWithTag( "Hero" ).GetComponent< HeroController >();
				hc.healthPoints += health;
				slots[ index ].GetComponent< InventorySlot >().item = null;
				slots[ index ].GetChild( 0 ).GetComponent< UISprite >().spriteName = "Nothing";
			}
		}
		for ( int i = 0; i < 4; i++ ){
			if ( index == i )
				slots[ index ].localScale = Vector3.Lerp( slots[ index ].localScale, bigScale, Time.deltaTime * 5 );
			else
				slots[ i ].localScale = Vector3.Lerp( slots[ i ].localScale, smallScale, Time.deltaTime * 5 );
		}
		switch( index ){
		case 0:
			slots[ 0 ].localPosition = Vector3.Lerp( slots[ 0 ].localPosition, initialPositions[ 0 ], Time.deltaTime * 5 );
			slots[ 1 ].localPosition = Vector3.Lerp( slots[ 1 ].localPosition, initialPositions[ 1 ], Time.deltaTime * 5 );
			slots[ 2 ].localPosition = Vector3.Lerp( slots[ 2 ].localPosition, initialPositions[ 2 ], Time.deltaTime * 5 );
			slots[ 3 ].localPosition = Vector3.Lerp( slots[ 3 ].localPosition, initialPositions[ 3 ], Time.deltaTime * 5 );
			break;
		case 1:
			slots[ 1 ].localPosition = Vector3.Lerp( slots[ 1 ].localPosition, initialPositions[ 0 ], Time.deltaTime * 5 );
			slots[ 2 ].localPosition = Vector3.Lerp( slots[ 2 ].localPosition, initialPositions[ 1 ], Time.deltaTime * 5 );
			slots[ 3 ].localPosition = Vector3.Lerp( slots[ 3 ].localPosition, initialPositions[ 2 ], Time.deltaTime * 5 );
			slots[ 0 ].localPosition = Vector3.Lerp( slots[ 0 ].localPosition, initialPositions[ 3 ], Time.deltaTime * 5 );
			break;
		case 2:
			slots[ 2 ].localPosition = Vector3.Lerp( slots[ 2 ].localPosition, initialPositions[ 0 ], Time.deltaTime * 5 );
			slots[ 3 ].localPosition = Vector3.Lerp( slots[ 3 ].localPosition, initialPositions[ 1 ], Time.deltaTime * 5 );
			slots[ 0 ].localPosition = Vector3.Lerp( slots[ 0 ].localPosition, initialPositions[ 2 ], Time.deltaTime * 5 );
			slots[ 1 ].localPosition = Vector3.Lerp( slots[ 1 ].localPosition, initialPositions[ 3 ], Time.deltaTime * 5 );
			break;
		case 3:
			slots[ 3 ].localPosition = Vector3.Lerp( slots[ 3 ].localPosition, initialPositions[ 0 ], Time.deltaTime * 5 );
			slots[ 0 ].localPosition = Vector3.Lerp( slots[ 0 ].localPosition, initialPositions[ 1 ], Time.deltaTime * 5 );
			slots[ 1 ].localPosition = Vector3.Lerp( slots[ 1 ].localPosition, initialPositions[ 2 ], Time.deltaTime * 5 );
			slots[ 2 ].localPosition = Vector3.Lerp( slots[ 2 ].localPosition, initialPositions[ 3 ], Time.deltaTime * 5 );
			break;
		}
	}

}
