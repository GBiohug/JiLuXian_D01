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

    [SerializeField] private float statusValueTime;
    
    /// <summary>
    /// ��������
    /// </summary>
    [Header("��ť����")]
    [SerializeField] private Button targetButton1;
    [SerializeField] private Button targetButton2;

    private void Start()
    {
        statusValueimage.fillAmount = 1;
        statusValueTime = 0.5f;


        // ��ӵ���¼�����
        if (targetButton1 != null && targetButton2 != null)
        {
            targetButton1.onClick.AddListener(StartCoroutineOnClick_increse);
            targetButton2.onClick.AddListener(StartCoroutineOnClick_decrese);
        }
        else
        {
            Debug.LogError("δ�ҵ�Button���", this);
        }

    }

    /// <summary>
    /// �������
    /// </summary>
    private void StartCoroutineOnClick_increse()
    {
        StartCoroutine(Increase(0.1f));
    }

    private void StartCoroutineOnClick_decrese()
    {
        StartCoroutine(Decrease(0.1f));
    }



   

    /// <summary>
    /// ֱ������״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseValue(float amount)
    {
        float currentAmount = Mathf.Clamp(statusValueimage.fillAmount + amount, 0f, 1f);

        DOTween.To(() => statusValueimage.fillAmount, x => statusValueimage.fillAmount = x, currentAmount, 0.2f)
          .OnUpdate(() => Debug.Log($"�ظ���ǰֵ: {statusValueimage.fillAmount}"))
          .OnComplete(() => Debug.Log("�ظ��ѴﵽĿ��ֵ"));
    }

    /// <summary>
    /// ֱ�Ӽ���״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseValue_im(float amount)
    {
        statusValueimage.fillAmount = Mathf.Clamp(statusValueimage.fillAmount - amount, 0f, 1f);
    }

    /// <summary>
    /// ������״ֵ̬
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public void ChangeValue_step()
    {
        //float currentValue = statusValueBack.fillAmount;

        DOTween.To(() => statusValueBack.fillAmount, x => statusValueBack.fillAmount = x, statusValueimage.fillAmount, statusValueTime)
           .OnUpdate(() => Debug.Log($"��ǰֵ: {statusValueBack.fillAmount}"))
           .OnComplete(() => Debug.Log("�ѴﵽĿ��ֵ"));
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
    #endregion


}
