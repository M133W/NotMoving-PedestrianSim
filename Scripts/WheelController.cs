using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider FR;
    [SerializeField] WheelCollider FL;
    [SerializeField] WheelCollider BR;
    [SerializeField] WheelCollider BL;

    [SerializeField] Transform FRTransform;
    [SerializeField] Transform FLTransform;
    [SerializeField] Transform BRTransform;
    [SerializeField] Transform BLTransform;

    public float acceleration = 500f;
    public float breakingForce = 1500f;
    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
            currentBreakingForce = breakingForce;
        else
            currentBreakingForce = 0f;

        FR.motorTorque = currentAcceleration;
        FL.motorTorque = currentAcceleration;

        FR.brakeTorque = currentBreakingForce;
        FL.brakeTorque = currentBreakingForce;
        BR.brakeTorque = currentBreakingForce;
        BL.brakeTorque = currentBreakingForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        FL.steerAngle = currentTurnAngle;
        FR.steerAngle = currentTurnAngle;

        UpdateWheel(FL, FLTransform);
        UpdateWheel(FR, FRTransform);
        UpdateWheel(BL, BLTransform);
        UpdateWheel(BR, BRTransform);

    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        UnityEngine.Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
