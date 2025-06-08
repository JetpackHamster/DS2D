using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCrosshairScript : MonoBehaviour
{
    public GameObject[] thingsSpawnable;
    public GameObject cam;
    public PolygonCollider2D collider;
    public bool isDebugMode;
    // Start is called before the first frame update
    void Start()
    {
        // Disable VSync to use targetFrameRate
        QualitySettings.vSyncCount = 0;

        // Set target frame rate to 120 FPS
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        // RMB hold spawns things for debugging
        if(Input.GetMouseButton(1) && isDebugMode && thingsSpawnable.Length > 0)
        {
            Instantiate(thingsSpawnable[0], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        }
        // MMB hold activates collision for debugging
        if (Input.GetMouseButtonDown(2) && isDebugMode)
        {
            collider.enabled = true;
        } else if (Input.GetMouseButtonUp(2))
        {
            collider.enabled = false;
        }
        // spawn a single other thing for debugging
        if (Input.GetKeyDown(KeyCode.Slash) && isDebugMode)
        {
            Instantiate(thingsSpawnable[1], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        }
        // set position to mouse position
        Vector3 mousePos = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}
