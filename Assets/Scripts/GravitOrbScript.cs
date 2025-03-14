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

            // limit force by checking for very close
            if (Mathf.Abs(xdist) < 0.2)
            {
                if (Mathf.Abs(ydist) < 2)
                {
                    // both x,y vel changed
                    RB.velocity = new Vector2(
                        RB.velocity.x - (100F * Time.deltaTime) / (Mathf.Pow(xdist, 2)),
                        RB.velocity.y - (100F * Time.deltaTime) / (Mathf.Pow(ydist, 2))
                        );
                } else
                {
                    // just x vel changed
                    RB.velocity = new Vector2(
                        RB.velocity.x - (100F * Time.deltaTime) / (Mathf.Pow(xdist, 2)),
                        RB.velocity.y
                        );
                }
                
            } else
            {
                // just y vel changed
                RB.velocity = new Vector2(
                    RB.velocity.x,
                    RB.velocity.y - (100F * Time.deltaTime) / (Mathf.Pow(ydist, 2))
                    );
            }
            
        }
    }
}
