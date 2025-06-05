using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            allData += "|structures;";
            // get locations of stuctures and applicable data such as tradestation upgradelists from terrain parent & terrain
            for (int i = 0; i < GameObject.Find("TerrainPieces").transform.childCount; i++) {
                for (int j = 0; j < GameObject.Find("TerrainPieces").transform.GetChild(i).transform.childCount; j++) {
                    // add localPosition of each structure // check if readable numbers, maybe convert
                    allData += "" + GameObject.Find("TerrainPieces").transform.GetChild(i).transform.GetChild(j).transform.name + ";" + GameObject.Find("TerrainPieces").transform.GetChild(i).transform.GetChild(j).transform.localPosition + ";";
                    // if tradestation, add instance specific data (upgradeList)
                    if (GameObject.Find("TerrainPieces").transform.GetChild(i).transform.GetChild(j).transform.name.Equals("TradeStation(Clone)")) {
                        allData += "upgradeList;";
                        for(int k = 0; k < GameObject.Find("TerrainPieces").transform.GetChild(i).transform.GetChild(j).GetComponent<TradeStationScript>().upgradeList.Length; k++) {
                            allData += GameObject.Find("TerrainPieces").transform.GetChild(i).transform.GetChild(j).GetComponent<TradeStationScript>().upgradeList[k].transform.name + ";";
                        }
                        allData += "endobj;";
                    }
                }
            }
            // get locations of existing scrap in magneticScrap/Items layer and any other items
            allData += "|items;";
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            for (int i = 0; i < items.Length; i++) {
                // add localPosition, rotation, and value of each item // check if readable numbers, maybe convert // some items don't have ScrapScript
                allData += "" + items[i].transform.position + ";";
                allData += "r" + items[i].transform.rotation + ";";
                
                items[i].TryGetComponent<ScrapScript>(out ScrapScript script);
                if (script != null) {
                    allData += "v" + script.value + ";" + items[i].GetComponent<ScrapScript>().spawnIndex + ";";
                } else {
                    Debug.Log("unsaveable item (" + items[i].transform.name + "). enjoy not having it!");
                }
                allData += "endobj;";
            }
            // get location of player vehicle, and state of upgrades, fuel, etc
            allData += "|playervehicle;";
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
            // get crane magnet properties
            for(int i = 0; i < TChassis.transform.GetChild(7).GetComponent<CraneMagnetScript>().xyLimits.Length; i++) {
                allData += TChassis.transform.GetChild(7).GetComponent<CraneMagnetScript>().xyLimits[i] + ";";
            }
            allData += "r" + TChassis.transform.rotation + ";";
            allData += "endobj;";
            // save terrain offset
            allData += "|terrain;";
            allData += GameObject.Find("TerrainPieceManager").GetComponent<TerrainManagerScript>().terrainOffset + ";";
            allData += "endobj;";

            // save all above data to file, might make new file if none exists
            Debug.Log(filePath);
            File.WriteAllText(filePath, allData);
        }
        
    }
}
