using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; //このホイールがエンジンにアタッチされているかどうか
    public bool steering; // このホイールがハンドルの角度を反映しているかどうか
}

public class CarController : MonoBehaviour
{

    public List<AxleInfo> axleInfos; // 個々の車軸の情報
    public float maxMotorTorque; //ホイールに適用可能な最大トルク
    public float maxSteeringAngle; // 適用可能な最大ハンドル角度

    public Transform center;

    public List<WheelCollider> wheels;
    public List<Transform> wheelModel;
    float steering = 0.0f;
    float motor = 0.0f;
    float _brakeTorque;


    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = center.localPosition;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        WheelFrictionCurve forwardFriction = new WheelFrictionCurve();
        forwardFriction.extremumSlip = 0.4f;
        forwardFriction.extremumValue = 1;
        forwardFriction.asymptoteSlip = 0.5f;
        forwardFriction.asymptoteValue = 0.8f;
        forwardFriction.stiffness = 6f;

        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve();
        sidewaysFriction.extremumSlip = 0.2f;
        sidewaysFriction.extremumValue = 1f;
        sidewaysFriction.asymptoteSlip = 0.5f;
        sidewaysFriction.asymptoteValue = 0.8f;
        sidewaysFriction.stiffness = 2f;

        for (int i = 0; i < wheels.Count; i++) 
        {
            wheelModel[i].Rotate(wheels[i].rpm / 60 * 360 * Time.deltaTime, 0, 0);
            wheels[i].GetWorldPose(out var pos, out var rot);
            wheelModel[i].position = pos;
            wheelModel[i].rotation = rot;
            wheels[i].forwardFriction = forwardFriction;
            wheels[i].sidewaysFriction = sidewaysFriction;

            if (Input.GetKey(KeyCode.Space))
            {
                _brakeTorque = 1000f;
                forwardFriction.extremumSlip = 2.4f;
                forwardFriction.extremumValue = 1;
                forwardFriction.asymptoteSlip = 0.8f;
                forwardFriction.asymptoteValue = 0.8f;
                forwardFriction.stiffness = 2f;

                sidewaysFriction.extremumSlip = 2.2f;
                sidewaysFriction.extremumValue = 1f;
                sidewaysFriction.asymptoteSlip = 0.5f;
                sidewaysFriction.asymptoteValue = 0.8f;
                sidewaysFriction.stiffness = 2f;

                wheels[i].forwardFriction = forwardFriction;
                wheels[i].sidewaysFriction = sidewaysFriction;
                Debug.Log("sss");

            }
            else
            {
                _brakeTorque = 0f;
            }
        }
        wheels[0].brakeTorque = _brakeTorque;
        wheels[1].brakeTorque = _brakeTorque;
       
    }

    public void FixedUpdate()
    {
       motor = maxMotorTorque * Input.GetAxis("Vertical");
       steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            
            if (axleInfo.steering)
            {               
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            { 
                axleInfo.leftWheel.motorTorque = motor*2;
                axleInfo.rightWheel.motorTorque = motor*2;
            }
        }
    }
}