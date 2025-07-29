using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// ��ʼ�����
/// </summary>
public class StartLayer :BaseLayer
{
    static readonly string path = "Prefabs/UI/Start/StartUI";
  public StartLayer():base(new UIType(path)) { }

    public override void OnEnter()
    {
        UITool.GetOrAddComponentInChildren<Button>("����").onClick.AddListener(()=>
        {
            //����¼�����д����
           UnityEngine.Debug.Log("���ð�ť���");
            LayerManager.Push(new SettingLayer());
        });

        UITool.GetOrAddComponentInChildren<Button>("������Ϸ").onClick.AddListener(() =>
        {
            //����¼�����д����
            UnityEngine.Debug.Log("��ʼ��Ϸ��ť���");
            GameControl.Instance.SceneSystem.SetScene(new MainScene());
        });
    }
 

}