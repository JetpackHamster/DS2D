using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.UI;
using UnityEngine.UIElements;

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
        float IndicatorZ = gameObject.transform.GetChild(0).transform.eulerAngles.z;
        if (IndicatorZ > 180) {
            IndicatorZ -= 360;
        }
        float targetZ = (Mathf.Abs(chassis.GetComponent<TChassisScript>().GetComponent<TChassisScript>().EngineSpeed)
         / chassis.GetComponent<TChassisScript>().motorTopSpeed) * (-160F) + (80F);
        
        float rSpeed = (20F + chassis.GetComponent<TChassisScript>().EngineAccel / 2F) * Time.deltaTime * (targetZ > IndicatorZ ? 10F : -10F);
        
        // if close enough to overshoot in this frame, decrease speed to exact distance
        if (Mathf.Abs(targetZ - IndicatorZ) < Mathf.Abs(rSpeed)) {
            rSpeed = targetZ - IndicatorZ;
        }
        // .Rotate uses euler angles
        gameObject.transform.GetChild(0).transform.Rotate(0, 0, rSpeed);

        // rotate fuel indicator
        targetZ = (chassis.GetComponent<TChassisScript>().fuelQty / chassis.GetComponent<TChassisScript>().fuelLimit) * (-160F) + (80F);
        IndicatorZ = gameObject.transform.GetChild(1).transform.eulerAngles.z;
        if (IndicatorZ > 180) {
            IndicatorZ -= 360;
        }
        rSpeed = (20F + chassis.GetComponent<TChassisScript>().EngineAccel / 2F) * Time.deltaTime * (targetZ > IndicatorZ ? 10F : -10F);
        
        // if close enough to overshoot in this frame, decrease speed to exact distance
        if (Mathf.Abs(targetZ - IndicatorZ) < Mathf.Abs(rSpeed)) {
            rSpeed = targetZ - IndicatorZ;
        }
        // .Rotate uses euler angles
        gameObject.transform.GetChild(1).transform.Rotate(0, 0, rSpeed);

        /*if (IndicatorZ < targetZ) {
            // .Rotate uses euler angles
            gameObject.transform.GetChild(1).transform.Rotate(0, 0, (20F) * Time.deltaTime);
        } else if (IndicatorZ > targetZ) {
            gameObject.transform.GetChild(1).transform.Rotate(0, 0, (-20F) * Time.deltaTime);
        }*/

        // change fuel display
        string fuelq = ("" + chassis.GetComponent<TChassisScript>().fuelQty);
        gameObject.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = (fuelq.Substring(0, fuelq.IndexOf(".") + 2) + "L");

    }
}
