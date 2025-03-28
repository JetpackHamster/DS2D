using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;

//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Runtime.InteropServices;

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
    public GameObject sellTile;
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
        sellList = new GameObject[0];
        sellTiles = new GameObject[0];
        seekedObjs = new GameObject[0];
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
        //Debug.Log("enter: " + collision.transform.name);
        if (collision.transform.name == "Player") { // display UI open prompt (currently full UI) if player detected
            //Debug.Log("playerentered");
            UIEnabled = true;
            canvas.enabled = true;
        } else if (collision.gameObject.layer == 8 && !UnityEditor.ArrayUtility.Contains<GameObject>(sellList, collision.gameObject)) { // if not player and not in sell list, add to sell list
            /*int i = 0;
            while(i < sellList.Length && sellList[i] != null) { // find empty slot index
                i++;
            }*/
            
            UnityEditor.ArrayUtility.Add(ref sellList, collision.gameObject);
            /*if (i < sellList.Length) { // add to slot if within limits
            sellList[i] = collision.gameObject;
            } else {
                Debug.Log("sellList full");
            }*/

            // create new image tile in canvas
            Debug.Log("new sellTile: " + collision.transform.name);
            GameObject panel = GameObject.Find("Panel");
            float tileY = 0;
            float tileX = sellTiles.Length;
            while(tileX > 7F) {
                tileX -= 8F;
                tileY += 2F;
            }
            UnityEditor.ArrayUtility.Add(ref sellTiles, Instantiate(
                sellTile, new Vector3(
                    panel.transform.position.x + (0F + (tileX * 1.5F)),
                     panel.transform.position.y + tileY, canvas.transform.position.z),
                     panel.transform.rotation, panel.transform)); // original, position, rotation, parent)
            sellTiles[sellTiles.Length-1].GetComponentInChildren<sellTileScript>().obj = collision.gameObject;
            sellTiles[sellTiles.Length-1].GetComponentInChildren<Image>().sprite = collision.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Player") { // disable UI when player leave
            UIEnabled = false;
            canvas.enabled = false;
        } else if (UnityEditor.ArrayUtility.Contains<GameObject>(sellList, collision.gameObject)) { // if not player and in sell list, remove from sell list
            //Debug.Log("Attempt remove " + collision.transform.name);
            UnityEditor.ArrayUtility.Remove(ref sellList, collision.gameObject);
            
            // find index of tile to destroy it
            int index = 0;
            for (int i = 0; i < sellTiles.Length; i++) {
                if (sellTiles[i] != null && sellTiles[i].GetComponentInChildren<sellTileScript>().obj == collision.gameObject) {
                    index = i;
                }
            }
            GameObject.Destroy(sellTiles[index]);
            UnityEditor.ArrayUtility.Remove<GameObject>(ref sellTiles, sellTiles[index]);
        // remove from seeked if applicable
        } else if (UnityEditor.ArrayUtility.Contains<GameObject>(seekedObjs, collision.gameObject)) {
            UnityEditor.ArrayUtility.Remove(ref seekedObjs, collision.gameObject);
        }
    }

    // called when something is in the trigger area
    private void OnTriggerStay2D(Collider2D collision) {
        //Debug.Log("thing present: " + collision.transform.name);
        
        if (collision.transform.name != "Player") { // if not player
            Rigidbody2D otherRB = collision.GetComponentInParent<Rigidbody2D>();
            
            // if other is seeked, increase other's velocity toward magnet
            if (/*Input.GetKey(KeyCode.T) &&*/ UnityEditor.ArrayUtility.Contains<GameObject>(seekedObjs, collision.gameObject)) // if in seeked objs, attract
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

    /*private void GameObject.Find("Collector").OnTriggerEnter2D(Collider2D collision) // when enter collector destroy and reward
    {
        if (UnityEditor.ArrayUtility.Contains<GameObject>(seekedObjs, collision.gameObject)) {
            GameObject TCScript = GameObject.Find("TChassis").GetComponent<TChassisScript>();
            float value = 3F;
            if (TCScript.fuelQty < TCScript.fuelLimit - value) {
                TCScript.fuelQty += value;
            } else {
                TCScript.fuelQty += (TCScript.fuelLimit - CScript.fuelQty);
            }
            GameObject.Destroy(collision.gameObject);
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        // toggle UI?
        /*if (Input.GetKeyDown(KeyCode.U)) {
            UIEnabled = !UIEnabled;
            //Debug.Log("UIEnabled: " + UIEnabled);
        }*/
        
        int numNull = 0; // count empty slots in array
        for(int i = 0; i < seekedObjs.Length; i++) {
            if(seekedObjs[i] == null) {
                numNull++;
            }
        }

        if (numNull ==0) { // reset magMultiplier when all seekedObjs collected
            Debug.Log("magMultiplier reset, value was " + magMultiplier);
            magMultiplier = 1F;
        } else {
            magMultiplier += 0.1F * Time.deltaTime; // increase multiplier as long as magnet in use
        }
    }
}
