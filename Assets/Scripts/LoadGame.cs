using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public string filename;
    private string filePath;

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
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        
        // load file
        if (filename == null) {
            // TODO: open prompt to get filename and append extension
            Debug.Log("Open filename prompt");
        } else {
            filePath = Path.Combine(Application.persistentDataPath, filename);
            string allData = File.ReadAllText(filePath);
            
            // *begin epic data parsing montage*

            // parse structures
            string[] splitData = allData.Split(";");
            // biiig loop
            foreach (var term in splitData) {
                //if(term.) // if contains thing do thing
    
            }
        }
    }
}
