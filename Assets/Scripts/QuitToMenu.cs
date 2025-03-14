using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var button = GetComponentInChildren<Button>(false); //gameObject.GetComponent("Button");
        //button.clicked += LoadNew();
        //Debug.Log("whaow");

        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadMenu()
    {
        Debug.Log("LoadMenu Attempt");
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    //    return null;
    }
}
