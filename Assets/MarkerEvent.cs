using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class MarkerEvent : MonoBehaviour
{
    /*
        * We stream out the trigger event during OnTriggerEnter which is, in our opinion, the closest
        * time to when the trigger actually occurs (i.e., independent of its rendering).
        * A simple way to print the events is with pylsl: `python -m pylsl.examples.ReceiveStringMarkers`
        */
    string StreamName = "cortivision_markers";// prendre le meme nom avec l'ordi serveur
    //string StreamName = "cortivision_markers_sim";
    string StreamType = "Markers";
    private StreamOutlet outlet;
    // private GameManager gameManager;
    // private PlayerController playerController;
    private int[] sample={0};

    void Start()
    {
        // gameManager=FindObjectOfType<GameManager>();
        // playerController=FindObjectOfType<PlayerController>();
        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
        // StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 2, LSL.LSL.IRREGULAR_RATE,

            channel_format_t.cf_int8, hash.ToString());//hash est source id
        outlet = new StreamOutlet(streamInfo);
    }


    public void RecordRestartTry()
    {
        if (outlet != null)
        {
            sample[0] = 1;//"RestartTry";
            outlet.push_sample(sample);
            Debug.Log("RestartTry marker recorded for " + gameObject.name);
        }
    }

    public void RecordNextCondition()
    {
        if (outlet != null)
        {
            sample[0] = 2;//"NextCondition";
            outlet.push_sample(sample);
            Debug.Log("NextCondition marker recorded for " + gameObject.name);
        }
    }

    public void RecordBlackOutStart()
    {
        if (outlet != null)
        {
            sample[0] = 3;//"BlackOutStart";
            outlet.push_sample(sample);
            Debug.Log("BlackOutStart marker recorded for " + gameObject.name);
        }
    }
        public void RecordSpeedChange()
    {
        if (outlet != null)
        {
            sample[0] = 4;//"SpeedChangeMoment";
            outlet.push_sample(sample);
            Debug.Log("SpeedChange marker recorded for " + gameObject.name);
        }
    }

}