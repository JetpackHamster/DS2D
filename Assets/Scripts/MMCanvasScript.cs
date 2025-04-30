using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MMCanvasScript : MonoBehaviour
{
    public string[] splashes;

    // Start is called before the first frame update
    void Start()
    {
        // set random splash text
        GameObject.Find("SplashText").GetComponent<TMP_Text>().text = (splashes[Random.Range(0, splashes.Length)]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
