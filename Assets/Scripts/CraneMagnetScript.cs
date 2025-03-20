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
    int leftofXlimit;
    int leftofYlimit;

    // Start is called before the first frame update
    void Start()
    {
        craneEnabled = false;
            parentpos = GetComponentInParent<Transform>(false).position;
    }
    // check direction first float is from second float
    private int directionOfLimit(float pos, float limit) {
        if (pos >= limit) {
            return -1;
        } else {
            return 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // toggle crane with C
        if (Input.GetKeyDown(KeyCode.C)) {
            craneEnabled = !craneEnabled;
            Debug.Log("craneEnabled: " + craneEnabled);
        }

        // if crane time do crane things
        if (craneEnabled) {
            float xdist = -1 * (transform.position.x - pointer.transform.position.x);
            float ydist = -1 * (transform.position.y - pointer.transform.position.y);
            float xpos = transform.localPosition.x;// - parentpos.x;
            float ypos = transform.localPosition.y;// - parentpos.y;
            
            //Debug.Log("xdist, ydist: " + xdist + ", " + ydist);
            
            // move axes that aren't out of bounds if cursor not at same spot as magnet
            if (Mathf.Abs(xdist) > 0.2 || Mathf.Abs(ydist) > 0.2) {
                if (xpos <= xyLimits[1] && xpos >= xyLimits[0]) {
                    //Debug.Log("crane x within limits");
                    if (ypos <= xyLimits[3] && ypos >= xyLimits[2]) {
                        //Debug.Log("crane xy within limits");
                        // move x, y
                        fVector = new Vector2(xdist, ydist);
                    } else {
                        // attempt recenter y, move x
                        fVector = new Vector2(xpos, directionOfLimit(ypos, xyLimits[3]));
                    }
                } else {
                    if (ypos <= xyLimits[3] && ypos >= xyLimits[2]) {
                        //Debug.Log("crane y within limits");
                        // attempt recenter x, move y
                        fVector = new Vector2(directionOfLimit(xpos, xyLimits[1]), ydist);
                    } else {
                        // attempt recenter
                        fVector = new Vector2(directionOfLimit(xpos, xyLimits[1]), directionOfLimit(ypos, xyLimits[3]));
                        //Debug.Log("all out of bounds; crane y, limit: " + ypos + " " + xyLimits[2]);
                    }
                }
                fVector.Normalize();
                fVector *= Time.deltaTime * 0.2F;
                
                transform.localPosition = new Vector3(transform.localPosition.x + fVector.x, transform.localPosition.y + fVector.y, transform.localPosition.z);

            }
            
        }

    }
}
