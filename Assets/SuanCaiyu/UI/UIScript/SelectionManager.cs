using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;
    
    private SelectableImage currentlySelected;
    
    [SerializeField] private List<SelectableImage> selectedObjects = new List<SelectableImage>();
    [SerializeField] private int currentSelectionIndex = 0;

    public InfiniteScroll InfiniteScroll;

    //��ȴʱ��
    public float minInterval = 0.5f;
    private float lastCallTime;

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
    
    void Start(){
    //���������еĺ������ɵ���������б�֮��
      for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            selectedObjects.Add(child.GetComponent<SelectableImage>());
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
        
    }

     public void moveRightOnEdge()
     {
        if (Time.time - lastCallTime < minInterval)
        {
            Debug.Log("��������̫Ƶ����������");
            return;
        }

        if (currentSelectionIndex >= 8 && (InfiniteScroll.content_rtf.localPosition.x <= -2365))
        {
            currentSelectionIndex -= 7;
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

        if (currentSelectionIndex <= 5 && (InfiniteScroll.content_rtf.localPosition.x >= -265))
        {
            currentSelectionIndex += 7;
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