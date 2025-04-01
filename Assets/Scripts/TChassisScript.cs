using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class TChassisScript : MonoBehaviour
{
    public Rigidbody2D SRRigidbody;
    public float jumpStrength = 10;
    
    HingeJoint2D[] ArrayHJ;
    public GameObject[] ArrayWheels;

    public float wheelMotorMultiplier = 50F;
    public float motorTopSpeed = 35F;
    public float wheelTargetSpeed = 0F;
    public float idleSpeed = 10F;
    public float EngineSpeed;
    public float EngineAccel = 8F;
    float[] wheelDiameters;
    public float fuelQty;
    public float fuelLimit;
    public float motorForce;
    float tempSpeed;
    public float maxMotorForce = 80F;
    public float minMotorForce = 1F;
    public float brakingForce = 100F;
    public float frameFuelUsage = 0F;
    public float clutch = 0F;
    float reverseTimer = 0F;
    public float reverseDelay = 1F;
    bool lowFuelNotified;
    bool braking;

    // Start is called before the first frame update
    void Start()
    {
        // do a little happy hop yay
        SRRigidbody.velocity = Vector2.up * 10;

        // get wheel sizes and store in array, to be used for wheelspeed sync
        wheelDiameters = new float[ArrayWheels.Length];
        for (int i = 0; i < ArrayWheels.Length; i++)
        {
            //Debug.Log(ArrayWheels[i].GetComponent<CircleCollider2D>());
            //Debug.Log(ArrayWheels[i].GetComponent<CircleCollider2D>().radius);
            
            wheelDiameters[i] = ArrayWheels[i].GetComponent<CircleCollider2D>().radius;
            //Debug.Log("did wheel thing");\
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
        lowFuelNotified = false;
        EngineSpeed = idleSpeed;

        motorForce = 50F; // check original force first probably, this will be changed live according to torque curve

        /*
        plan:
        change motor speed slightly with inputs, otherwise goes to idle value
        motor speed changed slightly by difference to wheelspeed? (allow wheelspeed to affect EngineSpeed back)

        change motor speed direction when at idle value and hold other direction input
        set motor force to low when no inputs
        set motor force to a multiplier of the torque curve value(dependent of EngineSpeed) that ramps up to 1x when either input
        
        // enginespeed changed with inputs, engine torque determined by torque curve and disabled when no inputs
        
        wheel target speed set to 0 unless inputs, then uses ramping multiplier up to EngineSpeed

        scripting TODO:
        
        other bugfixing:
        EngineSpeed goes up thousands when should decrease
        fuel usage inversely proportional and problematic otherwise

        */
    }

    // Update is called once per frame
    void Update()
    {
        braking = false;
        // set motorForce here with torquecurve?? lerp motor info to graph domain??
        if(clutch > 0) {
            // torque curve should be: sqrtx - x, 0 <= x <= 0.35
            tempSpeed = Mathf.Lerp(0F, 0.3F, (Mathf.Abs(EngineSpeed)/motorTopSpeed));
            //Debug.Log("curve out: " + (Mathf.Sqrt(tempSpeed) - tempSpeed));
            motorForce = minMotorForce + (((maxMotorForce-minMotorForce)*4) * (Mathf.Sqrt(tempSpeed) - tempSpeed)); // lowest force value + maxmotorforce/max from curve * curve
        } else {
            motorForce = minMotorForce;
        }


        if(Input.GetKey(KeyCode.X)) // cursed vehicle rotation
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 1 * Time.deltaTime, transform.rotation.z, transform.rotation.w);
        }

        if (Input.GetKeyDown(KeyCode.Space)) // do a jumpy
        {
            SRRigidbody.velocity += Vector2.up * jumpStrength;
        }

        // if input A, make EngineSpeed go more left, engage clutch
        
        // if reverse and not at top speed go faster
        if (Input.GetKey(KeyCode.A) && EngineSpeed < (idleSpeed / (-10F)) && fuelQty > 0)
        {
            increaseClutch();
            if(Mathf.Abs(EngineSpeed) < motorTopSpeed)
            {
                EngineSpeed -= EngineAccel * Time.deltaTime;
            }
            // wheelTargetSpeed -= 1F * Time.deltaTime;//(0.1F / (wheelTargetSpeed + 0.05F)) * Time.deltaTime;
            //Debug.Log("a accel");

        // braking when go forward
        } else if (Input.GetKey(KeyCode.A) && EngineSpeed > (idleSpeed / (10F)) && wheelTargetSpeed > 0)
        { 
            braking = true;
            wheelTargetSpeed -= 10F * Time.deltaTime;
            //Debug.Log("a braking by " + 3F * Time.deltaTime);
        
        
        // switch to reverse
        } else if (Input.GetKey(KeyCode.A) && wheelTargetSpeed < (idleSpeed / (10F)))
        {
            // increment timer
            reverseTimer += Time.deltaTime;
            if (reverseTimer > reverseDelay) {
                //reverse
                reverseTimer = 0;
                EngineSpeed = -1F * (idleSpeed);
            }
        }
        // if input D, make EngineSpeed go more right, engage clutch

        // if forward and not at top speed go faster
        if (Input.GetKey(KeyCode.D) && EngineSpeed > (idleSpeed / (10F)) && fuelQty > 0) 
        {
            increaseClutch();
            if(Mathf.Abs(EngineSpeed) < motorTopSpeed)
            {
                EngineSpeed += EngineAccel * Time.deltaTime;
            }
            // wheelTargetSpeed += 1F * Time.deltaTime;//(0.1F / (wheelTargetSpeed + 0.05F)) * Time.deltaTime;
            //Debug.Log("d accel");
            

        // braking when go backward
        } else if (Input.GetKey(KeyCode.D) && EngineSpeed < (idleSpeed / (-10F)) && wheelTargetSpeed < 0)
        {
            wheelTargetSpeed += 10F * Time.deltaTime;
            //Debug.Log("d brake");
            braking = true;
        
        // switch to forward
        } else if (Input.GetKey(KeyCode.D) && wheelTargetSpeed < (idleSpeed / (10F)))
        {
            // increment timer
            reverseTimer += Time.deltaTime;
            if (reverseTimer > reverseDelay) {
                //reverse
                reverseTimer = 0;
                EngineSpeed = 1F * (idleSpeed);
            }
        }

        // if NOT A and reverse or NOT D and forward
        if((!Input.GetKey(KeyCode.A) && EngineSpeed < (idleSpeed / (-10F))) || (!Input.GetKey(KeyCode.D) && EngineSpeed > (idleSpeed / (10F)))) // deceleration (to idle motor speed)
        {
            // reset clutch
            clutch = 0F;

            // make the speed smaller
            wheelTargetSpeed /= (1F + 0.4F * Time.deltaTime); // wheel target speed goes down to 0
            
            // engine speed goes down to idle
            if (EngineSpeed > 0) {
                EngineSpeed = ((EngineSpeed - idleSpeed) / (1F + 1F * Time.deltaTime)) + idleSpeed;
            } else if (EngineSpeed < 0) {
                EngineSpeed = ((EngineSpeed + idleSpeed) / (1F + 1F * Time.deltaTime)) - idleSpeed;
            }
            // if wheel target speed very small make it 0
            if ((wheelTargetSpeed < 0.01F && wheelTargetSpeed > 0) || (wheelTargetSpeed > -0.01F && wheelTargetSpeed < 0))
            {
                wheelTargetSpeed = 0;
            }

            // decrease fuel qty by estimate of motor work
            frameFuelUsage = (EngineSpeed - /*actual wheelspeed?*/wheelTargetSpeed) * clutch + (/*idle usage rate*/0.1F * engineSpeed) * /*fuel usage multiplier*/0.1F;
            /*if(wheelTargetSpeed < EngineSpeed) { // if motor trying to make wheels faster
                frameFuelUsage = Mathf.Abs(wheelTargetSpeed) * (motorForce / 10) * Time.deltaTime;
            } else {
                frameFuelUsage = Mathf.Abs(EngineSpeed / 10) * Time.deltaTime;
            }/*
            fuelQty -= frameFuelUsage;

            if(fuelQty < fuelLimit / 10F && !lowFuelNotified) { // debug low fuel warning, might upgrade to UI later
                Debug.Log("fuel <10%: " + fuelQty);
                lowFuelNotified = true;
            }
        }
        if (braking) {
            clutch = 0F;
            motorForce = brakingForce;
        }
        if(clutch > 0) {
            // change wheel target speed to match engine according to clutch engagement (LERP WOOOOOOO YEAAAA)
            wheelTargetSpeed = Mathf.Lerp(wheelTargetSpeed, EngineSpeed, clutch); // deltatime feels wrong here since it's in changing these components
        }
        setWheelSpeed(wheelTargetSpeed);
    }

    // set each wheel to the speed, this does not manage engine inertia
    void setWheelSpeed(float speedValue)
    {
        // get a motor to modify, then give it back to all HingeJoint2D in array
        
        for(int i = 0; i < ArrayHJ.Length; i++)
        {
            var motor1 = ArrayHJ[i].motor;
            motor1.motorSpeed = (float)(wheelMotorMultiplier * speedValue * 0.05F / (3.1415926535F*wheelDiameters[i]));
            motor1.maxMotorTorque = motorForce;
            ArrayHJ[i].motor = motor1;
        }
    }
    void increaseClutch() {
        // increase clutch engagement
        if (clutch < 1F) {
            clutch += 3F * Time.deltaTime;
        } else {
            clutch = 1F;
        }
    }
}
