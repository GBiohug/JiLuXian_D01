using UnityEngine;
using UnityEngine.UI;

// ������۽ű�
public class Slot : MonoBehaviour
{
    public Text text;
    public Text description;
    public Image image;
    public int number;

    // ������Ҫ�洢BagList���ã���Ϊ���ǻ�ͨ���������ȡ
    private BagList bagList;

    void ScrollCellIndex(int idx)
    {
        // �Ӹ������ȡBagList���
        if (bagList == null)
        {
            bagList = GetComponentInParent<BagList>();
            if (bagList == null)
            {
                Debug.LogError("�޷��ҵ��������ϵ�BagList���");
                return;
            }
        }

        // ��������Ƿ���Ч
        if (idx < 0 || idx >= bagList.bagItems.Count)
        {
            Debug.LogError($"��Ч������: {idx}, �б���: {bagList.bagItems.Count}");
            return;
        }

        // ��ȡ��Ӧ��Ʒ
        var item = bagList.bagItems[idx];
        if (item == null)
        {
            Debug.LogError($"���� {idx} ������ƷΪnull");
            return;
        }

        // ����UI
        if (text != null)
        {
            text.text = item.text_1.text; // ����BagItem��itemName����
            transform.name = item.text_1.text;
        }

        if (image != null && item.image_1 != null) // ����BagItem��icon����
        {
            image.sprite = item.image_1.sprite;
        }

        // ������������
        if (description != null)
        {
            //description.text = item.text.text; // ����BagItem��description����
        }

        number = idx; // �洢��ǰ����
        //Debug.Log(number);
    }

    void ScrollCellReturn()
    {
        Debug.Log("���մ���");
        // �������������ò��״̬
        if (text != null) text.text = "";
        if (description != null) description.text = "";
        if (image != null) image.sprite = null;
        number = -1;
    }
}
