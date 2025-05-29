using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartNewGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // add this to list of persistent between scenes
        GameObject.DontDestroyOnLoad(gameObject);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadNew);
        // enable scrap spawning in menu
        GameObject.Find("MMScrapSpawner").SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadNew()
    {
        GameObject.Find("GameLoader").GetComponent<LoadGameScript>().newGame();
    }
}
