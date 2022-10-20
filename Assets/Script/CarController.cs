using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController: MonoBehaviour
{
    public List<WheelCollider> wheels;
    public List<Transform> wheelModel;
    public Transform center;
    public float LeftShiftSpeed;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = center.localPosition;
    }

   
    private void Update()
    {
        float s = 0;
        float a = 0;
        float b = 0;
       
        if (Input.GetKey(KeyCode.S))
        {
            s = -1000;
        }
        if (Input.GetKey(KeyCode.A))
        {
            a = -25;
        }
        if (Input.GetKey(KeyCode.D))
        {
            a = 25;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            b=1000;
        }

        if (Input.GetKey(KeyCode.LeftShift)&& Input.GetKey(KeyCode.W))
        {
             s= 2000* LeftShiftSpeed;
            Debug.Log("sss");
        }
        else if (Input.GetKey(KeyCode.W))
        {

            s = 2000;
        }

        for (int i=0;i<wheels.Count;i++)
        {
            wheels[i].motorTorque = s;
            wheels[i].brakeTorque = b;
            // Vector3 pos;
            // Quaternion rot;
            wheels[i].GetWorldPose(out var pos,out var rot);
            wheelModel[i].position = pos;
            wheelModel[i].rotation = rot;
        }
        wheels[0].steerAngle = a;
        wheels[1].steerAngle = a;
        
    }
}
