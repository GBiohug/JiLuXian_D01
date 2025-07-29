using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������������ջ���洢UI
/// </summary>
public class LayerManager 
{
    /// <summary>
    /// �洢UI����ջ
    /// </summary>
    private Stack<BaseLayer> stackLayer;
    /// <summary>
    /// UI������
    /// </summary>
    private UIManager_1 uIManager;
    private BaseLayer layer;

    public LayerManager()
    {
        stackLayer = new Stack<BaseLayer>();
        uIManager = new UIManager_1();
    }

   /// <summary>
   /// UI��ջ��������ʾһ�����
   /// </summary>
   /// <param name="nextlayer"></param>
    public void Push(BaseLayer nextlayer)
    {
        if(stackLayer.Count>0)
        {
            layer = stackLayer.Peek();
            layer.OnPause();
        }
        stackLayer.Push(nextlayer);
        GameObject layerget = uIManager.GetSingleUI(nextlayer.uitype);
        nextlayer.Initialize(new UITool(layerget));
        nextlayer.Initialize(this);
        nextlayer.Initialize(uIManager);
        nextlayer.OnEnter();//�ɷ�����Ż��õ��� 
    }
    
    /// <summary>
    /// ִ�����ĳ�ջ����
    /// </summary>
    public void Pop()
    {
        if (stackLayer.Count > 0)
        {
           stackLayer.Peek().OnExit();
           stackLayer.Pop();
        }
        if(stackLayer.Count>0)
        {
           stackLayer.Peek().OnResume();
        }
    }
}

