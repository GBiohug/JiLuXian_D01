using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// Ҫ��������븽��UnityEngine.UI.LoopScrollRect���
[RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
// ��ֹ��ͬһ��GameObject�ϸ��Ӷ�������
[DisallowMultipleComponent]
public class BagScroller : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject item;// �б����Ԥ����
    // public int totalCount = -1;// �б���������-1��ʾ��������
    // �����ʵ�֣�ʾ����Stackʵ�֣�
    Stack<Transform> pool = new Stack<Transform>();

    /// <summary>
    /// ��ȡ�б������LoopScrollPrefabSource�ӿ�ʵ�֣�
    /// </summary>
    /// <param name="index">������</param>
    /// <returns>�б���GameObject</returns>
    public GameObject GetObject(int index)
    {
        // ��������Ϊ�գ�����ʵ����һ������
        if (pool.Count == 0)
        {
            return Instantiate(item);
        }

        // �Ӷ������ȡ��һ������
        Transform candidate = pool.Pop();
        candidate.gameObject.SetActive(true);// �������
        return candidate.gameObject;
    }

    /// <summary>
    /// �����б������LoopScrollPrefabSource�ӿ�ʵ�֣�
    /// </summary>
    /// <param name="trans">Ҫ���յ�Transform</param>
    public void ReturnObject(Transform trans)
    {
        // ���ͻ�����Ϣ����ѡ��
        trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);

        // ���ö������ø��ڵ�
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);

        // ������Żس���
        pool.Push(trans);
    }

    /// <summary>
    /// Ϊ�б����ṩ���ݣ�LoopScrollDataSource�ӿ�ʵ�֣�
    /// </summary>
    /// <param name="transform">�б����Transform</param>
    /// <param name="idx">������</param>
    public void ProvideData(Transform transform, int idx)
    {
        // ����������Ϣ��֪ͨ�б��������ʾ
        transform.SendMessage("ScrollCellIndex", idx);
    }

    void Start()
    {
        // ��ȡLoopScrollRect�������ʼ��
        var ls = GetComponent<LoopScrollRect>();
        ls.prefabSource = this;// ����Ԥ����Դ
        ls.dataSource = this;// ��������Դ
        ls.totalCount = Inventory.Instance.inventoryItems.Count;// ����������
        ls.RefillCells();// ��䵥Ԫ��
    }
}
