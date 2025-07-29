using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ȫ�ֵĲ��������л�����
/// </summary>
public class GameControl : MonoBehaviour
{
 
    public static GameControl Instance { get; private set; }
    /// <summary>
    /// ����������
    /// </summary>
    public SceneSystem SceneSystem { get; private set; }

    private void Awake()
    {
        Instance = this;
        SceneSystem = new SceneSystem();
    }

    private void Start()
    {
        SceneSystem.SetScene(new StartScene());
    }
}


