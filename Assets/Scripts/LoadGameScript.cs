using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.Search;


public class LoadGameScript : MonoBehaviour
{
    public string filename;
    private string filePath;
    //AsyncOperation unloadtask;

    // Start is called before the first frame update
    void Start()
    {
        // add this to list of persistent between scenes
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (unloadtask != null) {
            if (!unloadtask.isDone) {
                Debug.Log("unloadprogress: " + unloadtask.progress);
            }
        }*/
    }

    public void LoadGame()
    {
        Debug.Log("LoadGame Attempt");
        SceneManager.LoadScene("Level0", LoadSceneMode.Single);
        // disable scrap spawning in menu
        //GameObject.Find("MMScrapSpawner").SetActive(false);
        //GameObject.Find("EventSystem").SetActive(false);
        
        // load file
        if (filename == null)
        {
            // TODO: open prompt to get filename and append extension
            Debug.Log("Open filename prompt");
        }
        else
        {
            filePath = Path.Combine(Application.persistentDataPath, filename);
            string allData = File.ReadAllText(filePath);

            // *begin epic data parsing montage*

            // parse structures
            string[] sortedData = allData.Split("|");
            // biiig loop
            foreach (var section in sortedData)
            {
                UnityEditor.Search.StringView sectionView = new StringView(section);
                if (sectionView.Contains("structures;"))
                { // if Contains thing do thing
                    // section start
                    string position = "";
                    string type = ""; ;
                    GameObject newStructure;
                    string[] upgrades;

                    // maybe split by endobj; instead
                    string[] dicedData = section.Split(";");
                    for (int i = 1; i < dicedData.Length; i++)
                    {
                        // 
                        if (new UnityEditor.Search.StringView(dicedData[i]).StartsWith('('))
                        {
                            // structure position
                            position = dicedData[i];
                        }
                        else if (dicedData[i].Equals("upgradeList"))
                        {
                            type = "TradeStation";

                            // maybe use map of type strings to obj prefabs
                            // clear upgrades list if default not empty
                        }
                        else if (new UnityEditor.Search.StringView(dicedData[i]).Contains("Upgrade"))
                        {
                            // add this upgrade to the list for instantiated structure
                        }
                        else if (dicedData[i].Equals("endobj"))
                        {
                            // TODO: instantiate newStructure from type map with data


                            // add data if applicable
                            if (type.Equals("TradeStation"))
                            {
                                // add upgrades
                                //UnityEditor.ArrayUtility.Add(ref newStructure.GetComponent<TradeStationScript>().upgradeList, upgrades[]//index
                            }

                            // reset variables position, type, newStructure
                            type = "";
                            position = "";
                            newStructure = null;
                        }
                    }
                }
                else if (sectionView.Contains("items;"))
                {
                    // section start
                    string position;
                    string value;

                    // maybe split by endobj; instead
                    string[] dicedData = section.Split(";");
                    for (int i = 1; i < dicedData.Length; i++)
                    {
                        // 
                        if (new UnityEditor.Search.StringView(dicedData[i]).StartsWith('('))
                        {
                            // item position
                            position = dicedData[i];
                        }
                        else if (dicedData[i].Equals("endobj"))
                        {
                            // TODO: instantiate newStructure from type map with data
                        }
                        else if (new UnityEditor.Search.StringView(dicedData[i]).StartsWith('v'))
                        {
                            // has value, assign without indicator ("v2.6" -> "2.6")
                            value = dicedData[i].Substring(1, dicedData[i].Length);
                        }
                    }
                }
                else if (sectionView.StartsWith("terrain;"))
                {
                    // get and assign terrain offset
                    Debug.Log(float.Parse(sectionView.Substring(8, sectionView.length - 2).ToString())); // TODO: fix
                    GameObject.Find("TerrainPieceManager").GetComponent<TerrainManagerScript>().terrainOffset = float.Parse(sectionView.Substring(8, sectionView.length - 2).ToString());
                }
            }
        }
        //Debug.Log("unloading menu");
        //unloadtask = SceneManager.UnloadSceneAsync("MainMenuScene");
    }
}
