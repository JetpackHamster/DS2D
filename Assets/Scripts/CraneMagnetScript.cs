using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMagnetScript : MonoBehaviour
{
    public bool craneEnabled;
    public GameObject pointer;
    public float[] xyLimits;
    Vector2 fVector;

    // Start is called before the first frame update
    void Start()
    {
        craneEnabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C)) {
            craneEnabled = !craneEnabled;
            Debug.Log("craneEnabled: " + craneEnabled);
        }

        if (craneEnabled) {
            float xdist = transform.position.x - pointer.transform.position.x;
            float ydist = transform.position.y - pointer.transform.position.y;


            // move axes that aren't out of bounds
            if (xdist > 0.1 || ydist > 0.1) {
                if (transform.position.x <= xyLimits[1] && transform.position.x >= xyLimits[0]) {
                    if (transform.position.y <= xyLimits[2] && transform.position.y >= xyLimits[3]) {

                        fVector = new Vector2(xdist, ydist);
                    } else {
                        fVector = new Vector2(xdist, 0);
                    }
                } else {
                    if (transform.position.y <= xyLimits[2] && transform.position.y >= xyLimits[3]) {

                        fVector = new Vector2(0, ydist);
                    }
                }
                fVector.Normalize();
                fVector *= Time.deltaTime;
                fVector *= 999999999;
                
                transform.position = new Vector3(transform.position.x + fVector.x, transform.position.y + fVector.y, transform.position.z);

                
            }
            
        }

    }
}
