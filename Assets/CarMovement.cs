using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using System;
using System.Threading;
using System.Collections;
using EasyRoads3Dv3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    public ERRoadNetwork roadNetwork;
    public ERRoad road;
    public float defaultSpeedKPH; // 每小时的速度
    private float speedMPS; // 每秒的速度
    // private int currentElement = 500;// 1 elemnt = 2 metres
    private int currentElement = 80;
    private float distance = 0;
    public Transform ped;
    public float instantSpeed;
    // private float accelFactor;
    public float acceleration; // 初始加速度
    public float turnTime;
    private float turnStartTime;
    private bool isChangingSpeed = false;
    private float distanceToMove;
    private float relativeSpeed;
    public bool isConstantSpeed;
    public float initialSpeed;
    private bool isPositiveAcceleration;
    private float accelerationMagnitude;
    private GameManager gameManager;
    public float TTC;
    public Text TTCText;
    // private bool isBlackScreenActive = false;
    // private bool playerChoice; // true为button11（返回1), false为button10 (返回0)
    private ScreenFader screenFader;
    private JoystickButtonTester joystickButtonTester;
    // private Joystick joystick;
    public Vector3 screenPosition;
    private bool hasReachedDestination = false;
    private MarkerEvent markerEvent;
    // private float totalDistance = 0;

    
    public void Start()
    {
        markerEvent=FindObjectOfType<MarkerEvent>();
        // joystick = Input.GetJoystickButton;
        joystickButtonTester = FindObjectOfType<JoystickButtonTester>();
        screenFader = FindObjectOfType<ScreenFader>();
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager.currentCondition=="SameSpeed30"){
            isConstantSpeed=true;
            initialSpeed=30;
            isPositiveAcceleration=true;
            accelerationMagnitude=0;
            SetCarSpeed(true,30,true,0);
        }else if (gameManager.currentCondition=="SameSpeed50"){
            isConstantSpeed=true;
            initialSpeed=50;
            isPositiveAcceleration=true;
            accelerationMagnitude=0;
            SetCarSpeed(true,50,true,0);
        }else if (gameManager.currentCondition=="SpeedUpFrom30a04"){
            isConstantSpeed=false;
            initialSpeed=30;
            isPositiveAcceleration=true;
            accelerationMagnitude=0.04f;
            SetCarSpeed(false,30,true,0.04f);
        }else if (gameManager.currentCondition=="SpeedUpFrom30a08"){
            isConstantSpeed=false;
            initialSpeed=30;
            isPositiveAcceleration=true;
            accelerationMagnitude=0.08f;
            SetCarSpeed(false,30,true,0.08f);
        }else if (gameManager.currentCondition=="SpeedUpFrom50a04"){
            isConstantSpeed=false;
            initialSpeed=50;
            isPositiveAcceleration=true;
            accelerationMagnitude=0.04f;
            SetCarSpeed(false,50,true,0.04f);
        }else if (gameManager.currentCondition=="SpeedUpFrom50a08"){
            isConstantSpeed=false;
            initialSpeed=50;
            isPositiveAcceleration=true;
            accelerationMagnitude=0.08f;
            SetCarSpeed(false,50,true,0.08f);
        }else if (gameManager.currentCondition=="SlowDownFrom30a04"){
            isConstantSpeed=false;
            initialSpeed=30;
            isPositiveAcceleration=false;
            accelerationMagnitude=0.04f;
            SetCarSpeed(false,30,false,0.04f);
        }else if (gameManager.currentCondition=="SlowDownFrom30a08"){
            isConstantSpeed=false;
            initialSpeed=30;
            isPositiveAcceleration=false;
            accelerationMagnitude=0.08f;
            SetCarSpeed(false,30,false,0.08f);
        }else if (gameManager.currentCondition=="SlowDownFrom50a04"){
            isConstantSpeed=false;
            initialSpeed=50;
            isPositiveAcceleration=false;
            accelerationMagnitude=0.04f;
            SetCarSpeed(false,50,false,0.04f);
        }else{
            isConstantSpeed=false;
            initialSpeed=50;
            isPositiveAcceleration=false;
            accelerationMagnitude=0.08f;
            SetCarSpeed(false,50,false,0.08f);
        }
        speedMPS = defaultSpeedKPH / 3.6f;
        roadNetwork = new ERRoadNetwork();
        road = roadNetwork.GetRoadByName("road_0002");

        if (road == null)
        {
            UnityEngine.Debug.LogError("Road object not found.");
        }
    }

    void Update()
    {
        // totalDistance += distance;
        screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // 限制视口坐标的范围在 [0, 1] 之间
        screenPosition.x = Mathf.Clamp01(screenPosition.x);
        screenPosition.y = Mathf.Clamp01(screenPosition.y);
        screenPosition.z=0;

        if(!hasReachedDestination){
            Vector3[] centerPoints = road.GetSplinePointsCenter();

            if (centerPoints.Length > 0)
            {
                distance += distanceToMove;

                while (distance > Vector3.Distance(centerPoints[currentElement], centerPoints[currentElement + 1]))
                {
                    distance -= Vector3.Distance(centerPoints[currentElement], centerPoints[currentElement + 1]);
                    currentElement--;
                    if (currentElement >= centerPoints.Length - 1)
                    {
                        currentElement = 0;
                    }
                }
                // UnityEngine.Debug.Log("element interval dist : "+Vector3.Distance(centerPoints[currentElement], centerPoints[currentElement + 1]));
                // 计算车辆位置
                Vector3 position = Vector3.Lerp(centerPoints[currentElement + 1], centerPoints[currentElement], distance / Vector3.Distance(centerPoints[currentElement], centerPoints[currentElement + 1]));

                // 获取道路上的法线（垂直于道路的方向）
                Vector3 normal = Vector3.up; // 初始化为向上的法线
                float tilting = road.GetMarkerTilting(currentElement);

                // 根据道路倾斜值调整法线
                normal = Quaternion.AngleAxis(tilting, centerPoints[currentElement + 1] - centerPoints[currentElement]) * normal;
                // 根据法线调整车辆位置
                position += normal * 1.1f; // 调整值根据需要调整
                // float offset=1.1f;
                // position = Vector3.ProjectOnPlane(position, normal*offset);
                // 直接设置车辆的位置
                transform.position = position;

                // 计算车辆朝向
                Vector3 direction = centerPoints[currentElement + 1] - centerPoints[currentElement];
                
                Quaternion rotation = Quaternion.LookRotation(-direction.normalized, normal);
                // 直接设置车辆的旋转
                transform.rotation = rotation;

                float distanceToPed = Vector3.Distance(transform.position, ped.position);
                // 计算TTC
                TTC = distanceToPed / relativeSpeed;

                // UnityEngine.Debug.Log($"TTC: {TTC}");
                instantSpeed = relativeSpeed * 3.6f;
            }
            MoveCar();
        }
        TTCText.text = "TTC: "+ TTC.ToString("F2");
    }

    void MoveCar()
    {
        if (isChangingSpeed)
        {
            turnTime = Time.time - turnStartTime;
            // Interpolate acceleration
            acceleration = isPositiveAcceleration ? accelerationMagnitude : -accelerationMagnitude;

            // Use Time.deltaTime to calculate the distance moved in the current frame
            relativeSpeed = speedMPS + acceleration * turnTime;
            distanceToMove = (relativeSpeed *Time.deltaTime) + (0.5f * acceleration *Time.deltaTime* Time.deltaTime);//* turnTime * turnTime  

            Debug.Log($"Distance To Move: {distanceToMove}, Instant Speed: {instantSpeed}, turnTime: {turnTime}");
        }
        else
        {
            // Use Time.deltaTime to calculate the distance moved in the current frame
            distanceToMove = speedMPS * Time.deltaTime;
            relativeSpeed = speedMPS;
            acceleration = 0f;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedChange"))
        {
            if(!isConstantSpeed)
            {
                isChangingSpeed = true;
                turnStartTime=Time.time;
                StartCoroutine(SmoothAccelerationChange());
                markerEvent.RecordSpeedChange();
                UnityEngine.Debug.Log("Speed Change ! ");
            }
        }
        if(other.CompareTag("destination"))
        {
            UnityEngine.Debug.Log("<color=green>Morph !</color>");
            gameManager.RestartTry();
            hasReachedDestination = true;
        }
    }
    IEnumerator SmoothAccelerationChange()
    {
        float elapsedTime = 0f;
        float duration = 3f; // Adjust the duration based on your preference

        float initialAcceleration = acceleration;
        float targetAcceleration = isPositiveAcceleration ? accelerationMagnitude : -accelerationMagnitude;

        while (elapsedTime < duration)
        {
            acceleration = Mathf.Lerp(initialAcceleration, targetAcceleration, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        acceleration = targetAcceleration; // Directly set the target acceleration
    }

    public void ResetCar()
    {
        isChangingSpeed = false;
        turnTime = 0;
        turnStartTime = 0;
        // 重置车辆位置和参数
        distance = 0;
        currentElement = 80;

        UnityEngine.Debug.Log("Reset car");
        screenFader.ChoiceMade = 2;
        screenFader.reactionTime=0;
        hasReachedDestination = false;
        Start();
    }
    public void SetCarSpeed(bool isConstantSpeed, float initialSpeed, bool IsPositiveAcceleration, float AccelerationMagnitude)
    {
        // 设置车辆速度参数
        // this.isConstantSpeed = IsConstantSpeed;
        this.isPositiveAcceleration=IsPositiveAcceleration;
        defaultSpeedKPH = initialSpeed;
        // 重新计算初始速度和加速度
        speedMPS = defaultSpeedKPH / 3.6f;
        accelerationMagnitude=AccelerationMagnitude;

    }
}