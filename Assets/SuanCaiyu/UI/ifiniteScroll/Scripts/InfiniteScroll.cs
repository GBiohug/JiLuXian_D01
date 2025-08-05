using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    public ScrollRect SR;
    public RectTransform viewPort_rtf;
    public RectTransform content_rtf;
    public HorizontalLayoutGroup HLG;
    public RectTransform[] ItemList;

    [SerializeField] private float moveDistance = 300f; // ÿ���ƶ��ľ���
    [SerializeField] private float moveSpeed = 1500f;   // �ƶ��ٶ�
    private Vector3 targetPosition;
    public bool isMovingLeft = false;
    public bool isMovingRight = false;
    public bool isJumping = false;

    //private bool isUpdated;
    //private Vector2 OldVelocity;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect component not found!");
            return;
        }

        // ��ֹ�����ק�����Կ�ͨ��������ƹ���
        scrollRect.enabled = false; // �������� ScrollRect ����

        targetPosition = content_rtf.localPosition;
        //isUpdated = false;
        //OldVelocity = Vector2.zero;//����ֹ������

        //Mathf.CeilToInt(a)��á�a����С����
        //ǰ�����Ҫ������ٸ�
        int AddNum = Mathf.CeilToInt(viewPort_rtf.rect.width / (ItemList[0].rect.width + HLG.spacing));
        //�ұ߲���(ABC...)
        for (int i = 0; i < AddNum; i++)
        {
            RectTransform rtf = Instantiate(ItemList[i % ItemList.Length], content_rtf);
            rtf.SetAsLastSibling();
        }
        //��߲���(ZYX...)
        for (int i = 0; i < AddNum; i++)
        {
            //����ѭ��
            int j = ItemList.Length - 1 - i;
            while (j < 0)
            {
                j += ItemList.Length;
            }
            RectTransform rtf = Instantiate(ItemList[j], content_rtf);
            rtf.SetAsFirstSibling();
        }

        //Ĭ����ʾԭʼ��(x������߲�����)
        content_rtf.localPosition = new Vector3(-(ItemList[0].rect.width + HLG.spacing)*AddNum + (viewPort_rtf.rect.width- ItemList[0].rect.width)/2,
                                                content_rtf.localPosition.y, 
                                                content_rtf.localPosition.z);
    }


    void Update()
    {
        if (isJumping)
        {
            isJumping = false;
        }
        //��߲�������ʱ��ת��ԭʼ��
        if (content_rtf.localPosition.x > 0)
        {   
            isJumping = true;
            Debug.Log("isJumping");
            //ǿ��ˢ�£��������������ֵ����
            Canvas.ForceUpdateCanvases();
            //OldVelocity = SR.velocity;
            targetPosition -= new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            content_rtf.localPosition -= new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            //isUpdated = true;
           
        }
        //�ұ߲�������ʱ��ת��ԭʼ��
        if (content_rtf.localPosition.x < -(ItemList[0].rect.width + HLG.spacing) * ItemList.Length)
        {   
            isJumping = true;
            Debug.Log("isJumping");
            Canvas.ForceUpdateCanvases();
            //OldVelocity = SR.velocity;
            targetPosition += new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            content_rtf.localPosition += new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            //isUpdated = true;
           
        }

        if (isMovingLeft)
        {
           
            // ƽ���ƶ���Ŀ��λ��
            content_rtf.localPosition = Vector3.MoveTowards(content_rtf.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // ����Ѿ�����Ŀ��λ�ã�ֹͣ�ƶ�
            if (content_rtf.localPosition == targetPosition)
            {
                isMovingLeft = false;
            }
        }

        if (isMovingRight)
        {
            
            // ƽ���ƶ���Ŀ��λ��
            content_rtf.localPosition = Vector3.MoveTowards(content_rtf.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            // ����Ѿ�����Ŀ��λ�ã�ֹͣ�ƶ�
            if (content_rtf.localPosition == targetPosition)
            {
                isMovingRight = false;
            }
        }

    }

    public void MovingLeft()
    {
        
      
        if (!isMovingLeft)
        {
            // �����µ�Ŀ��λ�ã������ƶ���
            targetPosition = content_rtf.localPosition + Vector3.left * moveDistance;
            isMovingLeft = true;
        }
    }

    public void MovingRight()
    {


        if (!isMovingRight)
        {
            // �����µ�Ŀ��λ�ã������ƶ���
            targetPosition = content_rtf.localPosition + Vector3.right * moveDistance;
            isMovingRight = true;
        }
    }

}
