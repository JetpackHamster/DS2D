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
            // send to saveAs to get filename and append extension
            Debug.Log("Send to saveAs");
        } else {
            filePath = Path.Combine(Application.persistentDataPath, filename);
            string allData;
            
            // get locations of stuctures and applicable data such as tradestation upgradelists
            // get all gameobjects with terrain tag and save children data?
            // get locations of existing scrap in magneticScrap/Items layer and any other items
            // get location of player vehicle, and state of upgrades, fuel, etc
            // ensure terrain generation same, maybe save terrain data
            
            // save all above data to file
            File.WriteAllText(filePath, allData);
        }
        
    }
}
