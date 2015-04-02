using UnityEngine;
using System.Collections;

public class SpitFire : MonoBehaviour {

    void OnEnable()
    {
        InvokeRepeating( "fire", 0, 3 );
    }

    void fire()
    {
        transform.GetChild( 0 ).GetComponent<EffectSettings>().IsVisible = !transform.GetChild( 0 ).GetComponent<EffectSettings>().IsVisible;
    }

}
