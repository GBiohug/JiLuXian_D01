using UnityEngine;
using UnityEngine.UI;

public class Emotionalbar : MonoBehaviour
{
    [Header("Slider Reference")]
    [SerializeField] private Slider pointerSlider;

    [Header("State Thresholds")]
    [SerializeField] private float lowThreshold = -0.75f;
    [SerializeField] private float highThreshold = 0.75f;

    // ��ǰָ��״̬
    public enum PointerState
    {
        Low,        // �͹�״̬ (< -0.75)
        Normal,     // ��ͨ״̬ (-0.75 ~ 0.75)
        High        // ����״̬ (> 0.75)
    }

    private PointerState currentState;

    private void Start()
    {
        // ��ʼ��Slider��Χ
        pointerSlider.minValue = -1f;
        pointerSlider.maxValue = 1f;
        pointerSlider.value = 0f;

        // ��ʼ״̬
        currentState = PointerState.Normal;
    }

    private void Update()
    {
        // ���״̬�仯
        CheckStateTransition();
    }

    #region ���з����ӿ� - ��ֵ����

    /// <summary>
    /// ����ָ��ֵ
    /// </summary>
    /// <param name="amount">����ֵ</param>
    public void IncreaseValue(float amount)
    {
        pointerSlider.value = Mathf.Clamp(pointerSlider.value + amount, -1f, 1f);
    }

    /// <summary>
    /// ����ָ��ֵ
    /// </summary>
    /// <param name="amount">����ֵ</param>
    public void DecreaseValue(float amount)
    {
        pointerSlider.value = Mathf.Clamp(pointerSlider.value - amount, -1f, 1f);
    }

    /// <summary>
    /// ����ָ��ֵ
    /// </summary>
    /// <param name="value">Ŀ��ֵ (-1��1֮��)</param>
    public void SetValue(float value)
    {
        pointerSlider.value = Mathf.Clamp(value, -1f, 1f);
    }

    /// <summary>
    /// ����ָ�뵽ƽ���(0)
    /// </summary>
    public void ResetToNeutral()
    {
        pointerSlider.value = 0f;
    }

    #endregion

    #region ���з����ӿ� - ״̬���

    /// <summary>
    /// ��ȡ��ǰָ��״̬
    /// </summary>
    public PointerState GetCurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// ����Ƿ��ڵ͹�״̬
    /// </summary>
    public bool IsInLowState()
    {
        return pointerSlider.value < lowThreshold;
    }

    /// <summary>
    /// ����Ƿ��ڿ���״̬
    /// </summary>
    public bool IsInHighState()
    {
        return pointerSlider.value > highThreshold;
    }

    /// <summary>
    /// ����Ƿ�����ͨ״̬
    /// </summary>
    public bool IsInNormalState()
    {
        return !IsInLowState() && !IsInHighState();
    }

    #endregion

    #region ˽�з���

    // ���״̬�仯�������¼�
    private void CheckStateTransition()
    {
        float currentValue = pointerSlider.value;
        PointerState newState;

        if (currentValue < lowThreshold)
        {
            newState = PointerState.Low;
        }
        else if (currentValue > highThreshold)
        {
            newState = PointerState.High;
        }
        else
        {
            newState = PointerState.Normal;
        }

        // ���״̬�����仯
        if (newState != currentState)
        {
            PointerState previousState = currentState;
            currentState = newState;
            OnStateChanged(previousState, currentState);
        }
    }

    // ״̬�仯�ص�
    private void OnStateChanged(PointerState oldState, PointerState newState)
    {
        Debug.Log($"״̬�仯: {oldState} -> {newState}");

        // ����������״̬�仯ʱ�ľ����߼�
        switch (newState)
        {
            case PointerState.Low:
                HandleLowStateEnter();
                break;
            case PointerState.Normal:
                HandleNormalStateEnter();
                break;
            case PointerState.High:
                HandleHighStateEnter();
                break;
        }
    }

    private void HandleLowStateEnter()
    {
        // �͹�״̬�����߼�
        Debug.Log("����͹�״̬");
    }

    private void HandleNormalStateEnter()
    {
        // ��ͨ״̬�����߼�
        Debug.Log("������ͨ״̬");
    }

    private void HandleHighStateEnter()
    {
        // ����״̬�����߼�
        Debug.Log("���뿺��״̬");
    }

    #endregion
}