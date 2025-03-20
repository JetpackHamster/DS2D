using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitOrbScript : MonoBehaviour
{
    public Rigidbody2D RB;
    public GameObject targetObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if left click then increase velocity toward target
        if (Input.GetMouseButton(0))
        {
            

            // find distances
            float xdist = transform.position.x - targetObj.transform.position.x;
            float ydist = transform.position.y - targetObj.transform.position.y;
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
            if(totaldist > 0.2) {
                fVector *= (100/Mathf.Pow(totaldist, 2));
                while (fVector.magnitude > 10) {
                    Debug.Log("force over limit");
                    fVector *= 0.9F;
                }
            } else {
                fVector /= 10;
            }
            RB.velocity -= fVector;
            
            
        }
    }
}
