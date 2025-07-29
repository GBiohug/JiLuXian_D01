using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�Ĺ����ߣ�������ȡĳ���Ӷ�������Ĳ���
/// </summary>
public class UITool : MonoBehaviour
{   /// <summary>
/// ��ǰ�Ļ���
/// </summary>
    GameObject activeLayer;

    public UITool(GameObject layer)
    {
        activeLayer = layer; 
    }

    /// <summary>
    /// ����ǰ�����ȡ�������һ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOrAddComponent<T>() where T : Component
    {
        if (activeLayer.GetComponent<T>() == null)
        {
            activeLayer.AddComponent<T>();
        }
        return (T)activeLayer.GetComponent<T>();
    }

    /// <summary>
    /// �������Ʋ���һ���Ӷ���
    /// </summary>
    /// <param name="name">�Ӷ�������</param>
    /// <returns></returns>
    public GameObject FindChildGameObject(string name)
    {
        Transform[] transforms = activeLayer.GetComponentsInChildren<Transform>();

        foreach (Transform item in transforms)
        {
            if(item.name == name)
            {
                return item.gameObject;
            }
        }
        Debug.LogWarning($"{activeLayer.name}���Ҳ�����Ϊ{name}���Ӷ���");
        return null;
    }

    /// <summary>
    /// �������ƻ�ȡһ���Ӷ�������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="name">�Ӷ��������</param>
    /// <returns></returns>
    public T GetOrAddComponentInChildren<T>(string name) where T : Component
    { 
      GameObject child = FindChildGameObject(name);
        if (child != null) 
        {
            if (child.GetComponent<T>() == null)
            {
                child.AddComponent<T>();
            }
            return (T)child.GetComponent<T>();
        }
        return null;
    }


}
