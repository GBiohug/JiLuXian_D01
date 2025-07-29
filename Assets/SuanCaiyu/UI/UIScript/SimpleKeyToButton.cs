using UnityEngine;
using UnityEngine.UI;

public class SimpleKeyToButton : MonoBehaviour
{
    [Header("��������")]
    public KeyCode triggerKey = KeyCode.Tab;

    [Header("��ť����")]
    public Button targetButton;

    private void Update()
    {
        // ��ⰴ�������Ұ�ť�ɽ���
        if (Input.GetKeyDown(triggerKey))
        {
            if (targetButton != null && targetButton.interactable)
            {
                // ֱ�Ӵ�������¼�
                targetButton.onClick.Invoke();
            }
        }
    }

    // �༭������������ʱ�Զ���ȡ��ť���
    private void Reset()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }
    }
}