using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sellTileScript : MonoBehaviour
{
    
    public TradeStationScript StationScript;    
    public GameObject obj;
    public GameObject source;
    public GameObject cam;
    Canvas canvas;
    public bool isUpdated;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("StructureUICanvas").GetComponent<Canvas>();
        cam = GameObject.Find("Main Camera");
        StationScript = source.GetComponentInChildren<TradeStationScript>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(sell);

        //canvas.AddListener(setup());
        
        setup();
        
    }   

    // Update is called once per frame
    void Update()
    {
        if (!isUpdated) {
            isUpdated = true;
            float tileY = 0;//-7; // why is 0 so far down on screen, is 1/2 tile from bottom, is the panel's center at the bottom?
            float tileX = source.GetComponentInChildren<TradeStationScript>().tileXOffset + (index * source.GetComponentInChildren<TradeStationScript>().tileXSpacing);
            // fit to grid
            while(tileX > source.GetComponentInChildren<TradeStationScript>().tileXlimit) {
                tileX -= source.GetComponentInChildren<TradeStationScript>().tileXlimit;// - source.GetComponentInChildren<TradeStationScript>().tileXOffset;
                tileY += source.GetComponentInChildren<TradeStationScript>().tileRowSpacing;
            }
            float camSize = (float)cam.GetComponent<Camera>().orthographicSize / 10F;
            transform.position = new Vector3(
                transform.position.x + ((tileX) * camSize),
                transform.position.y + ((tileY) * camSize),
                transform.position.z);
        }
    }

    void sell() {
        
        //Debug.Log("sell attempt: " + obj.transform.name);
        // find seekedObjs then put obj there   
        if (!UnityEditor.ArrayUtility.Contains(StationScript.seekedObjs, obj)) {
            UnityEditor.ArrayUtility.Add(ref StationScript.seekedObjs, obj);
        }

    }
    public void setup() {
        //Debug.Log("tile setup attempt");
        
        if (cam.GetComponent<MainCamScript>().UIStructure == source) { // if source is the active UI structure, setup 
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Button>().enabled = true;

        } else {
            // disable rendering
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            gameObject.GetComponent<Button>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<TMP_Text>().enabled = false;
            //Debug.Log("beef"); // "dis beef" - Red, 12:15pm, nacho day
        }
    }
}
