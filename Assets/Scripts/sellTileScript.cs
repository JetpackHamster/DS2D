using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sellTileScript : MonoBehaviour
{
    
    public TradeStationScript StationScript;    
    public GameObject obj;
    public GameObject source;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        if (cam.GetComponent<MainCamScript>().UIStructure = source) { // if source is the active UI structure, setup 

            Button button = GetComponent<Button>();
            button.onClick.AddListener(sell);

            StationScript = source.GetComponentInChildren<TradeStationScript>();
        } else {
            gameObject.GetComponent<Image>().enabled = false;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    void sell() {
        
        Debug.Log("sell attempt: " + obj.transform.name);
        // find seekedObjs then put obj there   
        if (!UnityEditor.ArrayUtility.Contains(StationScript.seekedObjs, obj)) {
            UnityEditor.ArrayUtility.Add(ref StationScript.seekedObjs, obj);
        }

    }
    canvas.OnEnable() {

    }
}
