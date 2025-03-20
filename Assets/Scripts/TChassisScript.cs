using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TChassisScript : MonoBehaviour
{
    public Rigidbody2D SRRigidbody;
    public float jumpStrength = 10;
    
    HingeJoint2D[] ArrayHJ;
    public GameObject[] ArrayWheels;
    public float wheelMotorSpeed = 600F;
    public float wheelTopSpeed = 2F;
    float wheelTargetSpeed = 0F;
    float[] wheelDiameters;
    public float fuelQty;
    public float fuelLimit;

    // Start is called before the first frame update
    void Start()
    {
        // do a little happy hop yay
        SRRigidbody.velocity = Vector2.up * 10;

        // get wheel sizes and store in array, to be used for wheelspeed sync
        wheelDiameters = new float[ArrayWheels.Length];
        for (int i = 0; i < ArrayWheels.Length; i++)
        {
            Debug.Log(ArrayWheels[i].GetComponent<CircleCollider2D>());
            Debug.Log(ArrayWheels[i].GetComponent<CircleCollider2D>().radius);
            
            wheelDiameters[i] = ArrayWheels[i].GetComponent<CircleCollider2D>().radius;
            Debug.Log("did wheel thing");
            // .GetComponent<Collider2D>() // try... () out CircleCollider2D wheelCollider
            
            
        }   

        // get hingejoints from wheel array and store in array
        ArrayHJ = new HingeJoint2D[ArrayWheels.Length];
        for (int i = 0; i < ArrayWheels.Length; i++)
        {
            ArrayHJ[i] = ArrayWheels[i].GetComponent<HingeJoint2D>();
        }

        // set fuel
        fuelQty = 20F;
        fuelLimit = 30F;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.X)) // cursed rotation
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 1 * Time.deltaTime, transform.rotation.z, transform.rotation.w);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SRRigidbody.velocity += Vector2.up * jumpStrength;
        }

        /*if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            // previously enabled useMotor for all here
        }*/
        /*if (wheelTargetSpeed != 0)
        {
            Debug.Log(wheelTargetSpeed);
        }*/
        if (Input.GetKey(KeyCode.A) && fuelQty > 0) // make wheels go more left
        {
            //SRRigidbody.velocity += Vector2.left * jumpStrength;
            
            // if not at top speed go faster
            if (wheelTargetSpeed > (wheelTopSpeed * -1))
            {
                wheelTargetSpeed -= 1F * Time.deltaTime;//(0.1F / (wheelTargetSpeed + 0.05F)) * Time.deltaTime;
            }
            setWheelSpeed(wheelTargetSpeed);
        } else {
        
            if (Input.GetKey(KeyCode.D) && fuelQty > 0) // make wheels go more right
            {
                //SRRigidbody.velocity += Vector2.right * jumpStrength;

                // if not at top speed go faster
                if (wheelTargetSpeed < wheelTopSpeed)
                {
                    wheelTargetSpeed += 1F * Time.deltaTime;//(0.1F / (wheelTargetSpeed + 0.05F)) * Time.deltaTime;
                }
                setWheelSpeed(wheelTargetSpeed);
            }
            else // deceleration
            {
                //if (false)//SRRigidbody.velocity.x > 5)
                // previously disabled useMotor for all here

                // set wheels to the speed
                setWheelSpeed(wheelTargetSpeed);
                // make the speed smaller
                wheelTargetSpeed /= (1 + 0.25F * Time.deltaTime);
                // if wheel speed very small make it nothing
                if ((wheelTargetSpeed < 0.01 && wheelTargetSpeed > 0) || (wheelTargetSpeed > -0.01 && wheelTargetSpeed < 0))
                {
                    wheelTargetSpeed = 0;
                }
                // decrease fuel qty by time and wheelspeed
                fuelQty -= wheelTargetSpeed * Time.deltaTime;
                Debug.Log("fuel: " + fuelQty);
            }

        } 
        
        //SRRigidbody.velocity += Vector2.left / 100 * transform.position.x;
    }

    // set each wheel to the speed, this does not manage engine inertia
    void setWheelSpeed(float multiplier)
    {
        // get a motor to modify, then give it back to all HingeJoint2D in array
        
        for(int i = 0; i < ArrayHJ.Length; i++)
        {
            var motor1 = ArrayHJ[i].motor;
            motor1.motorSpeed = (float)(wheelMotorSpeed * multiplier / (3.1415926535*wheelDiameters[i]));
            ArrayHJ[i].motor = motor1;
        }
    }
}
