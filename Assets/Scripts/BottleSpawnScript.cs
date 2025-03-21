using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSpawnScript : MonoBehaviour
{
    public GameObject spawnedObj;
    public float spawnDelay = 0.25F;
    private float timer = 0;
    public float heightOffset = 4;


    // Start is called before the first frame update
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnDelay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            
            spawn();
            timer = 0;

        }
    }

    void spawn()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(spawnedObj, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
