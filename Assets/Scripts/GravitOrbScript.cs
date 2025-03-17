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

            // use vector.normalize like method to set vector magnitude to 1 and then set it's magnitude seperately
            // just need to get direction with these below lines

            Vector2 fVector = new Vector2(xdist, ydist);
            fVector.Normalize();

            // limit force by checking for very close
            /*if (Mathf.Abs(xdist) > 0.5F)
            {
                if (Mathf.Abs(ydist) > 0.5F)
                {
                    // both x,y vel changed
                    RB.velocity = new Vector2(
                        RB.velocity.x - (10F * Time.deltaTime) * xdir * Mathf.Abs(1/ydist)  / (Mathf.Pow(xdist, 2)),
                        RB.velocity.y - (10F * Time.deltaTime) * ydir * Mathf.Abs(1/xdist) / (Mathf.Pow(ydist, 2))
                        ); 
                        
                    
                } else
                {
                    // just x vel changed
                    RB.velocity = new Vector2(
                        RB.velocity.x - (10F * Time.deltaTime) * xdir * Mathf.Abs(1/ydist) / (Mathf.Pow(xdist, 2)),
                        RB.velocity.y
                        ); 
                }
                
            } else if (Mathf.Abs(ydist) > 0.5F)
            {
                // just y vel changed
                RB.velocity = new Vector2(
                    RB.velocity.x,
                    RB.velocity.y - (10F * Time.deltaTime) * ydir * Mathf.Abs(1/xdist) / (Mathf.Pow(ydist, 2))
                    ); 
            }*/
            
        }
    }
}
