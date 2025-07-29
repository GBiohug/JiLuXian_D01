using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������״̬����ϵͳ
/// </summary>
public class SceneSystem 
{
    /// <summary>
    /// ����״̬��
    /// </summary>
    SceneState sceneState;

    /// <summary>
    /// ���ò����뵱ǰ����
    /// </summary>
    /// <param name="sceneState"></param>
    public void SetScene(SceneState state)
    {
        sceneState?.OnExit();
        sceneState=state;
        sceneState?.OnEnter();

    }
}
