using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatuValueControl : MonoBehaviour
{
    [Header("Slider Reference")]
    [SerializeField] private Image statusValueimage;
    [SerializeField] private Image statusValueBack;
    [SerializeField] private Image backGround;

    [SerializeField] private float statusValueTime;

    [SerializeField] private float maxValue;//��ʼѪ��Ϊ400
    [SerializeField] private float targetWidth;

    [Header("Boss HP")]
    [SerializeField] private float BossFullHp = 0;

    public Ease easeType = Ease.OutQuad;

    /// <summary>
    /// ��������
    /// </summary>
   // [Header("��ť����")]
   //// [SerializeField] private Button targetButton1;
   //// [SerializeField] private Button targetButton2;
   //// [SerializeField] private Button targetButton3;
   //// [SerializeField] private Button targetButton4;

    private void Start()
    {
        //��ʼ����ǰ���
        maxValue = statusValueimage.rectTransform.rect.width;
        //��ʼ��ʱ����
        statusValueTime = 0.4f;
        //��ʼ��Ŀ����
        targetWidth = maxValue;

        SetBossHP(1000f);
        // ��ӵ���¼�����
        //if (targetButton1 != null && targetButton2 != null)
        //{
        //    targetButton1.onClick.AddListener(() => Increse(100f));
        //    targetButton2.onClick.AddListener(() => Decrese(100f));
        //    targetButton3.onClick.AddListener(() => IncreseMaxOnly(100f));
        //    targetButton4.onClick.AddListener(() => DecreseMax(100f));
        //}
        //else
        //{
        //    Debug.LogError("δ�ҵ�Button���", this);
        //}

    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="amount"></param>
     public void Increse(float amount)
    {
        StartCoroutine(Increase(amount));
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="amount"></param>
    public void Decrese(float amount)
    {
        StartCoroutine(Decrease(amount));
    }

    /// <summary>
    /// ������Ѫ������
    /// </summary>
    public void IncreseMaxOnly(float amount)
    {
       
        IncreasemMax(amount);
    }

    /// <summary>
    /// ����Ѫ������ͬʱ����Ѫ
    /// </summary>
    /// <param name="amount"></param>
    public void IncreseMax_Full(float amount)
    {
        StartCoroutine(IncreseMaxFull(amount));
    }

    /// <summary>
    /// ����Ѫ�����ްٷֱȣ����÷����
    /// </summary>
    /// <param name="amount"></param>
    public void DecreseMax(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();
        if (rectTransform.rect.width >= maxValue)
        {
            StartCoroutine(DecreseMax_Full(amount));
        }
        else
        {
            DecreasemMaxOnly(amount);
        }
    }

    public void DecreseBossHP(float amount)
    {
        amount = (800 * amount) / BossFullHp;
        StartCoroutine(Decrease(amount));
    }
    //----------------------------------------------------------------------------�����Ǿ���ʵ��

    /// <summary>
    /// ʵѪֱ������״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    private void IncreaseValue(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();

         targetWidth = Mathf.Clamp(targetWidth + amount, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

    }

    /// <summary>
    /// ʵѪֱ�Ӽ���״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    private void DecreaseValue_im(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();
        Debug.Log("Ѫ�����٣�");
         targetWidth = Mathf.Clamp(targetWidth - amount, 0f, maxValue);

        rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// ��Ѫ�𲽸ı����ֵ 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
     private void ChangeValue_step()
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
    private void IncreasemMax(float amount)
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
     private void IncreseValueToMax(float amount)
    {
        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();
        RectTransform rectTransform_1 = statusValueBack.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(maxValue, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

        rectTransform_1.DOSizeDelta(new Vector2(targetWidth, rectTransform_1.sizeDelta.y), statusValueTime)
           .SetEase(easeType);
        
    }

    /// <summary>
    /// Ѫ����ʱ�����������
    /// </summary>
    /// <param name="amount"></param>
      private void DecreaseMaxFull(float amount)
    {
        //�����������
        maxValue = maxValue - amount;

        RectTransform rectTransform = statusValueimage.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(maxValue, 0f, maxValue);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);
       
    }

    /// <summary>
    /// ���������������ֵ
    /// </summary>
    /// <param name="amount"></param>
    private void DecreseValueToMax(float amount)
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

    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="amount"></param>
    private void DecreasemMaxOnly(float amount)
    {
        //�����������
        maxValue = maxValue - amount;

        RectTransform rectTransform = backGround.GetComponent<RectTransform>();

        float targetWidth = Mathf.Clamp(maxValue + 10 , 0f, maxValue + 10);

        rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), statusValueTime)
           .SetEase(easeType);

    }

    /// <summary>
    /// ���õ�ǰֵ���浵������ʱ����
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        RectTransform rectTransform = backGround.GetComponent<RectTransform>();
        RectTransform rectTransform_1 = statusValueBack.GetComponent<RectTransform>();
        RectTransform rectTransform_2 = statusValueimage.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(value + 10f , rectTransform.sizeDelta.y);
        rectTransform_1.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
        rectTransform_2.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// ��ʼ��Boss��ѪѪ��
    /// </summary>
    /// <param name="value"></param>
    public void SetBossHP(float value)
    {
        RectTransform rectTransform = backGround.GetComponent<RectTransform>();
        RectTransform rectTransform_1 = statusValueBack.GetComponent<RectTransform>();
        RectTransform rectTransform_2 = statusValueimage.GetComponent<RectTransform>();

        BossFullHp = value;

        rectTransform.sizeDelta = new Vector2(820f, rectTransform.sizeDelta.y);
        rectTransform_1.sizeDelta = new Vector2(800, rectTransform_1.sizeDelta.y);
        rectTransform_2.sizeDelta = new Vector2(800, rectTransform_1.sizeDelta.y);
    }
    #region Э�̣���ֵ����
    /// <summary>
    /// ���յ��ⲿ��������Э��
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator Increase(float amount)
    {
        IncreaseValue(amount);
        yield return  new WaitForSeconds(0.2f);

        ChangeValue_step();

    }

    private IEnumerator Decrease(float amount)
    {
        DecreaseValue_im(amount);
        yield return new WaitForSeconds(0.2f);

        ChangeValue_step();

    }

    private IEnumerator IncreseMaxFull(float amount)
    {
        IncreasemMax(amount);
        yield return new WaitForSeconds(0.1f);

        IncreseValueToMax(amount);
    }

    private IEnumerator DecreseMax_Full(float amount)
    {
        DecreaseMaxFull(amount);
        yield return new WaitForSeconds(0.1f);

        DecreseValueToMax(amount);
    }

   
    #endregion


}
