using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

namespace LSL4Unity.Samples.SimplePhysicsEvent
{
    public class SimpleOutletTriggerEvent : MonoBehaviour
    {
        /*
         * This is a simple example of an LSL Outlet to stream out irregular events occurring in Unity.
         * This uses only LSL.cs and is intentionally simple. For a more robust version, see another sample.
         * 
         * We stream out the trigger event during OnTriggerEnter which is, in our opinion, the closest
         * time to when the trigger actually occurs (i.e., independent of its rendering).
         * A simple way to print the events is with pylsl: `python -m pylsl.examples.ReceiveStringMarkers`
         *
         * If you are instead trying to log a stimulus event then there are better options. Please see the 
         * LSL4Unity SimpleStimulusEvent Sample for such a design.
         */
        string StreamName = "cortivision_markers";// prendre le meme nom avec l4ordi serveur
        string StreamType = "Markers";
        private StreamOutlet outlet;
        private int[] sample={0};

        void Start()
        {
            var hash = new Hash128();
            hash.Append(StreamName);
            hash.Append(StreamType);
            hash.Append(gameObject.GetInstanceID());
            StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
                channel_format_t.cf_int8, hash.ToString());//hash est source id
            outlet = new StreamOutlet(streamInfo);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (outlet != null)
            {
                sample[0] =1;//"TriggerEnter " + gameObject.GetInstanceID();
                // Debug.Log(sample[0]);
                outlet.push_sample(sample);
                Debug.Log("TriggerEnter: " + gameObject.name + " collided with " + other.gameObject.name);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (outlet != null)
            {
                sample[0] = 2;//"TriggerExit " + gameObject.GetInstanceID();
                // Debug.Log(sample[0]);
                outlet.push_sample(sample);
                Debug.Log("TriggerExit: " + gameObject.name + " exited from " + other.gameObject.name);
            }
        }
    }
}