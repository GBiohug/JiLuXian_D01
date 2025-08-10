using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Common.Helper;
using System.Collections;


public class MoneyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI changedMonyText;
    [SerializeField]private int currentDisplayValue; // ��ǰ��ʾ��ֵ
    private int targetValue;       // Ŀ��ֵ
    private Tween countTween;      // �洢��������

    [SerializeField]private FadeUI fadeText;
    

    // ��ʼ��
    private void Start()
    {
        currentDisplayValue = 0;
        UpdateText();
    }

    
    // ���ý�Ǯֵ����������
    public void SetMoney(int newValue, float duration = 0.5f)
    {
        // ������ж����ڽ��У���ɱ����
        if (countTween != null && countTween.IsActive())
        {
            countTween.Kill();
        }

        targetValue = newValue;

        // ʹ��DOTween.Toʵ�����ֹ���
        countTween = DOTween.To(() => currentDisplayValue, x => {
            currentDisplayValue = x;
            UpdateText();
        }, targetValue, duration)
        .SetEase(Ease.OutQuad);
    }

    //����ƿ��
    public void AddBottle(int amount)
    {
        SetMoney(targetValue + amount, 0.5f);
    }

    //����ƿ��
    public void SubBottle(int amount)
    {
        SetMoney(targetValue - amount, 0.5f);
    }
    // ���ӽ�Ǯ
    public void AddMoney(int amount )
    {
        StartCoroutine(ChangeMoney_Increase(amount));
    }

    // ���ٽ�Ǯ
    public void SubtractMoney(int amount)
    {
        StartCoroutine (ChangeMoney_Decrease(amount));
        
    }

    // ����UI�ı�
    private void UpdateText()
    {
        moneyText.text = currentDisplayValue.ToString(); // N0��ʽ���ǧλ�ָ���
      
    }

    //���¸ı��Ǯ��UI�ı�
    private void IncreaseChangedText(int amount)
    {
        changedMonyText.text = $" +{amount.ToString()}";
    }

    private void DecreaseChangedText(int amount)
    {
        changedMonyText.text = $" -{amount.ToString()}";
    }

    /// <summary>
    /// ����ʾ���ӻ��߼��ٵ�Ǯ�����ٸ���UI�ı�
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator ChangeMoney_Increase(int amount)
    {
        IncreaseChangedText(amount);
        fadeText.FadeIn();    
        yield return new WaitForSeconds(0.8f);
        SetMoney(targetValue + amount, 0.5f);
        fadeText.FadeOut();
    }

    private IEnumerator ChangeMoney_Decrease(int amount)
    {
        DecreaseChangedText(amount);
        fadeText.FadeIn();
        yield return new WaitForSeconds(0.8f);
        SetMoney(targetValue - amount, 0.5f);
        fadeText.FadeOut();
    }
}