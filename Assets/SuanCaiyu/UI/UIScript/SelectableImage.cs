using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SelectableImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Image borderImage; // ��ɫ�߿�ͼƬ
    
    [Header("Hover Effects")]
    [SerializeField] private Color hoverColor = new Color(0.95f, 0.95f, 0.95f, 1f);
    [SerializeField] private Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private float hoverDuration = 0.15f;
    
    [Header("Selection Effects")]
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Vector3 selectedScale = new Vector3(1.3f, 1.3f, 1.3f);
    [SerializeField] private Color borderColor = new Color(1f, 1f, 1f, 0.8f); // ��ɫ��͸���߿�
    [SerializeField] private float selectionDuration = 0.2f;
    
    private Color originalColor;
    private Vector3 originalScale;
    private bool isSelected = false;
    
    private void Awake()
    {
        // �Զ���ȡ����
        if (targetImage == null) targetImage = GetComponent<Image>();
        if (targetTransform == null) targetTransform = transform;
        
        // ��ʼ��״̬
        originalColor = targetImage.color;
        originalScale = targetTransform.localScale;
        
        //��ʼʱ���ر߿�
        if (borderImage != null)
        {
            borderImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
    
    // �����ͣЧ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected) return;
        
        targetImage.DOColor(hoverColor, hoverDuration);
        targetTransform.DOScale(hoverScale, hoverDuration).SetEase(Ease.OutQuad);
    }
    
    // ����뿪Ч��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;
        
        targetImage.DOColor(originalColor, hoverDuration);
        targetTransform.DOScale(originalScale, hoverDuration).SetEase(Ease.InOutQuad);
    }
    
    // �������
    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionManager.Instance.SelectImage(this);
    }
    
    // ����ѡ��״̬
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        
        if (selected)
        {
            // ѡ��Ч��
            targetImage.DOColor(selectedColor, selectionDuration);
            targetTransform.DOScale(selectedScale, selectionDuration).SetEase(Ease.OutQuad);
            
            // ��ʾ��ɫ�߿���������еĻ���
            if (borderImage != null)
            {
                borderImage.DOColor(borderColor, selectionDuration);
            }
         
        }
        else
        {
            // ȡ��ѡ��Ч��
            targetImage.DOColor(originalColor, selectionDuration);
            targetTransform.DOScale(originalScale, selectionDuration).SetEase(Ease.InOutQuad);
            
            // ���ذ�ɫ�߿�
            if (borderImage != null)
            {
                borderImage.DOColor(new Color(1f, 1f, 1f, 0f), selectionDuration);
            }
        }
    }
}