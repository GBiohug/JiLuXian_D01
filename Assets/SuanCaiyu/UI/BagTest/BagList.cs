using UnityEngine;
using System.Collections.Generic;
using ns.BagSystem;


/// <summary>
/// �������ĸ��ӣ����洢Ԥ������Ϣ����ʵ������
/// </summary>
public class BagList : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private GameObject slotPrefab; // ����SlotԤ����
    [SerializeField] private int initialSlotCount = 50; // ��ʼ��������

    [Header("����ʱ����")]
    public List<SlotBase> bagItems = new List<SlotBase>();


    void Awake()
    {
        InitializeSlotPrefabReferences();
    }

    /// <summary>
    /// ��ʼ��SlotԤ�������õ��б�
    /// </summary>
    private void InitializeSlotPrefabReferences()
    {
        bagItems.Clear();

        if (slotPrefab == null)
        {
            Debug.LogError("δ����SlotԤ���壡", this);
            return;
        }

        SlotBase prefabSlotComponent = slotPrefab.GetComponent<SlotBase>();
        if (prefabSlotComponent == null)
        {
            Debug.LogError("ָ����Ԥ���岻����Slot�����", slotPrefab);
            return;
        }

        for (int i = 0; i < initialSlotCount; i++)
        {
            
            // bagItems.Add(prefabSlotComponent);�Ǵ����д�����൱�ڽ�һ��Ԥ���帴����49�ݣ�����������Դ������ͬ��
            // �����µ�GameObjectʵ��
            GameObject slotObj = Instantiate(slotPrefab);
            slotObj.SetActive(false); // ��������ʾ

            // ��ȡ��ʵ���ϵ����
            SlotBase newSlot = slotObj.GetComponent<SlotBase>();
            bagItems.Add(newSlot);
        }


        foreach (var slot in bagItems)
        {
            slot.text_1.text = "";
            slot.image_1.sprite = null;
        }

        //�������Ʒ��Ϣ��ӵ�����
        InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Material, out var itemLst);
        Debug.Log("��ȡ�������Ʒ��Ϣ");
        for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
        {
            bagItems[i].text_1.text = itemLst[i].itemInfo.name;
            bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
        }

        for (int i = 0; i < initialSlotCount; i++)
        {
            Debug.Log(bagItems[i].text_1.text);
        }



        Debug.Log($"�ѳɹ���� {bagItems.Count} ��Slot��Ϣ���б�");
    }

    //private Slot InstantiateSlotInfo(Slot source, int index)
    //{
    //    // �����µ�Slotʵ���������ݣ�����GameObject��
    //    Slot newSlot = new Slot();

    //    // ������Ը�����Ҫ������
    //    // ���磺newSlot.text = source.text;
    //    // ע�⣺����Ҫ�޸�Slot��ʹ������л�

    //    return newSlot;
    //}

    /// <summary>
    /// ��ȡָ��������Slot��Ϣ
    /// </summary>
    public SlotBase GetSlotInfo(int index)
    {
        if (index >= 0 && index < bagItems.Count)
        {
            return bagItems[index];
        }

        Debug.LogWarning($"��������� {index} ������Χ (0-{bagItems.Count - 1})");
        return null;
    }
}