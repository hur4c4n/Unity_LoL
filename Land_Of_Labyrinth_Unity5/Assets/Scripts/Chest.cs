using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	
	public AudioClip chestOpen;
    public AudioClip submit;	
	public string keyName;
    public Transform chestMessage;
    public Transform camera;
    private Transform hero;
    private bool opened;

    void Start()
    {
        hero = GameObject.Find( "Hero" ).transform;
    }

    void Update()
    {
        if ( Vector3.Distance( transform.position, hero.transform.position ) < 1 && Input.GetMouseButtonDown( 0 ) && !opened )
        {
            opened = true;
            GetComponent<Animator>().Play( "box_open" );
            AudioSource.PlayClipAtPoint( chestOpen, transform.position );
            Invoke( "ShowMessage", 1 );
        }
    }

    void ShowMessage()
    {
        Time.timeScale = 0f;
        chestMessage.gameObject.SetActive( true );
        chestMessage.GetChild( 1 ).GetComponent<UILabel>().text = "You found " + keyName + " key.";
    }

    public void Submit()
    {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint( submit, transform.position );        
        chestMessage.gameObject.SetActive( false );
    }
	
}
