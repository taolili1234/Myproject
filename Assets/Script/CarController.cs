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
        for (int i = 0; i < wheels.Count; i++) 
        {
            wheelModel[i].Rotate(wheels[i].rpm / 60 * 360 * Time.deltaTime, 0, 0);
            wheels[i].GetWorldPose(out var pos, out var rot);
            wheelModel[i].position = pos;
            wheelModel[i].rotation = rot;
        }
        wheels[0].brakeTorque = _brakeTorque;
        wheels[1].brakeTorque = _brakeTorque;
        if (Input.GetKey(KeyCode.Space))
        {        
            _brakeTorque = 1000f;
            Debug.Log("sss");
        }
        else
        {
            _brakeTorque = 0f;
        }


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
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }
}