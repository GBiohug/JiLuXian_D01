using ns.BagSystem;
using ns.BagSystem.Freamwork;
using Scy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������۽ű�
public class Slot : MonoBehaviour
{


    public Text text;

    public Image image;

    

    void ScrollCellIndex(int idx)
        {
             InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Material, out var itemLst);
             Debug.Log(idx);
           
            //�޸��ı�
            text.text = itemLst[idx].itemInfo.name;
            //�޸�������
            transform.name = itemLst[idx].itemInfo.name;
            //չʾͼƬ
            image.sprite = itemLst[idx].itemInfo.ItemIcon;

        }

        void ScrollCellReturn()
        {
            Debug.Log("���մ���");
        }
    }

