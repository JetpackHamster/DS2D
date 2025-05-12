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
    }
}
