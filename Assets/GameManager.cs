using System.Globalization;
using System.ComponentModel;
// using System.Reflection.PortableExecutable;
using System.Diagnostics;
using System.Timers;
using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private ScreenFader screenFader;
    public float tryStartTime;
    public float elapsedTime;
    public int nbTry = 1;  
    public int maxNbTry = 5;
    private float gameStartTime;
    public float generalTime;
    private GameCondition selectedCondition;
    public List<GameCondition> availableConditions;
    public string currentCondition;
    private processControl processControl;
    private CarMovement carMovement;
    private MarkerEvent markerEvent;
    public Text ConditionText;
    public float normalizedYAxis;
    public bool ConditionTransition=true;

    public void Awake()
    {
        carMovement=FindObjectOfType<CarMovement>();
        tryStartTime = Time.realtimeSinceStartup;//Time.time;
        screenFader = FindObjectOfType<ScreenFader>();
        processControl = FindObjectOfType<processControl>();
        markerEvent=FindObjectOfType<MarkerEvent>();
        gameStartTime = Time.realtimeSinceStartup;//Time.time;
        availableConditions = Enum.GetValues(typeof(GameCondition)).OfType<GameCondition>().ToList();
        SelectRandomCondition();
        ExecuteConditionLogic();
        // }
        ConditionText.text = "Condition : "+currentCondition;
    }

    private void ExecuteConditionLogic()
    {
        // trafficController.enabled=false;
        switch (selectedCondition)
        {
            case GameCondition.SameSpeed30:
                UnityEngine.Debug.Log("Condition : SameSpeed30");
                // carMovement.SetCarSpeed(true,30,true,0);
                break;
            case GameCondition.SameSpeed50:
                UnityEngine.Debug.Log("Condition : SameSpeed50");
                // carMovement.SetCarSpeed(true,50,true,0);
                break;
            case GameCondition.SpeedUpFrom30a04:
                UnityEngine.Debug.Log("Condition : SpeedUpFrom30a=2");
                // carMovement.SetCarSpeed(false,30,true,2);
                break;
            case GameCondition.SpeedUpFrom30a08:
                UnityEngine.Debug.Log("Condition : SpeedUpFrom30a=4");
                // carMovement.SetCarSpeed(false,30,true,4);
                break;
            case GameCondition.SpeedUpFrom50a04:
                UnityEngine.Debug.Log("Condition : SpeedUpFrom50a=2");
                // carMovement.SetCarSpeed(false,50,true,2);
                break;
            case GameCondition.SpeedUpFrom50a08:
                UnityEngine.Debug.Log("Condition : SpeedUpFrom50a=4");
                // carMovement.SetCarSpeed(false,50,true,4);
                break;
            case GameCondition.SlowDownFrom30a04:
                UnityEngine.Debug.Log("Condition : SlowDownFrom30a=2");
                // carMovement.SetCarSpeed(false,30,false,2);
                break;
            case GameCondition.SlowDownFrom30a08:
                UnityEngine.Debug.Log("Condition : SlowDownFrom30a=4");
                // carMovement.SetCarSpeed(false,30,false,4);
                break;
            case GameCondition.SlowDownFrom50a04:
                UnityEngine.Debug.Log("Condition : SlowDownFrom50a=2");
                // carMovement.SetCarSpeed(false,50,false,2);
                break;
            case GameCondition.SlowDownFrom50a08:
                UnityEngine.Debug.Log("Condition : SlowDownFrom50a=4");
                // carMovement.SetCarSpeed(false,50,false,4);
                break;
            default:
                UnityEngine.Debug.LogError("Invalid game condition selected");
                break;
        }
        currentCondition = selectedCondition.ToString(); 
    }
    public bool RestartTry()
    {
        if (nbTry < maxNbTry)
        {
            screenFader.TriggerScreenFader();
            // nbTry++;
            ConditionTransition=false;
            return true;
        }else{
            // carMovement.ResetCar();
            // screenFader.TriggerScreenFader();
            NextCondition();
        }
        return false;
    }

    public void Update()
    {
        elapsedTime = Time.realtimeSinceStartup - tryStartTime;//Time.time-;
        generalTime=Time.realtimeSinceStartup - gameStartTime;//Time.time-;
        if (Input.GetKeyDown(KeyCode.N))
        {
           NextCondition();
        }
        // 获取手柄Y轴输入
        float yAxisInput = Input.GetAxis("Vertical");

        // 将输入值归一化到[0, 1]范围内
        normalizedYAxis = (yAxisInput + 1f) / 2f;
        // UnityEngine.Debug.Log("normalizedYAxis : " + normalizedYAxis);
        if (Input.GetKeyDown(KeyCode.R))//in case the car went through checkpoint
        {
            RestartTry();
            // nbTry-=1;
        }
    }
    public enum GameCondition
    {
        SameSpeed30,
        SameSpeed50,
        SpeedUpFrom30a04,
        SpeedUpFrom30a08,
        SpeedUpFrom50a04,
        SpeedUpFrom50a08,
        SlowDownFrom30a04,
        SlowDownFrom30a08,
        SlowDownFrom50a04,
        SlowDownFrom50a08,
    }

    private void SelectRandomCondition()
    {
        int randomIndex = UnityEngine.Random.Range(0, availableConditions.Count);
        selectedCondition = availableConditions[randomIndex];
        availableConditions.RemoveAt(randomIndex);

    }
    public void NextCondition()
    {
        markerEvent.RecordNextCondition();
        if (availableConditions.Count > 0 )//&&  trafficController.IsLastCondition()==false )//
        {
            // Select the next condition
            SelectRandomCondition();
            // playerController.ResetPed();
            // Execute logic for the new condition
            ExecuteConditionLogic();
            screenFader.TriggerScreenFader();
            // tryStartTime = Time.time;
            // carMovement.ResetCar();
            ConditionText.text = "Condition : "+currentCondition;
            ConditionTransition=true;
        }
        else
        {
            screenFader.TriggerScreenFader();
            // if (screenFader.reactionTime!=0){
            //     processControl.SessionOver();
            // }
            UnityEngine.Debug.LogWarning("No available conditions left.");
        }
    }
}
