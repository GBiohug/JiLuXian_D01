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

    /// <summary>
    /// ���������ö�٣������Ϸ�ѡ���ͨ��
    /// </summary>
    public enum ItemCategory
    { None, Consumable, Material, Currency, HeadEquipment , BodyEquipment , KernelEquipment , Spell ,Key ,  RightHandWeapon , LeftHandWeapon } 
    public ItemCategory currentCategory = ItemCategory.None;

    public BagScroller bagScroller;
    public void OnEnable()
    {
        InitializeSlotPrefabReferences();
    }

    //private void Start()
    //{
    //    InitializeSlotPrefabReferences();
    //}
    /// <summary>
    /// ��ʼ��SlotԤ�������õ��б�
    /// </summary>
    public void InitializeSlotPrefabReferences()
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
        if (currentCategory == ItemCategory.Consumable)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Consumable, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
        }
        else if (currentCategory == ItemCategory.Material)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Material, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
            
        }
        else if (currentCategory == ItemCategory.Currency)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Currency, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
         
        }
        else if (currentCategory == ItemCategory.HeadEquipment)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.HeadEquipMent, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
            
        }
        else if (currentCategory == ItemCategory.BodyEquipment)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.BodyEquipment, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
     
        }
        else if (currentCategory == ItemCategory.KernelEquipment)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.KernelEquipment, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
         
        }
        else if (currentCategory == ItemCategory.Spell)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Spell, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
           
        }
        else if (currentCategory == ItemCategory.Key)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Key, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
           
        }
        else if (currentCategory == ItemCategory.RightHandWeapon)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.RightHandWeapon, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
            
        }
        else if (currentCategory == ItemCategory.LeftHandWeapon)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.LeftHandWeapon, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
           
        }
        else if(currentCategory == ItemCategory.None)
        {
            InventoryManager.Instance.GetItemLst(ns.ItemInfos.ItemType.Consumable, out var itemLst);
            for (int i = 0; i < itemLst.Count && i < bagItems.Count; i++)
            {
                bagItems[i].text_1.text = itemLst[i].itemInfo.name;
                bagItems[i].image_1.sprite = itemLst[i].itemInfo.ItemIcon;
            }
        }

        bagScroller.OnEnable();

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