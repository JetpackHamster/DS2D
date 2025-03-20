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

    // called when something is in the trigger area
    private void OnTriggerStay2D(Collider2D otherC2D) {
        //Debug.Log("thing moven't");

        Rigidbody2D otherRB = otherC2D.GetComponentInParent<Rigidbody2D>();
        
        // if left click then increase other's velocity toward magnet
        if (Input.GetMouseButton(0))
        {
            

            // find distances
            float xdist = otherC2D.GetComponentInParent<Transform>().position.x - transform.position.x;
            float ydist = otherC2D.GetComponentInParent<Transform>().position.y - transform.position.y;
            float totaldist = Mathf.Sqrt(Mathf.Pow(xdist, 2) + Mathf.Pow(ydist, 2));

            int xdir = 1;
            if(xdist < 0) {
                xdir = -1;
            }
            int ydir = 1;
            if (ydist < 0) {
                ydir = -1;
            }

            // use vector.Normalize to set vector magnitude to 1 and then set correct magnitude seperately
            // just need to get direction with these below lines

            Vector2 fVector = new Vector2(xdist, ydist);
            fVector.Normalize();
            fVector *= Time.deltaTime;
            
            // set to correct magnitude unless very close
            if(totaldist > 0.3) {
                fVector *= (100/Mathf.Pow(totaldist, 2));
                /*while (fVector.magnitude > 10) {
                    Debug.Log("force over limit");
                    fVector *= 0.9F;
                }*/
            } else {
                fVector /= 10;
            }
            otherRB.velocity -= fVector;
            
            
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
