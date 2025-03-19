using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraneMagnetScript : MonoBehaviour
{
    public bool craneEnabled;
    public GameObject pointer;
    public float[] xyLimits;
    Vector2 fVector;
    Vector3 parentpos;

    // Start is called before the first frame update
    void Start()
    {
        craneEnabled = false;
            parentpos = GetComponentInParent<Transform>(false).position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C)) {
            craneEnabled = !craneEnabled;
            Debug.Log("craneEnabled: " + craneEnabled);
        }

        if (craneEnabled) {
            float xdist = -1 * (transform.position.x - pointer.transform.position.x);
            float ydist = -1 * (transform.position.y - pointer.transform.position.y);
            float xpos = transform.localPosition.x;// - parentpos.x;
            float ypos = transform.localPosition.y;// - parentpos.y;
            
            
            // TODO: fix recenter not working on 0 limits

            // move axes that aren't out of bounds
            if (Mathf.Abs(xdist) > 0.1 || Mathf.Abs(ydist) > 0.1) {
                if (xpos <= xyLimits[1] && xpos >= xyLimits[0]) {
                    //Debug.Log("crane x within limits");
                    if (ypos <= xyLimits[3] && ypos >= xyLimits[2]) {
                        //Debug.Log("crane xy within limits");
                        // move x, y
                        fVector = new Vector2(xdist, ydist);
                    } else {
                        // attempt recenter y, move x
                        fVector = new Vector2(xpos, (-1F*ypos));
                    }
                } else {
                    if (ypos <= xyLimits[3] && ypos >= xyLimits[2]) {
                        //Debug.Log("crane y within limits");
                        // attempt recenter x, move y
                        fVector = new Vector2((-1F*xpos), ydist);
                    } else {
                        // attempt recenter
                        fVector = new Vector2((-1F*xpos), (-1F*ypos));
                        Debug.Log("crane y, limit: " + ypos + " " + xyLimits[2]);
                    }
                }
                fVector.Normalize();
                //Debug.Log(fVector);
                fVector *= Time.deltaTime * 2;
                //fVector *= 999999999;
                
                transform.position = new Vector3(transform.position.x + fVector.x, transform.position.y + fVector.y, transform.position.z);

                
            }
            
        }

    }
}
