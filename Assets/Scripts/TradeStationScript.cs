using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Runtime.InteropServices;

public class TradeStationScript : MonoBehaviour
{
    public bool UIEnabled;
    private GameObject pointer;
    //public float[] xyLimits;
    //Vector2 fVector;
    //Vector3 parentpos;
    //int leftofXlimit;
    //int leftofYlimit;
    Canvas canvas;
    public GameObject sellTile;
    public GameObject[] sellList; // list of objects able to be sold
    public GameObject[] sellTiles;
    public GameObject[] seekedObjs; // objects player has clicked to sell but aren't yet collected
    public float magMultiplier; // magnet multiplier
    GameObject cam;
    GameObject TChassis;
    float whee = 0F; // amount to fling
    float wheeTimer = 0F; // how long until fling
    public float tileRowSpacing;
    public float tileXSpacing;
    public float tileXlimit;
    //public bool limitVisualizers;
    public float tileXOffset;
    public GameObject[] upgradeList;
    public GameObject[] allUpgrades;
    public int upgradeCount;
    int[] selectedIndices;

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
        cam = GameObject.Find("Main Camera");
        pointer = GameObject.Find("MouseCrosshair");
        TChassis = GameObject.Find("TChassis");
        //Button SpeedUpgradeButton = GameObject.Find("SpeedUpgradeButton").GetComponent<Button>();
        //Button AccelUpgradeButton = GameObject.Find("AccelerationUpgradeButton").GetComponent<Button>();
        //Button WheeUpgradeButton = GameObject.Find("WheeUpgradeButton").GetComponent<Button>();
        //Button FuelUsageUpgradeButton = GameObject.Find("FuelUsageUpgradeButton").GetComponent<Button>();

        // assign upgrades for this station
        upgradeList = new GameObject[upgradeCount];
        GameObject panel = GameObject.Find("Panel");
        selectedIndices = new int[upgradeCount];
        int index = 0;
        for(int i = 0; i < upgradeCount; i++) {
            while (UnityEditor.ArrayUtility.Contains(selectedIndices, index)) {
                index = Random.Range(1, allUpgrades.Length + 1);
            }
            selectedIndices[i] = index;
            upgradeList[i] = allUpgrades[index - 1];
        }

