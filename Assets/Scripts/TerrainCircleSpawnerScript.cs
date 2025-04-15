using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainCircleSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public GameObject cam;
    public float spawnRate;
    private float spawnerX;
    private float timer = 0;
    public float heightOffset;
    public GameObject[] spawnedObjs;
    public float tradeStructureTimer = 0;
    public float tradeStructureSpawnRate;
    public GameObject[] structures;
    private float temp;
    public Transform circleParent;
    public bool moving;

    //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.PerlinNoise1D.html


    // Start is called before the first frame update
    void Start()
    {
        spawnerX = transform.position.x;
        //spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        // if player close enough, spawn more terrain
        if (cam.transform.position.x > spawnerX - 150)
        {
            moving = true;
            if (timer < spawnRate) {
                timer += Time.deltaTime;
            } else {
                //spawnPipe();
                timer = 0;
                heightOffset += 0.1F * Time.deltaTime;
                // spawn scrap
                if (Random.Range(0F, 10F) > 9.8F) {
                    Instantiate(spawnedObjs[Random.Range(0,spawnedObjs.Length)], new Vector3(transform.position.x, transform.position.y + 10, 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
                }
                if (tradeStructureTimer < tradeStructureSpawnRate) {
                    tradeStructureTimer += 1F * Time.deltaTime;
                } else {
                    temp = heightOffset;
                    heightOffset = 1F;
                    tradeStructureTimer = 0F;
                    Debug.Log("structure make!");
                    Instantiate(structures[0], new Vector3(transform.position.x, transform.position.y + 10, 0), new Quaternion(0F, 0F, 0F, 1F));
                }
                if (heightOffset > 1.1 && heightOffset < 2) {
                    heightOffset = temp;
                }
            }
        
            // move forward
            spawnerX += 80F - 1;//gameObject.GetComponentInChildren<TerrainManagerScript>().terrainLength;//64 * Time.deltaTime;
        } else {
            moving = false;
        }
        gameObject.transform.position = new Vector3(spawnerX, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation, circleParent);
    }
}
