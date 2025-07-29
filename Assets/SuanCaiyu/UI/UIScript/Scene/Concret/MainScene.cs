using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main����
/// </summary>
public class MainScene : SceneState
{
    readonly string sceneName = "MainScene";
    LayerManager layerManager;

    /// <summary>
    /// ����ʱִ�еĲ���
    /// </summary>
    public override void OnEnter()
    {
        layerManager = new LayerManager();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            layerManager.Push(new StartLayer());
        }
    }

    /// <summary>
    /// �˳�ʱִ�еĲ���
    /// </summary>
    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;//ȡ��������

    }

    /// <summary>
    /// �����������֮��ִ�еķ���
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    public void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        //layerManager.Push(new ());
        Debug.Log($"{sceneName}�����������");
    }
}
