using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shimmer : MonoBehaviour {

    [Range(0, 1)]   public float speed;
    [Range(0, 50)] public int speedOfChange;

    private Light theLight;
    private float rand;
    private int counter;

	void Start () {

        theLight = GetComponent<Light>();
        rand = Random.Range(1, 3);
    }

	void Update () {

        if (counter >= speedOfChange)
        {
            rand = Random.Range(1, 3);
            counter = 0;
        }
        
        counter++;

        if (theLight.intensity >= rand)
        {
            theLight.intensity = theLight.intensity - speed;
        }
        else
        {
            theLight.intensity = theLight.intensity + speed;
        }

        
    }
}
