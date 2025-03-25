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
    public float motorTopSpeed = 2F;
    float wheelTargetSpeed = 0F;
    public float EngineSpeed = 0F;
    float[] wheelDiameters;
    public float fuelQty;
    public float fuelLimit;
    public float motorForce;
    public float maxMotorForce = 80F;
    public float idleSpeed = 10F;
    public float frameFuelUsage = 0F;
    public float clutch = 0F;

    // torque curve should be: sqrtx - x, 0 <= x <= 0.35
    // enginespeed changed with inputs, engine torque determined by torque curve and disabled when no inputs

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
        bool lowFuelNotified = false;

        motorForce = 50F; // check original force first probably, this will be changed live according to torque curve

        /*
        plan:
        change motor speed slightly with inputs, otherwise goes to idle value
        motor speed changed slightly by difference to wheelspeed? (allow wheelspeed to affect motorspeed back)

        change motor speed direction when at idle value and hold other direction input
        set motor force to low when no inputs
        set motor force to a multiplier of the torque curve value(dependent of motorspeed) that ramps up to 1x when either input
        wheel target speed set to 0 unless inputs, then uses ramping multiplier up to motorspeed


        */
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.X)) // cursed vehicle rotation
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 1 * Time.deltaTime, transform.rotation.z, transform.rotation.w);
        }

        if (Input.GetKeyDown(KeyCode.Space)) // do a jumpy
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
        if (Input.GetKey(KeyCode.A) && fuelQty > 0) // make motorspeed go more left
        {
            //SRRigidbody.velocity += Vector2.left * jumpStrength; // wheeeee
            
            // if reverse and not at top speed go faster
            if (motorspeed < (idleSpeed / (-10F)) && wheelTargetSpeed > (motorTopSpeed * -1))
            {
               motorSpeed -= 0.1F * Time.deltaTime;
               // wheelTargetSpeed -= 1F * Time.deltaTime;//(0.1F / (wheelTargetSpeed + 0.05F)) * Time.deltaTime;
            } else if (motorspeed > (idleSpeed/ (10F))){ // braking when go forward
                motorSpeed += 0.2F * Time.deltaTime;
            }
            setWheelSpeed(wheelTargetSpeed);
        } else {
        
            if (Input.GetKey(KeyCode.D) && fuelQty > 0) // make wheels go more right
            {
                //SRRigidbody.velocity += Vector2.right * jumpStrength;

                // if not at top speed go faster
                if (wheelTargetSpeed < motorTopSpeed)
                {
                    wheelTargetSpeed += 1F * Time.deltaTime;//(0.1F / (wheelTargetSpeed + 0.05F)) * Time.deltaTime;
                }
                setWheelSpeed(wheelTargetSpeed);
            }
            else // deceleration (to idle motor speed)
            {
                //if (false)//SRRigidbody.velocity.x > 5)
                // previously disabled useMotor for all here

                // set wheels to the speed
                setWheelSpeed(wheelTargetSpeed);
                // make the speed smaller
                wheelTargetSpeed /= (1 + 0.25F * Time.deltaTime); // wheel target speed goes down to 0
                motorSpeed + idleSpeed /= (1 + 0.25F * Time.deltaTime); // engine speed goes down to idle
                
                // if wheel target speed very small make it 0
                if ((wheelTargetSpeed < 0.01 && wheelTargetSpeed > 0) || (wheelTargetSpeed > -0.01 && wheelTargetSpeed < 0))
                {
                    wheelTargetSpeed = 0;
                }

                // decrease fuel qty by estimate of motor work
                if(wheelTargetSpeed < motorSpeed) { // if motor trying to make wheels faster
                    frameFuelUsage = Mathf.Abs(wheelTargetSpeed) * (motorForce / 10) * Time.deltaTime;
                } else {
                    frameFuelUsage = Mathf.Abs(motorSpeed / 10) * Time.deltaTime;
                }
                fuelQty -= frameFuelUsage;

                if(fuelQty < fuelLimit / 10F && !lowFuelNotified) { // debug low fuel warning, might upgrade to UI later
                    Debug.Log("fuel <10%: " + fuelQty);
                    lowFuelNotified = true;
                }
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
