using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class KeyboardImageCycler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image DisplayImage;
    [SerializeField] private Image NextImage;
    [SerializeField] private Button cycleButton; // ��������ʾ��;

    [Header("Weapon Images")]
    [SerializeField] private List<Sprite> weaponImages = new List<Sprite>();

    [Header("Keyboard Settings")]
    [SerializeField] private KeyCode cycleKey = KeyCode.RightArrow;
    [SerializeField] private float keyRepeatDelay = 0.5f;
    [SerializeField] private float keyRepeatRate = 0.1f;

    private int currentIndex = 0;
    private int nextIndex = 1;
    private float lastKeyPressTime;

    private void Start()
    {
        // ���ð�ť�����������Ӿ�Ч��
        cycleButton.interactable = false;

        // ��ʼ����ʾ
        if (weaponImages.Count > 0)
        {
            UpdateWeaponDisplay();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(cycleKey))
        {
            CycleWeapon();
            lastKeyPressTime = Time.time;
        }
        else if (Input.GetKey(cycleKey) &&
                Time.time > lastKeyPressTime + keyRepeatDelay &&
                Time.time > lastKeyPressTime + keyRepeatRate)
        {
            CycleWeapon();
            lastKeyPressTime = Time.time;
        }
    }

    private void CycleWeapon()
    {
        if (weaponImages.Count == 0) return;

        // ģ�ⰴť����
        StartCoroutine(PlayButtonAnimation());

        // �л�����
        currentIndex = (currentIndex + 1) % weaponImages.Count;

        if(NextImage != null)
        {
             nextIndex = (currentIndex + 1) % weaponImages.Count;
        }
       
        UpdateWeaponDisplay();
    }

    private void UpdateWeaponDisplay()
    {
        DisplayImage.sprite = weaponImages[currentIndex];
        DisplayImage.color = new Color(1, 1, 1, DisplayImage.sprite ? 1 : 0);//͸����Ϊ0-1֮��İٷֱ�
        if (NextImage != null)
        {
            NextImage.sprite = weaponImages[nextIndex];
            NextImage.color = new Color(1, 1, 1 , 0.6f );
        }
    }

    private System.Collections.IEnumerator PlayButtonAnimation()
    {
        // ��ť����״̬
        cycleButton.GetComponent<Image>().color = cycleButton.colors.pressedColor;

        yield return new WaitForSeconds(0.1f);

        // ��ť�ָ�����״̬
        cycleButton.GetComponent<Image>().color = cycleButton.colors.normalColor;
    }

    // �༭��������ʱ�Զ���ȡ����
    private void Reset()
    {
        if (cycleButton == null)
            cycleButton = GetComponent<Button>();
        if (DisplayImage == null && cycleButton != null)
            DisplayImage = cycleButton.GetComponent<Image>();
    }
}