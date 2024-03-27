using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LogCSV : MonoBehaviour
{
    private GameManager gameManager;
    private StreamWriter writer;
    private ScreenFader screenFader;
    private CarMovement carMovement;
    // private CollisionTimeEstimator collisionTimeEstimator;
    // private string distCars2Ped;

    void Awake()
    {
        carMovement=FindObjectOfType<CarMovement>();
        gameManager = FindObjectOfType<GameManager>();
        screenFader=FindObjectOfType<ScreenFader>();
        // collisionTimeEstimator = FindObjectOfType<CollisionTimeEstimator>();
    }

    void Start()
    {
        GenerateCSV();
    }

    void GenerateCSV()
    {
        //  get the date and time as the name of the file
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";

        // get path to Downloads folder
        string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";

        // create the complete path
        string filePath = Path.Combine(downloadsPath, fileName);

        // creat a StreamWriter
        writer = new StreamWriter(filePath);

        // write the headlines of the CSV file 
        writer.WriteLine("Time;TimeWithinEachTrial;TypeCond;NbTrial;JoystickYAxis;BinaryChoice;RT;ScreenCoordinate;TurnTime;InstantSpeed;AccelFactor");
//TTCEstimatedOnStraightLine;
        Debug.Log("CSV file generated: " + filePath);
    }

    void Update()
    {
        // distCars2Ped=collisionTimeEstimator.zCoordinatesList;
        //the elaspedTime gets reset after each transportation
        writer.WriteLine(gameManager.generalTime+";"+
        gameManager.elapsedTime + ";"+
        gameManager.currentCondition+";"+
        gameManager.nbTry+";"+
        // carMovement.TTC +";" +
        gameManager.normalizedYAxis//.ToString()
        +";"+screenFader.ChoiceMade
        +";"+screenFader.reactionTime
        +";"+carMovement.screenPosition
        +";"+carMovement.turnTime
        +";"+carMovement.instantSpeed
        +";"+carMovement.acceleration
        );
    }

    void OnDestroy()
    {
        // closing StreamWriter
        if (writer != null)
        {
            writer.Close();
        }
    }
}
