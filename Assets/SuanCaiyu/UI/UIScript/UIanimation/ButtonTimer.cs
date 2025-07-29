using System.Collections;
using UnityEngine;
using Common.UI;
using System.Runtime.CompilerServices;

/// <summary>
/// �̵�UI��һЩ����
/// </summary>
public class ButtonTimer : MonoBehaviour
{
    public float StampAinmationTime = 0.6f;
    public float ExitAinmationTime = 0.6f;
    public float ChangeColorTime = 0.1f;
    public Animator stampAnimator;
    public Animator PayButtonAnimator;
    
    //public float delayTime_Close = 1.5f;
    //public float delayTime_Stamp = 0.3f;
    //public float delayTime_Exit = 1f;
    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        
    }
    //�Ƴ�����
    private void CloseShopUI()
    {
        Debug.Log("�ر��̵�ҳ�棡");
        UIManager.Instance.OpenUIGroup("MainUI");//�ر��̵�UI
    }

    //��ӡ�¶���
    public IEnumerator PayButton_Stamp()
    {

        Debug.Log("����ɹ���");
        stampAnimator.Play("Stamp");
        
        yield return new WaitForSeconds(StampAinmationTime);
    }

    //�˳�����
    public IEnumerator PayButton_Exit()
    {
        Debug.Log("�˳��̵꣡");
        animator.Play("ExitShop");
        yield return new WaitForSeconds(ExitAinmationTime);
        CloseShopUI();

    }

    //���򰴼����
    public IEnumerator ChangeColor()
    {
        Debug.Log("���㣬�޷�����");
        PayButtonAnimator.Play("ChangeColor");
        yield return new WaitForSeconds(ChangeColorTime);
    }

    //���򰴼����ԭɫ
    public IEnumerator ColorBack()
    {
        Debug.Log("���Լ������");
        PayButtonAnimator.Play("ColorBack");
        yield return new WaitForSeconds(ChangeColorTime);
    }

}
