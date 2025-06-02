using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapScript : MonoBehaviour
{
    public float value;
    public bool disco;
    float discoDelay;
    public int spawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (value == 0) {
            value = 2.5F;
        }
        if (Random.Range(0F, 10F) > 9.7F) {
            disco = true;
            value *= 5;
            discoDelay = 0F;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (disco) {
            if (discoDelay < 0.1f) {
                discoDelay += Time.deltaTime;
            } else {
                discoDelay = 0;
                gameObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                //Debug.Log(gameObject.GetComponent<SpriteRenderer>().color);
            }
        }
    }
}
