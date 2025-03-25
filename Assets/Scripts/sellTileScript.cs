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
        
        // find first null index in seekedObjs then put obj there
        int nullIndex = UnityEditor.ArrayUtility.FindIndex(StationScript.seekedObjs, null);
        UnityEditor.ArrayUtility.Insert(ref StationScript.seekedObjs, nullIndex, obj);

    }
}
