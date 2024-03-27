using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElementManager : MonoBehaviour
{
    // 用来存储场景中的元素
    private List<GameObject> sceneElements = new List<GameObject>();

    // 禁用场景中的所有元素
    public void DisableAllElements()
    {
        // 遍历场景中的元素并禁用它们
        foreach (var element in sceneElements)
        {
            element.SetActive(false);
        }
    }

    // 启用场景中的所有元素
    public void EnableAllElements()
    {
        // 遍历场景中的元素并启用它们
        foreach (var element in sceneElements)
        {
            element.SetActive(true);
        }
    }

    // 添加场景元素到管理器
    public void AddSceneElement(GameObject element)
    {
        if (element != null)
        {
            sceneElements.Add(element);
        }
    }
}
