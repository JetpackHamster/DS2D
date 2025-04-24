using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeCluster : MonoBehaviour
{
    //script chassisScript;
    
    // Start is called before the first frame update
    void Start()
    {
        //script chassisScript = GameObject.Find("TChassis").GetComponent<TChassisScript>();
        Debug.Log(typeof(GameObject.Find("TChassis").GetComponent<TChassisScript>()));
    }

    // Update is called once per frame
    void Update()
    {
        /*// rotate RPM indicator
        if (gameObject.transform.GetChild(0).transform.Rotation.z < ((chassisScript.EngineSpeed / chassisScript.motorTopSpeed) * -100F)) {
            // .Rotate uses euler angles
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, (20F + chassisScript.EngineAccel / 2F) * Time.deltaTime);
        } else if (gameObject.transform.GetChild(0).transform.Rotation.z > ((chassisScript.EngineSpeed / chassisScript.motorTopSpeed) * -100F)) {
            gameObject.transform.GetChild(0).transform.Rotate(0, 0, (-20F + chassisScript.EngineAccel / 2F) * Time.deltaTime);
        }

        // rotate fuel indicator
        if (gameObject.transform.GetChild(1).transform.Rotation.z < ((chassisScript.fuelQty / chassisScript.fuelLimit) * -100F)) {
            // .Rotate uses euler angles
            gameObject.transform.GetChild(1).transform.Rotate(0, 0, (20F) * Time.deltaTime);
        } else if (gameObject.transform.GetChild(1).transform.Rotation.z > ((chassisScript.fuelQty / chassisScript.fuelLimit) * -100F)) {
            gameObject.transform.GetChild(1).transform.Rotate(0, 0, (-20F) * Time.deltaTime);
        }*/


    }
}
