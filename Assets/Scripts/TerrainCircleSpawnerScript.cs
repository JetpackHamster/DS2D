using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainCircleSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public GameObject cam;
    public float spawnRate = 2;
    private float spawnerX;
    private float timer = 0;
    public float heightOffset = 4;
    public GameObject[] spawnedObjs;

    //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.PerlinNoise1D.html


    // Start is called before the first frame update
    void Start()
    {
        spawnerX = transform.position.x;
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        // if player close enough, spawn more terrain
        if (cam.transform.position.x > spawnerX - 150)
        {
            if (timer < spawnRate) {
                timer += Time.deltaTime;
            } else {
                spawnPipe();
                timer = 0;
                heightOffset += 0.005F;
                if (Random.Range(0F, 10F) > 9.8F) {
                    Instantiate(spawnedObjs[Random.Range(0,spawnedObjs.Length)], new Vector3(transform.position.x, transform.position.y + 10, 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
                }
            }
        
            // move forward
            spawnerX += 64 * Time.deltaTime;
        }
        gameObject.transform.position = new Vector3(spawnerX, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
