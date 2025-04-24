using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GaugeCluster : MonoBehaviour
{
    GameObject chassis;
    
    // Start is called before the first frame update
    void Start()
    {
        chassis = GameObject.Find("TChassis");
        //Debug.Log("" + typeof(GameObject.Find("TChassis").GetComponent<TChassisScript>()));
    }

    // Update is called once per frame
    void Update()
    {
        // rotate RPM indicator
        if (gameObject.transform.GetChild(0).transform.rotation.z < ((chassis.GetComponent<TChassisScript>().GetComponent<TChassisScript>().EngineSpeed / chassis.GetComponent<TChassisScript>().motorTopSpeed) * -0.6F - 0.6F)) {
            // .Rotate uses euler angles
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, (20F + chassis.GetComponent<TChassisScript>().EngineAccel / 2F) * Time.deltaTime * 20F);
        } else if (gameObject.transform.GetChild(0).transform.rotation.z > ((chassis.GetComponent<TChassisScript>().EngineSpeed / chassis.GetComponent<TChassisScript>().motorTopSpeed) * -0.6F - 0.6F)) {
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, (-20F + chassis.GetComponent<TChassisScript>().EngineAccel / 2F) * Time.deltaTime * 20F);
        }

        // rotate fuel indicator
        if (gameObject.transform.GetChild(1).transform.rotation.z < ((chassis.GetComponent<TChassisScript>().fuelQty / chassis.GetComponent<TChassisScript>().fuelLimit) * -0.6F - 0.6F)) {
            // .Rotate uses euler angles
            gameObject.transform.GetChild(1).transform.Rotate(0, 0, (20F) * Time.deltaTime);
        } else if (gameObject.transform.GetChild(1).transform.rotation.z > ((chassis.GetComponent<TChassisScript>().fuelQty / chassis.GetComponent<TChassisScript>().fuelLimit) * -0.6F - 0.6F)) {
            gameObject.transform.GetChild(1).transform.Rotate(0, 0, (-20F) * Time.deltaTime);
        }   

        // change fuel display
        string fuelq = ("" + chassis.GetComponent<TChassisScript>().fuelQty);
        gameObject.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = (fuelq.Substring(0, fuelq.IndexOf(".") + 2) + "L");

    }
}
