using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; //���̃z�C�[�����G���W���ɃA�^�b�`����Ă��邩�ǂ���
    public bool steering; // ���̃z�C�[�����n���h���̊p�x�𔽉f���Ă��邩�ǂ���
}

public class CarController : MonoBehaviour
{

    public List<AxleInfo> axleInfos; // �X�̎Ԏ��̏��
    public float maxMotorTorque; //�z�C�[���ɓK�p�\�ȍő�g���N
    public float maxSteeringAngle; // �K�p�\�ȍő�n���h���p�x

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