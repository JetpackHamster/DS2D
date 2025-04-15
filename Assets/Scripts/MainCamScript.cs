using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamScript : MonoBehaviour
{
    public float camZoomSpeed;
    public float maxZoomSize;
    public float minZoomSize;

    float camZoomV = 0;

    public GameObject cam;
    public GameObject playerObj;

    public GameObject UIStructure;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // decrease cam zoom speed to allow smooth camera zoom
        camZoomV /= (1F + (50F * Time.deltaTime));
        // camsize add camZoomV
        if (camZoomV > 0 && cam.GetComponent<Camera>().orthographicSize > minZoomSize)
        {
            cam.GetComponent<Camera>().orthographicSize -= camZoomV;
        } else if (camZoomV < 0 && cam.GetComponent<Camera>().orthographicSize < maxZoomSize)
        {
            cam.GetComponent<Camera>().orthographicSize -= camZoomV;
        }
        cam.transform.position = new Vector3 (playerObj.transform.position.x, playerObj.transform.position.y, -10F);
        // if camspeed low, set to 0
        if (Mathf.Abs(camZoomV) < 0.1)
        {
            camZoomV = 0;
        }
        // add mouseScroll input to camera zoom velocity
        camZoomV += Input.mouseScrollDelta.y * camZoomSpeed * 1000 * Time.deltaTime;
        /*if (camZoomV != 0) {
            Debug.Log(camZoomV);
        }*/
        if (cam.GetComponent<Camera>().orthographicSize > maxZoomSize) {
            cam.GetComponent<Camera>().orthographicSize = maxZoomSize;
        } else if (cam.GetComponent<Camera>().orthographicSize < minZoomSize) {
            cam.GetComponent<Camera>().orthographicSize = minZoomSize;
        }
    }
}
