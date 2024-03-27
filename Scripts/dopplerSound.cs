using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dopplerSound : MonoBehaviour
{
    private AudioSource audioSource;
    
    float minDistance = 0.05f; // 音效最小距离，即开始变化的距离
    float maxDistance = 15f; // 音效最大距离，即音效不再变化的距离
    float minPitch = -20f; // 最小音调
    float maxPitch = 8f; // 最大音调
    private Vector3 pedPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // playerController = FindObjectOfType<PlayerController>();
        pedPosition=new Vector3(3.3f,0.8f,-1.72f);
    }

    void Update()
    {
        // 获取车辆与观察者之间的距离
        float distanceToObserver = Vector3.Distance(transform.position, pedPosition);

        // 根据距离计算音效的变化范围
        float t = Mathf.Clamp01((distanceToObserver - minDistance) / (maxDistance - minDistance));

        // 根据t值来设置音调
        float newPitch = Mathf.Lerp(minPitch, maxPitch, t);

        // 设置AudioSource的音调
        audioSource.pitch = newPitch;

        // 根据t值来设置音量，例如t值从0到1，你可以设置音量从0到1
        float minVolume = -15f; // 最小音量
        float maxVolume = 50f; // 最大音量

        float newVolume = Mathf.Lerp(minVolume, maxVolume, t);

        // 设置AudioSource的音量
        audioSource.volume = newVolume;
    }
}
