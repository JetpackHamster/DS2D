using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMScrapSpawnScript : MonoBehaviour
{
    public GameObject[] spawnedObjs;
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
            transform.position = new Vector3(Random.Range(-20F, 20F), transform.position.y, transform.position.z);
            spawn();
            timer = 0;

        }
    }

    void spawn()
    {
        // spawn a new object with the position of the spawner that is a random object from the list
        Instantiate(spawnedObjs[Random.Range(0,spawnedObjs.Length)], new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
    }
}
