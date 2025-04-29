using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    public GameObject can;
    private GameObject newCan;
    float limit;
    // Start is called before the first frame update
    void Start()
    {
        limit = can.GetComponent<FuelCanScript>().limit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) // when enter collector destroy seeked scrap and reward player
    {
        if (UnityEditor.ArrayUtility.Contains<GameObject>(gameObject.GetComponentInParent<TradeStationScript>().seekedObjs, collision.gameObject)) {
            var TCScript = GameObject.Find("TChassis").GetComponent<TChassisScript>();
            float value = collision.GetComponent<ScrapScript>().value;
            // if enough capacity for fuel, add it
            if (TCScript.fuelQty < TCScript.fuelLimit - value) {
                TCScript.fuelQty += value;
            } else {
                // put excess fuel into cans
                float excess = value - (TCScript.fuelLimit - TCScript.fuelQty);
                TCScript.fuelQty += (TCScript.fuelLimit - TCScript.fuelQty);
                while (excess > limit) {
                    makeCan(limit);
                    excess -= limit;
                }
                makeCan(excess);
            }
            UnityEditor.ArrayUtility.Remove(ref gameObject.GetComponentInParent<TradeStationScript>().seekedObjs, collision.gameObject);
            GameObject.Destroy(collision.gameObject);
        }
    }
    // spawn new fuelcan
    void makeCan(float value) {
        newCan = Instantiate(can, transform.position, transform.rotation);
        newCan.GetComponent<FuelCanScript>().fuelQty = value;
    }
}
