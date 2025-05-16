using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadGameThing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGameThing() {
        GameObject.Find("GameLoader").GetComponent<LoadGameScript>().LoadGame();
    }
}
