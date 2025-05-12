using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameScript : MonoBehaviour
{
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
        // get locations of stuctures and applicable data such as tradestation upgradelists
        // get locations of existing scrap
        // get location of player vehicle, and state of upgrades, fuel, etc
        // ensure terrain generation same, maybe save data
        // 
    }
}
