using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    float timer = 0F;
    public GameObject bean;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 35 || transform.position.x < -35
            || transform.position.y > 25 || transform.position.y < -15)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
        if (timer > 2) {
            timer = 0;
            Instantiate(bean, transform.position, transform.rotation);
        }
    }
}
