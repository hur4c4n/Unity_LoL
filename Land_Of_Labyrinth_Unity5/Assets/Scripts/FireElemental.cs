using UnityEngine;
using System.Collections;

public class FireElemental : MonoBehaviour {

    private GameObject[] enemies;
    public float minDistanceDamage;
    public float damage;
    private float counter;
    private bool canDescrease = true;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
    }
    
    void Update()
    {
        counter += Time.deltaTime;
        if ( counter > 0.75f && counter < 7 )
            decreaseEnemyHealth();    
    }

    void decreaseEnemyHealth()
    {
        foreach ( GameObject go in enemies )
        {
            if ( Vector3.Distance( transform.position, go.transform.position ) < minDistanceDamage )
            {                  
                if ( canDescrease )
                {
                    go.GetComponent<Enemy>().battle.health -= damage;
                    if ( go.GetComponent<Enemy>().battle.health <= 0 )
                    {
                        go.GetComponent<Enemy>().setDeath();
                        canDescrease = false;
                    }                        
                }                
            }
        }
    }

}
