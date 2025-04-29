using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelCanScript : MonoBehaviour
{
    public GameObject cam;
    public GameObject pointer;
    public GameObject chassis;
    public float fuelQty;
    public float fRate;
    public float limit;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        pointer = GameObject.Find("MouseCrosshair");
        chassis = GameObject.Find("TChassis");
        reloadDisplay();

    }

    // Update is called once per frame
    void Update()
    {
        // detect when player uses the fuelcan
        if(Input.GetKey(KeyCode.F)) {
            if (Vector3.Distance(pointer.transform.position, transform.position) < 1) {
                Debug.Log("I have been chosen!!");
                // if enough fuel left for full flow rate
                if (fuelQty > fRate * Time.deltaTime && (chassis.GetComponent<TChassisScript>().fuelLimit - chassis.GetComponent<TChassisScript>().fuelQty) > fRate * Time.deltaTime) {
                    fuelQty -= fRate * Time.deltaTime;
                    chassis.GetComponent<TChassisScript>().fuelQty += fRate * Time.deltaTime;
                // else transfer remaining
                } else if (fuelQty > 0 && (chassis.GetComponent<TChassisScript>().fuelLimit - chassis.GetComponent<TChassisScript>().fuelQty) > fuelQty) {
                    chassis.GetComponent<TChassisScript>().fuelQty += fuelQty;
                    fuelQty = 0;
                }
                reloadDisplay();
            }
        } else if (Input.GetKey(KeyCode.G)) {
            if(Vector3.Distance(pointer.transform.position, transform.position) < 1) {
                Debug.Log("I have been chosen!!");
                // if enough capacity left for full flow rate
                if ((limit - fuelQty) > fRate * Time.deltaTime && chassis.GetComponent<TChassisScript>().fuelQty > fRate * Time.deltaTime) {
                    fuelQty += fRate * Time.deltaTime;
                    chassis.GetComponent<TChassisScript>().fuelQty -= fRate * Time.deltaTime;
                // else transfer remaining
                } else if (chassis.GetComponent<TChassisScript>().fuelQty > 0 && (limit - fuelQty) > chassis.GetComponent<TChassisScript>().fuelQty) {
                    fuelQty += chassis.GetComponent<TChassisScript>().fuelQty;
                    chassis.GetComponent<TChassisScript>().fuelQty = 0;
                }
                reloadDisplay();
            }
        }
        if (Input.GetKey(KeyCode.Y)) {
            Debug.Log("Wheeeeeeeeeeeeeee");
            gameObject.GetComponent<Rigidbody2D>().velocity += Vector2.up;
        }
    }
    void reloadDisplay() {
        gameObject.GetComponentInChildren<TMP_Text>().text = (("" + fuelQty).Substring(0, ("" + fuelQty).IndexOf(".") + 2) + "L");
    }
}

