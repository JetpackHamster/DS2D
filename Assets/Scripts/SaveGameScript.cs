using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameScript : MonoBehaviour
{
    public string filename;
    private string filePath;
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SaveGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SaveGame() {
        // save all game data to filename
        // open filename prompt to get filename from user
        // if already selected filename, just save to existing (different button "save as")
        if (filename == null) {
            // TODO: send to saveAs to get filename and append extension
            Debug.Log("Send to saveAs");
        } else {
            filePath = Path.Combine(Application.persistentDataPath, filename);
            string allData = "";
            allData += "|structures:"
            // get locations of stuctures and applicable data such as tradestation upgradelists from terrain parent & terrain
            for (int i = 0; i < GameObject.Find("TerrainParent").transform.childCount; i++) {
                for (int j = 0; j < GameObject.Find("TerrainParent").transform.GetChild(i).transform.childCount; j++) {
                    // add localPosition of each structure // check if readable numbers, maybe convert
                    allData += "" + GameObject.Find("TerrainParent").transform.GetChild(i).transform.GetChild(j).transform.localPosition + ";";
                    // if tradestation, add instance specific data (upgradeList)
                    if (GameObject.Find("TerrainParent").transform.GetChild(i).transform.GetChild(j).transform.name.equals("TradeStation(Clone)")) {
                        allData += "upgradeList:" + GameObject.Find("TerrainParent").transform.GetChild(i).transform.GetChild(j).GetComponent<TradeStationScript>().upgradeList + ";endobj;";
                    } else {
                        allData += "endobj;";
                    }
                }
            }
            // get locations of existing scrap in magneticScrap/Items layer and any other items
            allData += "|items:";
            GameObject[] items = GameObject.FindGameObjectsWithTag("MagneticScrap");
            for (item : items) {
                // add localPosition and value of each item // check if readable numbers, maybe convert
                allData += "" + item.transform.localPosition + ";" + item.GetComponent<ScrapScript>().value + ";endobj;"
            }
            // get location of player vehicle, and state of upgrades, fuel, etc
            allData += "|playervehicle:";
            GameObject TChassis = GameObject.Find("TChassis");
            var TChassisScript = GameObject.Find("TChassis").GetComponent<TChassisScript>();
            allData += TChassis.transform.position + ";";
            allData += TChassisScript.motorTopSpeed + ";";
            allData += TChassisScript.idleSpeed + ";";
            allData += TChassisScript.EngineSpeed + ";";
            allData += TChassisScript.EngineAccel + ";";
            allData += TChassisScript.fuelQty + ";";
            allData += TChassisScript.fuelLimit + ";";
            allData += TChassisScript.motorForce + ";";
            allData += TChassisScript.maxMotorForce + ";";
            allData += TChassisScript.minMotorForce + ";";
            allData += TChassisScript.brakingForce + ";";
            allData += TChassisScript.fuelUsageMultiplier + ";";
            allData += TChassisScript.trackWidth + ";";
            // get crane magnet properties // TODO: verify unprocessed arrays readable
            allData += TChassis.transform.GetChild(7).GetComponent<CraneMagnetScript>().xyLimits + ";";
            allData += "endobj;";
            // TODO: ensure terrain generation same, maybe save terrain data
            
            // save all above data to file, might make new file if none exists
            File.WriteAllText(filePath, allData);
        }
        
    }
}