        /*SpeedUpgradeButton.onClick.AddListener(SpeedUpgrade);
        AccelUpgradeButton.onClick.AddListener(AccelUpgrade);
        WheeUpgradeButton.onClick.AddListener(WheeUpgrade);
        FuelUsageUpgradeButton.onClick.AddListener(FuelUsageUpgrade);*/
    }
    void AccelUpgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 5 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 5;
            TChassis.GetComponent<TChassisScript>().EngineAccel *= 1.5F;
        }
    }
    void SpeedUpgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 5 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 5;
            TChassis.GetComponent<TChassisScript>().motorTopSpeed *= 1.5F;
        }
    }
    void WheeUpgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 4 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 5;
            wheeTimer += 2F;
            whee++;
        }
    }
    void FuelUsageUpgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 3 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 3;
            TChassis.GetComponent<TChassisScript>().fuelUsageMultiplier *= (1F - TChassis.GetComponent<TChassisScript>().fuelUsageMultiplier / 5);
        }
    }
    void IdleSpeedUpgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 3 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 3;
            TChassis.GetComponent<TChassisScript>().idleSpeed *= (1F + TChassis.GetComponent<TChassisScript>().idleSpeed / 5);
        }
    }
    void CraneRangeUpgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 6 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 6;
            for(int i = 0; i < 4; i++) {
                TChassis.transform.GetChild(7).GetComponent<CraneMagnetScript>().xyLimits[i] *= 1.2F;
            }
            TChassis.transform.GetChild(7).GetComponent<CraneMagnetScript>().ResizeRails();
        }
    }
    /*void Upgrade() {
        if(cam.GetComponent<MainCamScript>().UIStructure == gameObject && TChassis.GetComponent<TChassisScript>().fuelQty > 6 + 1) {
            TChassis.GetComponent<TChassisScript>().fuelQty -= 6;

        }
    }*/

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

            cam.GetComponent<MainCamScript>().UIStructure = gameObject; // assign this as active UI structure

            // setup each tile
            for(int i = 0; i < sellTiles.Length; i++) {
                sellTiles[i].GetComponent<sellTileScript>().setup();
            }

            // spawn upgrade buttons
            GameObject panel = GameObject.Find("Panel");
            for(int i = 0; i < upgradeCount; i++) {
                upgradeList[i] = Instantiate(allUpgrades[selectedIndices[i] - 1], panel.transform.position, panel.transform.rotation, panel.transform);
                upgradeList[i].transform.localPosition = new Vector3(0, (225F - 50F * i), 0);
            }
            // assign each upgrade button in list to its function
            for(int i = 0; i < upgradeList.Length; i++) {
                if (upgradeList[i].transform.name == "SpeedUpgradeButton(Clone)") {
                    upgradeList[i].GetComponent<Button>().onClick.AddListener(SpeedUpgrade);
                } else if (upgradeList[i].transform.name == "AccelerationUpgradeButton(Clone)") {
                    upgradeList[i].GetComponent<Button>().onClick.AddListener(AccelUpgrade);
                } else if (upgradeList[i].transform.name == "WheeUpgradeButton(Clone)") {
                    upgradeList[i].GetComponent<Button>().onClick.AddListener(WheeUpgrade);
                } else if (upgradeList[i].transform.name == "FuelUsageUpgradeButton(Clone)") {
                    upgradeList[i].GetComponent<Button>().onClick.AddListener(FuelUsageUpgrade);
                } else if (upgradeList[i].transform.name == "IdleSpeedUpgradeButton(Clone)") {
                    upgradeList[i].GetComponent<Button>().onClick.AddListener(IdleSpeedUpgrade);
                } else if (upgradeList[i].transform.name == "CraneRangeUpgradeButton(Clone)") {
                    upgradeList[i].GetComponent<Button>().onClick.AddListener(CraneRangeUpgrade);
                }
            }

        } else if (collision.gameObject.layer == 8 && !UnityEditor.ArrayUtility.Contains<GameObject>(sellList, collision.gameObject)) { // if not player and not in sell list, add to sell list
            UnityEditor.ArrayUtility.Add(ref sellList, collision.gameObject);

            // create new image tile in canvas
            //Debug.Log("new sellTile: " + collision.transform.name);
            GameObject panel = GameObject.Find("Panel");
            /*float tileY = -7; // disabled so tiles move selves?
            float tileX = tileXOffset + sellTiles.Length * tileXSpacing;
            // fit to grid
            while(tileX > tileXlimit) {
                tileX -= tileXlimit - tileXOffset;
                tileY += tileRowSpacing;
            }*/
            // find camera size to account for size changes
            //float camSize = (float)cam.GetComponent<Camera>().orthographicSize / 10F;
            
            // instantiate new in list
            UnityEditor.ArrayUtility.Add(ref sellTiles, Instantiate(
                sellTile, new Vector3(
                    panel.transform.position.x,/* + cam.transform.position.x*/
                    panel.transform.position.y,/* + cam.transform.position.y*/
                    panel.transform.position.z/* + cam.transform.position.z*/),
                panel.transform.rotation, panel.transform)); // original, position, rotation, parent)
            
            sellTiles[sellTiles.Length-1].GetComponentInChildren<sellTileScript>().obj = collision.gameObject; // assign scrap obj
            sellTiles[sellTiles.Length-1].GetComponentInChildren<sellTileScript>().source = gameObject; // assign source structure to this
            sellTiles[sellTiles.Length-1].GetComponentInChildren<sellTileScript>().index = sellTiles.Length - 1; // tell tile positioning where it is

            // assign sprite and text with value
            sellTiles[sellTiles.Length-1].transform.GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = collision.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            sellTiles[sellTiles.Length-1].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = ("Sell-" + (("" + collision.gameObject.GetComponent<ScrapScript>().value).IndexOf(".") + 2) + "L");

            // TODO: fix text sometimes missing
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Player") { // disable UI when player leave
            UIEnabled = false;
            canvas.enabled = false;
            cam.GetComponent<MainCamScript>().UIStructure = null; // reset UIStructure

            // setup each tile to ensure correct tiles enabled
            for(int i = 0; i < sellTiles.Length; i++) {
                sellTiles[i].GetComponent<sellTileScript>().setup();
            }

            // delete buttons
            for(int i = 0; i < upgradeCount; i++) {
                GameObject.Destroy(upgradeList[i]);
                upgradeList[i] = allUpgrades[selectedIndices[i] - 1];
            }


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
            // move back all later tiles
            for(int j = index; j < sellTiles.Length; j++) {
                /*float posX = sellTiles[j].transform.position.x - tileXSpacing; // is this section just supposed to move the tiles back? the tiles fix their own position
                float posY = sellTiles[j].transform.position.y;
                if(posX < 0) {
                    posX += tileXlimit;
                    posY -= tileRowSpacing;
                }*/
                sellTiles[j].GetComponentInChildren<sellTileScript>().index = j;//transform.position = new Vector3(posX, posY, sellTiles[j].transform.position.z);
                sellTiles[j].GetComponentInChildren<sellTileScript>().isUpdated = false;
            }
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
                otherRB.WakeUp();
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
        /*if (limitVisualizers) { // debug thing i despise needing
            Instantiate(sellTile, new Vector3(GameObject.Find("Panel").transform.localPosition.y, (0), GameObject.Find("Panel").transform.localPosition.z), GameObject.Find("Panel").transform.rotation, GameObject.Find("Panel").transform).transform.localPosition = new Vector3(((tileXlimit) * 36), (0), transform.localPosition.z);
            Instantiate(sellTile, new Vector3(GameObject.Find("Panel").transform.localPosition.y, (0), GameObject.Find("Panel").transform.localPosition.z), GameObject.Find("Panel").transform.rotation, GameObject.Find("Panel").transform).transform.localPosition = new Vector3(((tileXOffset) * 36), (0), transform.localPosition.z);
        }*/

        if (whee > 0) {
            if(wheeTimer > 0) {
                wheeTimer -= Time.deltaTime;
            } else {
                whee--;
                TChassis.GetComponent<Rigidbody2D>().velocity += Vector2.right * 100;   
            }
                
        }
        // toggle UI?
        /*if (Input.GetKeyDown(KeyCode.U)) {
            UIEnabled = !UIEnabled;
            //Debug.Log("UIEnabled: " + UIEnabled);
        }*/

        if (seekedObjs.Length == 0) { // reset magMultiplier when all seekedObjs collected
            //Debug.Log("magMultiplier reset, value was " + magMultiplier);
            magMultiplier = 5F;
        } else {
            magMultiplier += 2F * Time.deltaTime; // increase multiplier as long as magnet in use
        }
    }
}
