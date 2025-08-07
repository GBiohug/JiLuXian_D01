using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Statusvaluecontroller : MonoBehaviour
{
    [Header("Slider Reference")]
    [SerializeField] private Image statusValueimage;
    [SerializeField] private Image statusValueBack;
    [SerializeField] private Image backGround;

    [SerializeField] private float statusValueTime;

    [SerializeField] private float maxValue;

    public Ease easeType = Ease.OutQuad;

    /// <summary>
    /// ��������
    /// </summary>
    [Header("��ť����")]
    [SerializeField] private Button targetButton1;
    [SerializeField] private Button targetButton2;
    [SerializeField] private Button targetButton3;
    [SerializeField] private Button targetButton4;

    private void Start()
    {
        //��ʼ����ǰ���
        maxValue = statusValueimage.GetComponent<RectTransform>().rect.width;
        statusValueTime = 0.4f;


        // ��ӵ���¼�����
        if (targetButton1 != null && targetButton2 != null)
        {
            targetButton1.onClick.AddListener(StartCoroutineOnClick_increse);
            targetButton2.onClick.AddListener(StartCoroutineOnClick_decrese);
            targetButton3.onClick.AddListener(StartCoroutionOnClick_increseMax);
            targetButton4.onClick.AddListener (StartCoroutionOnClick_decreseMax);
        }
        else
        {
            Debug.LogError("δ�ҵ�Button���", this);
        }

    }

    /// <summary>
    /// �ⲿֱ�ӵ��õĽӿ�
    /// </summary>
    private void StartCoroutineOnClick_increse()
    {
        StartCoroutine(Increase(100f));
    }

    private void StartCoroutineOnClick_decrese()
    {
        StartCoroutine(Decrease(100f));
    }

    private void StartCoroutionOnClick_increseMax()
    {
        StartCoroutine(IncreseMax(100f));
    }

    private void StartCoroutionOnClick_decreseMax()
    {
        StartCoroutine(DecreseMax(100f));
    }




    /// <summary>
    /// ʵѪֱ������״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseValue(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(rectTransform.rect.width + amount, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

    }

    /// <summary>
    /// ʵѪֱ�Ӽ���״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseValue_im(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();
        Debug.Log("Ѫ�����٣�");
        float targetWidth = Mathf.Clamp(rectTransform.rect.width - amount, 0f, maxValue);

        rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// ��Ѫ�𲽸ı����ֵ 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public void ChangeValue_step()
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();
        RectTransform rectTransform_1 = statusValueBack.GetComponent<RectTransform>();

        float targetWidth = rectTransform.rect.width;
        
        rectTransform_1.DOSizeDelta(new Vector2(targetWidth, rectTransform_1.sizeDelta.y), statusValueTime)
           .SetEase(easeType);


    }

    /// <summary>
    /// ֱ����������
    /// </summary>
    /// <param name="amount"></param>
    public void IncreasemMax(float amount)
    {
        //�����������
        maxValue = maxValue + amount;
        RectTransform rectTransform = backGround.GetComponent<RectTransform>();
        
        float targetWidth = Mathf.Clamp(maxValue + 10f , 0f, maxValue +  10f);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);
    }

    /// <summary>
    /// ���ޱ仯ʱ��ѪʵѪ�𲽱仯
    /// </summary>
    public void IncreseValueToMax(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();
        RectTransform rectTransform_1 = statusValueBack.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(maxValue, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

        rectTransform_1.DOSizeDelta(new Vector2(targetWidth, rectTransform_1.sizeDelta.y), statusValueTime)
           .SetEase(easeType);
        
    }

    public void DecreasemMax(float amount)
    {
        //�����������
        maxValue = maxValue - amount;

        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(maxValue, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);
       
    }

    public void DecreseValueToMax(float amount)
    {
        RectTransform rectTransform = backGround.GetComponent<RectTransform>();
        RectTransform rectTransform_1 = statusValueBack.GetComponent<RectTransform>();
        RectTransform rectTransform_2 = statusValueimage.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(maxValue, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth + 10, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

        rectTransform_1.DOSizeDelta(new Vector2(targetWidth, rectTransform_1.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

    }


    #region ���з�������ֵ����
    /// <summary>
    /// ���յ��ⲿ��������Э��
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public IEnumerator Increase(float amount)
    {
        IncreaseValue(amount);
        yield return  new WaitForSeconds(0.2f);

        ChangeValue_step();

    }

     public IEnumerator Decrease(float amount)
    {
        DecreaseValue_im(amount);
        yield return new WaitForSeconds(0.2f);

        ChangeValue_step();

    }

    public IEnumerator IncreseMax(float amount)
    {
        IncreasemMax(amount);
        yield return new WaitForSeconds(0.1f);

        IncreseValueToMax(amount);
    }

    public IEnumerator DecreseMax(float amount)
    {
        DecreasemMax(amount);
        yield return new WaitForSeconds(0.1f);

        DecreseValueToMax(amount);
    }
    #endregion


}
