using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadGame : MonoBehaviour
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

    void LoadGameThing() 
    {
        Debug.Log("LoadGame Attempt");
        
    }
}
