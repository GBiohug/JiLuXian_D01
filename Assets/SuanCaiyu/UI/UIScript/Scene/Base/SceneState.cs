using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����״̬��������
/// </summary>
public abstract class SceneState
{
    /// <summary>
    /// ��������ʱ�Ĳ���
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// �����˳�ʱ�Ĳ���
    /// </summary>
    public abstract void OnExit();

}
