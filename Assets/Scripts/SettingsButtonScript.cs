using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OpenMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenMenu ()
    {
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.GetComponentInChildren<TMP_Text>().enabled = false;
        
    }
}
