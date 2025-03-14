using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FunctionQuit : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(quitFunction);
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.gameObject.Button.OnClick()) {
        //    Application.Quit();
        //}
    }

    void quitFunction() {
        Debug.Log("Quit Attempt");
        //Debug.Log(Random.Range(1F, 360F));
        Application.Quit();
    }
}
