using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����UI���ĸ��࣬����UI����״̬��Ϣ
/// </summary>
public class BaseLayer
{  
    
    /// <summary>
    /// UI ��Ϣ
    /// </summary>
    public UIType uitype {  get; private set; }

    /// <summary>
    /// UI������
    /// </summary>
    public UITool UITool { get; private set; }

    /// <summary>
    /// ��������
    /// </summary>
    public LayerManager LayerManager { get; private set; }

    /// <summary>
    /// UI������
    /// </summary>
    public UIManager_1 UIManager { get; private set; }
    
    /// <summary>
    /// ��ʼ��UItype
    /// </summary>
    /// <param name="uitype"></param>
    public BaseLayer(UIType uitype)
    {
        this.uitype = uitype;
    }

    /// <summary>
    ///  ��ʼ��UITool
    /// </summary>
    /// <param name="tool"></param>
    public void Initialize(UITool tool)
    {
        UITool = tool;
    }
   /// <summary>
   /// ��ʼ��Layermanager
   /// </summary>
   /// <param name="manager"></param>
    public void Initialize(LayerManager manager)
    {
        LayerManager = manager;
    }

     /// <summary>
    /// ��ʼ��UIMaanager
    /// </summary>
    /// <param name="uiManager"></param>
    public void Initialize(UIManager_1 uIManager)
    {
        UIManager = uIManager;
    }

    /// <summary>
    /// UI����ʱִ�еĲ�����ֻ��ִ��һ��
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// UI��ͣʱִ�еĲ���
    /// </summary>
    public virtual void OnPause() 
    {
        UITool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;//�����������������ý���ΪFalseʵ����ͣ
    }

    /// <summary>
    /// UI����ʱִ�еĲ���
    /// </summary>
    public virtual void OnResume() 
    {
        UITool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// UI�˳�ʱִ�еĲ���
    /// </summary>
    public virtual void OnExit()
    { 
       UIManager.DestroyUI(uitype);
    }
}
