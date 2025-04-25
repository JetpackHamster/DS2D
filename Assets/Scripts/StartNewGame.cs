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
        //var button = GetComponentInChildren<Button>(false); //gameObject.GetComponent("Button");
        //button.clicked += LoadNew();
        //Debug.Log("whaow");

        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadNew);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadNew()
    {
        //Debug.Log("LoadNew Attempt");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    //    return null;
    }
}
