using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCrosshairScript : MonoBehaviour
{
    public GameObject thingSpawnable;
    public GameObject cam;
    public PolygonCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // RMB hold spawns things
        if(Input.GetMouseButton(1))
        {
            Instantiate(thingSpawnable, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        }
        // MMB hold activates collision
        if (Input.GetMouseButtonDown(2))
        {
            collider.enabled = true;
        } else if (Input.GetMouseButtonUp(2))
        {
            collider.enabled = false;
        }
        // set position to mouse position
        Vector3 mousePos = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}
