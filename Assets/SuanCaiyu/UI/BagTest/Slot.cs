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
            Debug.Log(idx);
            //�޸��ı�
            text.text = Inventory.Instance.inventoryItems[idx].itemName;
            //�޸�������
            transform.name = Inventory.Instance.inventoryItems[idx].itemName;
            //չʾͼƬ
            image.sprite = Inventory.Instance.inventoryItems[idx].icon;

        }

        void ScrollCellReturn()
        {
            Debug.Log("���մ���");
        }
    }

