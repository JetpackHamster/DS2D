using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TradeStationScript : MonoBehaviour
{
    public bool UIEnabled;
    public GameObject pointer;
    //public float[] xyLimits;
    //Vector2 fVector;
    //Vector3 parentpos;
    //int leftofXlimit;
    //int leftofYlimit;
    Canvas canvas;
    public GameObject[] sellList;
    public GameObject[] sellTiles;
    public GameObject[] seekedObjs; // objects player has clicked to sell but aren't yet collected
    private float magMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("StructureUICanvas").GetComponent<Canvas>();
        UIEnabled = false;
        canvas.enabled = false;
        //parentpos = GetComponentInParent<Transform>(false).position;
        sellList = new GameObject[12];
        sellTiles = new GameObject[12];
        seekedObjs = new GameObject[12];
        magMultiplier = 1F;
        
    }
    // check direction first float is from second float
    private int directionOfLimit(float pos, float limit) {
        if (pos >= limit) {
            return -1;
        } else {
            return 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // enable UI when player enter, add sell objects to list
    {
        Debug.Log("enter: " + collision.transform.name);
        if (collision.transform.name == "Player") { // display UI open prompt (currently full UI) if player detected
            //Debug.Log("playerentered");
            UIEnabled = true;
            canvas.enabled = true;
        } else if (!sellList.Contains(collision)) { // if not player and not in sell list, add to sell list
            int i = 0;
            while(sellList[i] != null) { // find empty slot index
                i++;
            }
            if (i < sellList.length()) { // add to slot if within limits
            sellList[i] = collision;
            } else {
                Debug.Log("sellList full");
            }

            // create new image tile in canvas
            sellTiles[i] = Instantiate(sellTile, canvas.transform.position, canvas.transform.rotation);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Player") { // disable UI when player leave
            UIEnabled = false;
            canvas.enabled = false;
        } else if (!sellList.Contains(collision)) { // if not player and in sell list, remove from sell list
            sellList.remove(collision);
        }
    }

    // called when something is in the trigger area
    private void OnTriggerStay2D(Collider2D collision) {
        //Debug.Log("thing present: " + collision.transform.name);
        
        if (collision.transform.name != "Player") { // if not player
            Rigidbody2D otherRB = collision.GetComponentInParent<Rigidbody2D>();
            
            // if other is seeked, increase other's velocity toward magnet
            if (/*Input.GetKey(KeyCode.T) &&*/ seekedObjs.contains(collision)) // if in seeked objs, attract
            {
                // find distances to offset point
                float xdist = collision.GetComponentInParent<Transform>().position.x - (transform.position.x-1F);
                float ydist = collision.GetComponentInParent<Transform>().position.y - (transform.position.y + 2F);
                float totaldist = Mathf.Sqrt(Mathf.Pow(xdist, 2) + Mathf.Pow(ydist, 2));

                Vector2 fVector = new Vector2(xdist, ydist); // set vector direction
                fVector.Normalize(); // set vector magnitude to 1
                fVector *= Time.deltaTime;
                
                // set to correct magnitude unless very close
                if(totaldist > 0.5) {
                    fVector *= (100/Mathf.Pow(totaldist, 2));
                    /*while (fVector.magnitude > 10) { // force limit
                        Debug.Log("force over limit");
                        fVector *= 0.9F;
                    }*/
                } else {
                    fVector /= 10;
                }
                fVector *= magMultiplier;
                otherRB.velocity -= fVector;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        // toggle UI?
        /*if (Input.GetKeyDown(KeyCode.U)) {
            UIEnabled = !UIEnabled;
            //Debug.Log("UIEnabled: " + UIEnabled);
        }*/
        
        if (seekedObjs.isEmpty()){ // reset magMultiplier when all seekedObjs collected
            Debug.Log("magMultiplier reset, value was " + magMultiplier);
            magMultiplier = 1F;
        } else {
            magMultiplier += 0.1F * Time.deltaTime; // increase multiplier as long as magnet in use
        }
    }
}
