using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class TChassisScript : MonoBehaviour
{
    public Rigidbody2D SRRigidbody;
    public float jumpStrength;
    
    HingeJoint2D[] ArrayHJ;
    public GameObject[] ArrayWheels;

    public float wheelMotorMultiplier;
    public float motorTopSpeed;
    public float wheelTargetSpeed = 0F;
    public float idleSpeed;
    public float EngineSpeed;
    public float EngineAccel;
    float[] wheelDiameters;
    public float fuelQty;
    public float fuelLimit;
    public float motorForce;
    float tempSpeed;
    public float maxMotorForce;
    public float minMotorForce;
    public float brakingForce;
    public float frameFuelUsage = 0F;
    public float clutch = 0F;
    float reverseTimer = 0F;
    public float reverseDelay;
    public float fuelUsageMultiplier;
    bool lowFuelNotified;
    bool braking;
    bool pBrake;

    private Vector3[] HVertices;
    private Vector3[] bVertices;
    private Vector3[] allVertices;
    private int[] triangles = new int[0];// = new int[(int)(terrainLength * terrainVertexDensity * 6)];
    public int curveVertexCount;
    public float treadWidth;

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
        fuelLimit = 85F;
        lowFuelNotified = false;
        EngineSpeed = idleSpeed;

        motorForce = 50F; // check original force first probably, this will be changed live according to torque curve

        HVertices = new Vector3[(int)(ArrayWheels.Length - 2 + (curveVertexCount * 2))];
        bVertices = new Vector3[(int)(ArrayWheels.Length - 2 + (curveVertexCount * 2))];



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

        */
    }

    // Update is called once per frame
    void Update()
    {
        
        if (pBrake) {
            braking = true;
        } else {
            braking = false;
        }
        // set motorForce here with torquecurve?? lerp motor info to graph domain??
        if(clutch > 0) {
            // torque curve should be: sqrtx - x, 0 <= x <= 0.35
            tempSpeed = Mathf.Lerp(0F, 0.3F, (Mathf.Abs(EngineSpeed)/motorTopSpeed));
            //Debug.Log("curve out: " + (Mathf.Sqrt(tempSpeed) - tempSpeed));
            motorForce = minMotorForce + (((maxMotorForce-minMotorForce)*4) * (Mathf.Sqrt(tempSpeed) - tempSpeed)); // lowest force value + maxmotorforce/max from curve * curve
        } else {
            motorForce = minMotorForce;
        }

        if(Input.GetKey(KeyCode.End)) // vehicle go whee
        {
            if(Input.GetKey(KeyCode.PageDown)) {
                SRRigidbody.velocity += Vector2.left * 3;
            } else {
                SRRigidbody.velocity += Vector2.right * 3;
            }
        }
        if(Input.GetKey(KeyCode.X)) // cursed vehicle rotation
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 1 * Time.deltaTime, transform.rotation.z, transform.rotation.w);
        }

        if (Input.GetKeyDown(KeyCode.Space)) // do a jumpy
        {
            SRRigidbody.velocity += Vector2.up * jumpStrength;
        }
        if(Input.GetKeyDown(KeyCode.P)) // toggle parking brake
        {
            pBrake = !pBrake;
        }
        if(Input.GetKeyDown(KeyCode.V)) // toggle engine
        {
            if(EngineSpeed == 0 && fuelQty > 0) {
                EngineSpeed = idleSpeed;
            } else {
                EngineSpeed = 0;
            }
        }

        if(Input.GetKey(KeyCode.S)) // universal brake key
        {
            braking = true;
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
            //Debug.Log("a braking by " + 3F * Time.deltaTime);
        }
        // switch to reverse
        if (Input.GetKey(KeyCode.A) && EngineSpeed > (idleSpeed / (10F)) && Mathf.Abs(wheelTargetSpeed) < (idleSpeed / (10F)))
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
            //Debug.Log("d brake");
            braking = true;
        }
        // switch to forward
        if (Input.GetKey(KeyCode.D) && EngineSpeed < (idleSpeed / (-10F)) && Mathf.Abs(wheelTargetSpeed) < (idleSpeed / (10F)))
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
            // if wheel target speed very small make it 0, if engine off make it 0
            if ((wheelTargetSpeed < 0.01F && wheelTargetSpeed > 0) || (wheelTargetSpeed > -0.01F && wheelTargetSpeed < 0) || EngineSpeed == 0)
            {
                wheelTargetSpeed = 0;
            }

            if(fuelQty < fuelLimit / 10F && !lowFuelNotified) { // debug low fuel warning, might upgrade to UI later
                Debug.Log("fuel <10%: " + fuelQty);
                lowFuelNotified = true;
            }
        }
        if (braking) { // brake when braking
            clutch = 0F;
            motorForce = brakingForce;
            if (wheelTargetSpeed < (idleSpeed / (-10F))) {
                wheelTargetSpeed += 10F * Time.deltaTime;
            } else if (wheelTargetSpeed > (idleSpeed / (10F))) {
                wheelTargetSpeed -= 10F * Time.deltaTime;
            }
        }
        if (fuelQty <= 0) { // kill engine if no fuel
            EngineSpeed = 0;
        }
        if(clutch > 0) { // make clutch happen
            // change wheel target speed to match engine according to clutch engagement (LERP WOOOOOOO YEAAAA)
            wheelTargetSpeed = Mathf.Lerp(wheelTargetSpeed, EngineSpeed, clutch); // deltatime feels wrong here since it's in changing these components
        }
        setWheelSpeed(wheelTargetSpeed);

        // decrease fuel qty by estimate of motor work
        
        if(wheelTargetSpeed < EngineSpeed) { // if motor trying to make wheels faster // TODO: get actual wheelspeed
            frameFuelUsage = (Mathf.Abs(EngineSpeed) - /*actual wheelspeed?*/Mathf.Abs(wheelTargetSpeed)) * clutch + (/*idle usage rate*/0.01F * EngineSpeed) * 0.01F * fuelUsageMultiplier;
            //frameFuelUsage = Mathf.Abs(wheelTargetSpeed) * (motorForce / 10) * Time.deltaTime;
        } else {
            frameFuelUsage = (0.1F * clutch + (/*idle usage rate*/0.01F * Mathf.Abs(EngineSpeed)) * /*fuel usage multiplier*/0.01F) * fuelUsageMultiplier;
            //frameFuelUsage = Mathf.Abs(EngineSpeed / 10) * Time.deltaTime;
        }
        fuelQty -= frameFuelUsage * Time.deltaTime * 0.3F;

        updateTread();

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

    // tread mesh update
    void updateTread() {
        // reset values
        triangles = new int[0];
        HVertices = new Vector3[(int)(ArrayWheels.Length - 2 + (curveVertexCount * 2))];
        bVertices = new Vector3[(int)(ArrayWheels.Length - 2 + (curveVertexCount * 2))];
        
        // update inner vertices for roadwheels
        for(int i = 0; i < ArrayWheels.Length - 2; i++) {
            bVertices[i] = new Vector3(gameObject.transform.GetChild(1 + i).transform.GetChild(0).transform.localPosition.x, gameObject.transform.GetChild(1 + i).transform.GetChild(0).transform.localPosition.y - (wheelDiameters[i + 1] / 2), 0);
        }

        // update outer vertices for roadwheels
        for(int i = 0; i < ArrayWheels.Length - 2; i++) {
            HVertices[i] = new Vector3(bVertices[i].x, bVertices[i].y - treadWidth, 0);
        }

        // update vertices for rear wheel
        float wheelRelx;
        float wheelRely;
        for(int i = 0; i < curveVertexCount; i++) {
            // update inner vertex for rear wheel
            wheelRelx = wheelDiameters[ArrayWheels.Length - 1] / 2 * Mathf.Cos(Mathf.Lerp(3.141592653589F / 2, 4 * (3.141592653589F) / 3, (i/(curveVertexCount - 1))));
            //Debug.Log(wheelRelx); // all near 0
            wheelRely = wheelDiameters[ArrayWheels.Length - 1] / 2 * Mathf.Sin(Mathf.Lerp(3.141592653589F / 2, 4 * (3.141592653589F) / 3, (i/(curveVertexCount - 1))));
            bVertices[i + ArrayWheels.Length - 2] = new Vector3(
                (ArrayWheels[i + 1].transform.localPosition.x) + wheelRelx,
                (ArrayWheels[i + 1].transform.localPosition.y) + wheelRely,
                0);

            // update outer vertex for rear wheel
            // further out by treadWidth
            wheelRelx = (wheelDiameters[ArrayWheels.Length - 1] / 2 + treadWidth) * Mathf.Cos(Mathf.Lerp(3.141592653589F / 2, 4 * (3.141592653589F) / 3, (i/(curveVertexCount - 1))));
            wheelRely = (wheelDiameters[ArrayWheels.Length - 1] / 2 + treadWidth) * Mathf.Sin(Mathf.Lerp(3.141592653589F / 2, 4 * (3.141592653589F) / 3, (i/(curveVertexCount - 1))));
            HVertices[i + ArrayWheels.Length - 2] = new Vector3(
                (ArrayWheels[i + 1].transform.localPosition.x) + wheelRelx,// * relative multiplier, // instead of recalculating rels, use these??
                (ArrayWheels[i + 1].transform.localPosition.y) + wheelRely,// * relative multiplier,
                0);

        }

        // update vertices for front wheel
        for(int i = 0; i < curveVertexCount; i++) {
            // update inner vertex for front wheel
            wheelRelx = wheelDiameters[0] / 2 * Mathf.Cos(Mathf.Lerp(3.141592653589F / 2, 4 * (3.141592653589F) / 3, (i/(curveVertexCount - 1))));
            wheelRely = wheelDiameters[0] / 2 * Mathf.Sin(Mathf.Lerp(3.141592653589F / 2, 4 * (3.141592653589F) / 3, (i/(curveVertexCount - 1))));
            bVertices[i + ArrayWheels.Length - 2 + curveVertexCount] = new Vector3( // TODO: fix indexing
                (ArrayWheels[i + 1].transform.localPosition.x) + wheelRelx,
                (ArrayWheels[i + 1].transform.localPosition.y) + wheelRely,
                0);

            // update outer vertex for front wheel
            // further out by treadWidth
            wheelRelx = (wheelDiameters[0] / 2 + treadWidth) * Mathf.Cos(Mathf.Lerp(3.141592653589F / 2, -1 * (3.141592653589F) / 3, (i / curveVertexCount)));
            wheelRely = (wheelDiameters[0] / 2 + treadWidth) * Mathf.Sin(Mathf.Lerp(3.141592653589F / 2, -1 * (3.141592653589F) / 3, (i / curveVertexCount)));
            HVertices[i + ArrayWheels.Length - 2 + curveVertexCount] = new Vector3( // TODO: fix indexing
                (ArrayWheels[i + 1].transform.localPosition.x) + wheelRelx,// * relative multiplier, // instead of recalculating rels, use these??
                (ArrayWheels[i + 1].transform.localPosition.y) + wheelRely,// * relative multiplier,
                0);

        }


        // triangles assignment // TODO: align this functionality with the vertex organization
        //for each height if has next
        for (int i = 0; i < ArrayWheels.Length - 2 + (curveVertexCount * 2) - 2 ; i += 2){ // TODO: fix vertex order assignment
            // make near tri
            //add this near, this far, next near
            //UnityEditor.ArrayUtility.Add(ref triangles, );
            UnityEditor.ArrayUtility.Add(ref triangles, i);
            UnityEditor.ArrayUtility.Add(ref triangles, (i + bVertices.Length));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 2));
            
            // make far tri
            //add this far, next near, next far
            UnityEditor.ArrayUtility.Add(ref triangles, (i + bVertices.Length));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 2));
            UnityEditor.ArrayUtility.Add(ref triangles, (i + 2 + bVertices.Length));
            
        }
        
        // connect last vertices (on front wheel) to first vertices (on first roadwheel)
        UnityEditor.ArrayUtility.Add(ref triangles, (bVertices.Length * 2 - 1));
        UnityEditor.ArrayUtility.Add(ref triangles, (0));
        UnityEditor.ArrayUtility.Add(ref triangles, (1));
                

        // assign data to mesh 
        UnityEditor.ArrayUtility.AddRange(ref bVertices, HVertices);
        Mesh tMesh = new Mesh();
        tMesh.SetVertices(bVertices);
        tMesh.SetTriangles(triangles, 0, true, 0); 
        tMesh.RecalculateBounds();
        gameObject.transform.GetChild(ArrayWheels.Length).GetComponent<MeshFilter>().sharedMesh = tMesh;



        

    }
}
