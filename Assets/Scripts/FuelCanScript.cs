using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelCanScript : MonoBehaviour
{
    GameObject cam;
    GameObject pointer;
    GameObject chassis;
    public float fuelQty;
    public float fRate;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        pointer = GameObject.Find("MouseCrosshair");
        chassis = GameObject.Find("TChassis");
        gameObject.GetComponentInChildren<TMP_Text>().text = (("" + fuelQty).Substring(0, ("" + fuelQty).IndexOf(".") + 2) + "L");

    }

    // Update is called once per frame
    void Update()
    {
        // detect when player uses the fuelcan
        if(Input.GetKey(KeyCode.F)) {
            if(Vector3.Distance(pointer.transform.position, transform.position) < 1) {
                // if enough fuel left for full flow rate
                if (fuelQty > fRate * Time.deltaTime) {
                    fuelQty -= fRate * Time.deltaTime;
                    chassis.GetComponent<TChassisScript>().fuelQty += fRate * Time.deltaTime;
                // else transfer remaining
                } else if (fuelQty > 0) {
                    chassis.GetComponent<TChassisScript>().fuelQty += fuelQty;
                    fuelQty = 0;
                }
                gameObject.GetComponentInChildren<TMP_Text>().text = (("" + fuelQty).Substring(0, ("" + fuelQty).IndexOf(".") + 2) + "L");
            }
        }
    }
}
