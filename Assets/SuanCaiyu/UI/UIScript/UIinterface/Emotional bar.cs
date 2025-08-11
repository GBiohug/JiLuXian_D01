using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Common.UI
{
    public class Emotionalbar : MonoBehaviour
    {
        [Header("Slider Reference")]
        [SerializeField] private Slider pointerSlider;
        [SerializeField] private Image lowImage;
        [SerializeField] private Image highImage;
        [SerializeField] private Image normalImage;
        [SerializeField] private Image mark1;
        [SerializeField] private Image mark2;
        [SerializeField] private Image decreseImage;

        [Header("State Thresholds")]
        [SerializeField] private float lowThreshold = 0f;
        [SerializeField] private float highThreshold = 0;

        [SerializeField] private float currentAmount;
        [SerializeField] private float maxAmount;

        [SerializeField] private float fullAmount;

        public Ease easeType = Ease.OutQuad;
        // ��ǰָ��״̬
        public enum PointerState
        {
            Low,        // �͹�״̬ (< 0.25)
            Normal,     // ��ͨ״̬ (0- 0.75)
            High        // ����״̬ (> 0.75)
        }

        private PointerState currentState;

        private void Start()
        {
            // ��ʼ��Slider��Χ
            pointerSlider.minValue = 0f;
            pointerSlider.maxValue = 1f;
            pointerSlider.value = 0.5f;
            currentAmount = 0.5f;
            maxAmount = 1f;
            fullAmount = 100f;
            // ��ʼ״̬
            currentState = PointerState.Normal;

            lowThreshold = 0.2f;
            highThreshold = 0.9f;

            RestImage();
            DecreaseMax(0);
        }

        private void Update()
        {
            // ���״̬�仯
            // CheckStateTransition();
        }

        #region ���з����ӿ� - ��ֵ����

        /// <summary>
        /// ����ָ��ֵ
        /// </summary>
        /// <param name="amount">����ֵ</param>
        public void IncreaseValue(float amount)
        {
            currentAmount = Mathf.Clamp(currentAmount + amount / fullAmount, 0f, maxAmount);
            DOTween.To(() => pointerSlider.value, x => pointerSlider.value = x, currentAmount, 0.2f);

        }

        /// <summary>
        /// ����ָ��ֵ
        /// </summary>
        /// <param name="amount">����ֵ</param>
        public void DecreaseValue(float amount)
        {
            currentAmount = Mathf.Clamp(currentAmount - amount / fullAmount, 0f, maxAmount);
            DOTween.To(() => pointerSlider.value, x => pointerSlider.value = x, currentAmount, 0.2f);
        }

        /// <summary>
        /// ����ָ��ֵ
        /// </summary>
        /// <param name="value">Ŀ��ֵ (-1��1֮��)</param>
        public void SetValue(float value)
        {
            pointerSlider.value = Mathf.Clamp(value / fullAmount, 0f, maxAmount);
        }

        /// <summary>
        /// ����ָ�뵽ƽ���(0)
        /// </summary>
        public void ResetToNeutral()
        {
            pointerSlider.value = 0.5f;

        }

        /// <summary>
        /// �����������������ֵ����������
        /// </summary>
        /// <param name="newlow"></param>
        /// <param name="newhigh"></param>
        public void ResetEmotionalBar(float newlow, float newhigh, float fullamount)
        {
            lowThreshold = newlow / fullamount;
            highThreshold = newhigh / fullamount;
            fullAmount = fullamount;
            RestImage();
        }

        /// <summary>
        /// ���ñ���ɫ��
        /// </summary>
        public void RestImage()
        {
            RectTransform rectTransform = lowImage.GetComponent<RectTransform>();
            RectTransform rectTransform_1 = normalImage.GetComponent<RectTransform>();
            RectTransform rectTransform_2 = highImage.GetComponent<RectTransform>();
            RectTransform rectTransform_3 = mark1.GetComponent<RectTransform>();
            RectTransform rectTransform_4 = mark2.GetComponent<RectTransform>();
            RectTransform rectTransform_5 = decreseImage.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(lowThreshold * 950, rectTransform.sizeDelta.y);
            rectTransform_1.sizeDelta = new Vector2((highThreshold - lowThreshold) * 950, rectTransform.sizeDelta.y);
            rectTransform_2.sizeDelta = new Vector2((1f - highThreshold) * 950, rectTransform.sizeDelta.y);
            rectTransform_5.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);

            rectTransform_1.anchoredPosition = new Vector2(-475f + rectTransform.sizeDelta.x, 0);
            rectTransform_3.anchoredPosition = rectTransform_1.anchoredPosition;
            rectTransform_4.anchoredPosition = new Vector2(475f - rectTransform_2.sizeDelta.x, 0);
        }

        /// <summary>
        /// �������ްٷֱ�
        /// </summary>
        /// <param name="amount"></param>
        public void DecreaseMax(float amount)
        {
            maxAmount = maxAmount - amount;

            if (maxAmount <= 0)
            {
                maxAmount = 0;
            }
            else
            {
                maxAmount = maxAmount;
            }

            RectTransform rectTransform = decreseImage.GetComponent<RectTransform>();
            float targetWidth = (1 - maxAmount) * 950;

            rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), 0.1f)
               .SetEase(easeType);

            if (currentAmount > maxAmount)
            {
                DecreaseValue(currentAmount - maxAmount);
            }

        }

        /// <summary>
        /// �������ްٷֱ�
        /// </summary>
        /// <param name="amount"></param>
        public void IncreseMax(float amount)
        {
            maxAmount = maxAmount + amount;
            if (maxAmount >= 1)
            {
                maxAmount = 1;
            }
            else
            {
                maxAmount = maxAmount;
            }
            RectTransform rectTransform = decreseImage.GetComponent<RectTransform>();

            float targetWidth = (1 - maxAmount) * 950f;

            rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.sizeDelta.y), 0.1f)
               .SetEase(easeType);

        }

        /// <summary>
        /// ��������������ĳ��ֵ
        /// </summary>
        /// <param name="amount"></param>
        public void IncreseFullAmount(float amount)
        {
            fullAmount = amount;
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
        //private void CheckStateTransition()
        //{
        //    float currentValue = pointerSlider.value;
        //    PointerState newState;

        //    if (currentValue < lowThreshold)
        //    {
        //        newState = PointerState.Low;
        //    }
        //    else if (currentValue > highThreshold)
        //    {
        //        newState = PointerState.High;
        //    }
        //    else
        //    {
        //        newState = PointerState.Normal;
        //    }

        //    // ���״̬�����仯
        //    if (newState != currentState)
        //    {
        //        PointerState previousState = currentState;
        //        currentState = newState;
        //        OnStateChanged(previousState, currentState);
        //    }
        //}

        // ״̬�仯�ص�
        //private void OnStateChanged(PointerState oldState, PointerState newState)
        //{
        //    Debug.Log($"״̬�仯: {oldState} -> {newState}");

        //    // ����������״̬�仯ʱ�ľ����߼�
        //    switch (newState)
        //    {
        //        case PointerState.Low:
        //            HandleLowStateEnter();
        //            break;
        //        case PointerState.Normal:
        //            HandleNormalStateEnter();
        //            break;
        //        case PointerState.High:
        //            HandleHighStateEnter();
        //            break;
        //    }
        //}

        //private void HandleLowStateEnter()
        //{
        //    // �͹�״̬�����߼�
        //    Debug.Log("����͹�״̬");
        //}

        //private void HandleNormalStateEnter()
        //{
        //    // ��ͨ״̬�����߼�
        //    Debug.Log("������ͨ״̬");
        //}

        //private void HandleHighStateEnter()
        //{
        //    // ����״̬�����߼�
        //    Debug.Log("���뿺��״̬");
        //}

        #endregion
    }
}