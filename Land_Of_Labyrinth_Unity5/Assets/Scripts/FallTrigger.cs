using UnityEngine;
using System.Collections;

public class FallTrigger : MonoBehaviour {

    public Transform hero;

    void Start(){
        hero = GameObject.Find( "Hero" ).transform;
    }
    
    void Update(){
        //if ( collider.bounds.Contains( hero.position ) )
            //hero.GetComponent<HeroController>().setFall();
    }

    void OnTriggerEnter( Collider col )
    {
        Debug.Log( "deu" );
    }
        
        
    
}
