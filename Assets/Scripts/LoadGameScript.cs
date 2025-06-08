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
    public GameObject newVehicle;
    public bool loaded;
    public bool loading;
    public bool doLoadGame;
    public bool isNewGame;
    public float loadingTime;
    //AsyncOperation unloadtask;

    // Start is called before the first frame update
    void Start()
    {
        // delete self if not first
        if (GameObject.Find("GameLoader") != gameObject) {
            GameObject.Destroy(gameObject);
        } else {
            // add this to list of persistent between scenes
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (unloadtask != null) {
            if (!unloadtask.isDone) {
                Debug.Log("unloadprogress: " + unloadtask.progress);
            }
        }*/

        if(!loaded && doLoadGame) {
            LoadGame();
        } else if(!loaded && isNewGame) {
            newGame();
        } else if (loaded && (doLoadGame || isNewGame) && loading) {
            // reset
            loaded = false;
            doLoadGame = false;
            isNewGame = false;
            loading = false;
        }
    }

    public void newGame() {
        isNewGame = true;
        Debug.Log("LoadNew Attempt");
        if(!loading) {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            loading = true;
        }
        // check if loaded
        if (GameObject.Find("TChassis") == null) {
            //Debug.Log("didn't find");
        } else {
            // set offset
            GameObject.Find("TerrainPieceManager").GetComponent<TerrainManagerScript>().terrainOffset = Random.Range(100F, 1000F);
            // mark successful
            loaded = true;
        }
    }

    public void LoadGame()
    {
        Debug.Log("LoadGame Attempt");

        // record time to load so far and stop attempts if too long
        loadingTime += Time.deltaTime;
        if (loadingTime > 2F)
        {
            Debug.Log("Loading Incomplete");
            loaded = true;
        }

        if (!loading)
        {
            SceneManager.LoadScene("Level0", LoadSceneMode.Single);
            loading = true;
        }
        // disable scrap spawning in menu
        //GameObject.Find("MMScrapSpawner").SetActive(false);
        //GameObject.Find("EventSystem").SetActive(false);

        // check if loaded
        if (GameObject.Find("TChassis") == null) {
            //Debug.Log("didn't find");
        } else {

            GameObject.Find("TerrainPieceManager").GetComponent<TerrainManagerScript>().isLoading = true;
            
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
                        string position = "";
                        string rotation = "";
                        string value = "";

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
                            else if (new UnityEditor.Search.StringView(dicedData[i]).StartsWith('r'))
                            {
                                // has rotation, assign without indicator ("r(2,6)" -> "(2,6)")
                                rotation = dicedData[i].Substring(1, dicedData[i].Length - 1);
                            }
                            else 
                            {
                                // instantiate new with index and data
                                //var newItem = Instantiate(GameObject.Find("TerrainPieceManager").GetComponent<TerrainManagerScript>().spawnedObjs[int.Parse(dicedData[i])], position, rotation);
                                if(!value.Equals("")) {
                                    //newItem.GetComponent("ScrapScript").value = value;
                                }
                            }
                        }
                    }
                    else if (sectionView.StartsWith("playervehicle;"))
                    {
                        // section start
                        string position;

                        // maybe split by endobj; instead
                        string[] dicedData = section.Split(";");
                        for (int i = 1; i < dicedData.Length; i++)
                        {
                            // 
                            if (new UnityEditor.Search.StringView(dicedData[i]).StartsWith('('))
                            {
                                // vehicle position // TODO: rotation saveload
                                position = dicedData[i];
                                string[] values = position.Trim('(', ')').Split(',');
                                
                                GameObject.Find("TChassis").transform.position = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));

                            } else {
                                var chassisScript = GameObject.Find("TChassis").GetComponent<TChassisScript>();
                                
                                chassisScript.motorTopSpeed = float.Parse(dicedData[2]);
                                chassisScript.idleSpeed = float.Parse(dicedData[3]);
                                chassisScript.EngineSpeed = float.Parse(dicedData[4]);
                                chassisScript.EngineAccel = float.Parse(dicedData[5]);
                                chassisScript.fuelQty = float.Parse(dicedData[6]);
                                chassisScript.fuelLimit = float.Parse(dicedData[7]);
                                chassisScript.motorForce = float.Parse(dicedData[8]);
                                chassisScript.maxMotorForce = float.Parse(dicedData[9]);
                                chassisScript.minMotorForce = float.Parse(dicedData[10]);
                                chassisScript.brakingForce = float.Parse(dicedData[11]);
                                chassisScript.fuelUsageMultiplier = float.Parse(dicedData[12]);
                                chassisScript.trackWidth = float.Parse(dicedData[13]);
                                GameObject.Find("CraneMagnet").GetComponent<CraneMagnetScript>().xyLimits[0] = float.Parse(dicedData[14]);
                                GameObject.Find("CraneMagnet").GetComponent<CraneMagnetScript>().xyLimits[1] = float.Parse(dicedData[15]);
                                GameObject.Find("CraneMagnet").GetComponent<CraneMagnetScript>().xyLimits[2] = float.Parse(dicedData[16]);
                                GameObject.Find("CraneMagnet").GetComponent<CraneMagnetScript>().xyLimits[3] = float.Parse(dicedData[17]);
                                GameObject.Find("CraneMagnet").GetComponent<CraneMagnetScript>().ResizeRails();
                                
                                
                                
                            }
                        }


                    }
                    else if (sectionView.StartsWith("terrain;"))
                    {
                        // get and assign terrain offset
                        Debug.Log(sectionView.Substring(8, sectionView.length - 16)); // TODO: fix string format error, reading incorrect thing?
                        GameObject.Find("TerrainPieceManager").GetComponent<TerrainManagerScript>().terrainOffset = float.Parse(sectionView.Substring(8, sectionView.length - 16).ToString());
                    }
                }
            }
            //Debug.Log("unloading menu");
            //unloadtask = SceneManager.UnloadSceneAsync("MainMenuScene");

            // mark successful
            loaded = true;
        }
    }
}
