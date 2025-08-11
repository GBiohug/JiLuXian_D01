using System;
using System.Collections.Generic;
using UnityEngine;
using static BagList;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;
    
    private SelectableImage currentlySelected;
    
    [SerializeField] private List<SelectableImage> selectedObjects = new List<SelectableImage>();
    [SerializeField] private int currentSelectionIndex = 0;

    public InfiniteScroll InfiniteScroll;

    //�뱳�����ӽӿ�
    [SerializeField] private BagList bagList;

    //��ȴʱ��
    public float minInterval = 0.5f;
    private float lastCallTime;
    // ��SelectionManager��
    public static event Action<ItemCategory> OnCategoryChanged;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentSelectionIndex = 0;
    }
    
    void Start()
    {
        if (InfiniteScroll.endInitialization)
        //���������еĺ������ɵ���������б�֮��
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                selectedObjects.Add(child.GetComponent<SelectableImage>());
            }
        }
    }
    
    public void SelectImage(SelectableImage newSelection)
    {

        if (currentlySelected == newSelection)
        {
            currentlySelected.SetSelected(false);
            currentlySelected = null;
            return;
        }
        
        if (currentlySelected != null)
        {
            currentlySelected.SetSelected(false);
        }
        
        currentlySelected = newSelection;
        currentlySelected.SetSelected(true);
        currentSelectionIndex = selectedObjects.IndexOf(currentlySelected);

        //�л�ʱ���ӱ�����Ʒ�����ж�
        bagList.currentCategory = currentSelectionIndex switch
        {
            6 or 16 => ItemCategory.Consumable,
            7 or 17 => ItemCategory.Material,
            8 or 18 => ItemCategory.Currency,
            9 or 19 => ItemCategory.HeadEquipment,
            10 or 0 or 20 => ItemCategory.BodyEquipment,
            11 or 21 or 1 => ItemCategory.KernelEquipment,
            12 or 2 => ItemCategory.Spell,
            13 or 3 => ItemCategory.Key,
            14 or 4 => ItemCategory.RightHandWeapon,
            15 or 5 => ItemCategory.LeftHandWeapon,
            _ => ItemCategory.None
        };

        bagList.InitializeSlotPrefabReferences();
    }

     public void moveRightOnEdge()
     {
        if (Time.time - lastCallTime < minInterval)
        {
            Debug.Log("��������̫Ƶ����������");
            return;
        }

        if (currentSelectionIndex >= 10 && (InfiniteScroll.content_rtf.localPosition.x <= -2965))
        {
            currentSelectionIndex -= 9;
        }
        else
        {
            currentSelectionIndex += 1;
        }

        if (currentSelectionIndex > 0)
        {
            SelectionManager.Instance.SelectImage(selectedObjects[currentSelectionIndex]);
        }

        lastCallTime = Time.time;
    }

    public void moveLeftOnEdge()
    {
        if (Time.time - lastCallTime < minInterval)
        {
            Debug.Log("��������̫Ƶ����������");
            return;
        }

        if (currentSelectionIndex <= 7 && (InfiniteScroll.content_rtf.localPosition.x >= -265))
        {
            currentSelectionIndex += 9;
        }
        else
        {
            currentSelectionIndex -= 1;
        }

        if (currentSelectionIndex > 0)
        {
            SelectionManager.Instance.SelectImage(selectedObjects[currentSelectionIndex]);
        }

        lastCallTime = Time.time;
    }

}