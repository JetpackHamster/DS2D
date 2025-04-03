using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapScript : MonoBehaviour
{
    public float value;

    // Start is called before the first frame update
    void Start()
    {
        if (value == 0) {
            value = 2.5F;
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
