using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sellTileScript : MonoBehaviour
{
    
    public TradeStationScript StationScript;    
    public GameObject obj;  
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(sell);

        StationScript = GameObject.Find("TradeStation").GetComponentInChildren<TradeStationScript>();
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
}
