using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) // when enter collector destroy seeked scrap and reward player
    {
        if (UnityEditor.ArrayUtility.Contains<GameObject>(GameObject.Find("TradeStation").GetComponent<TradeStationScript>().seekedObjs, collision.gameObject)) {
            var TCScript = GameObject.Find("TChassis").GetComponent<TChassisScript>();
            float value = 3F;
            if (TCScript.fuelQty < TCScript.fuelLimit - value) {
                TCScript.fuelQty += value;
            } else {
                TCScript.fuelQty += (TCScript.fuelLimit - TCScript.fuelQty);
            }
            GameObject.Destroy(collision.gameObject);
        }
    }
}
