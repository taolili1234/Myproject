using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController: MonoBehaviour
{
    public List<WheelCollider> wheels;
    public List<Transform> wheelModel;
    public Transform center;
    //public float LeftShiftSpeed;
    public float c = 0;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
    }
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = center.localPosition;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }


     void FixedUpdate()
    {
        float s = 2500;
        float a = 0;
        float b = 0;
        if (!Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (Input.GetKey(KeyCode.S))
            {
                this.GetComponent<Rigidbody>().drag = 0f;
                for (int i = 0; i <= 1000; i +=10)
                {
                    s = -i;
                }
            }
            else if(Input.GetKeyUp(KeyCode.S))
            {
                this.GetComponent<Rigidbody>().drag = 1f;
            }


            if (Input.GetKey(KeyCode.A))
            {
                a = -45;
            }
            if (Input.GetKey(KeyCode.D))
            {
                a = 45;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                
                b = 1000;
            }

            if (Input.GetKey(KeyCode.W))
            {
                for (int  i = 0; i<= s; i += 2)
                {
                    s = i;
                }
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift)&& Input.GetKey(KeyCode.W))
            {
                for (int i = 0; i <= 2500; i += 2)
                {
                    for(int l=1;l<=10;l++)
                    {
                        s = i * l;
                    }
                }
            }
        
        }
        else
        {
            this.GetComponent<Rigidbody>().drag = 1f;
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
        wheels[0].steerAngle = Mathf.Lerp(wheels[0].steerAngle,a,c);
        wheels[1].steerAngle = Mathf.Lerp(wheels[1].steerAngle, a,c);
    }
    
}
