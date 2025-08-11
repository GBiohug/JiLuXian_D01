using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;


namespace Common.UI
{
    public class ImageCycler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image DisplayImage;
        [SerializeField] private Image NextImage;
        //[SerializeField] private Button cycleButton; // ��������ʾ��;

        [Header("Weapon Images")]
        [SerializeField] private List<Sprite> equipmentImages = new List<Sprite>();

        //[Header("Keyboard Settings")]
        //[SerializeField] private KeyCode cycleKey = KeyCode.RightArrow;
        //[SerializeField] private float keyRepeatDelay = 0.5f;
        //[SerializeField] private float keyRepeatRate = 0.1f;

        private int currentIndex = 0;
        private int nextIndex = 1;
        // private float lastKeyPressTime;

        private void Start()
        {
            // ���ð�ť�����������Ӿ�Ч��
            //cycleButton.interactable = false;

            // ��ʼ����ʾ
            if (equipmentImages.Count > 0)
            {
                UpdateImagerDisplay();
            }
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(cycleKey))
        //    {
        //        CycleImage();
        //        lastKeyPressTime = Time.time;
        //    }
        //    else if (Input.GetKey(cycleKey) &&
        //            Time.time > lastKeyPressTime + keyRepeatDelay &&
        //            Time.time > lastKeyPressTime + keyRepeatRate)
        //    {
        //        CycleImage();
        //        lastKeyPressTime = Time.time;
        //    }
        //}

        public void CycleImage()
        {
            if (equipmentImages.Count == 0) return;

            // ģ�ⰴť����
            StartCoroutine(PlayButtonAnimation());

            // �л�����
            currentIndex = (currentIndex + 1) % equipmentImages.Count;

            if (NextImage != null)
            {
                nextIndex = (currentIndex + 1) % equipmentImages.Count;
            }

            UpdateImagerDisplay();
        }

        private void UpdateImagerDisplay()
        {
            DisplayImage.sprite = equipmentImages[currentIndex];
            DisplayImage.color = new Color(1, 1, 1, DisplayImage.sprite ? 1 : 0);//͸����Ϊ0-1֮��İٷֱ�
            if (NextImage != null)
            {
                NextImage.sprite = equipmentImages[nextIndex];
                NextImage.color = new Color(1, 1, 1, 0.6f);
            }
        }

        private System.Collections.IEnumerator PlayButtonAnimation()
        {
            // ��ť����״̬
            DisplayImage.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);

            Debug.Log("�л�����");
            yield return new WaitForSeconds(0.1f);

            // ��ť�ָ�����״̬
            DisplayImage.GetComponent<Image>().color = new Color(1, 1, 1);
        }

        // �༭��������ʱ�Զ���ȡ����
        //private void Reset()
        //{
        //    if (cycleButton == null)
        //        cycleButton = GetComponent<Button>();
        //    if (DisplayImage == null && cycleButton != null)
        //        DisplayImage = cycleButton.GetComponent<Image>();
        //}
    }
}