using Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Common.UI
{
    public class MainUIFunc : MonoBehaviour
    {
        private StatuValueControl hpControl;
        private StatuValueControl mpControl;
        private StatuValueControl bossControl;
        private FadeUI bossFadeUI;
        private Emotionalbar emotionalbar;
        private ImageCycler rightWeapon;
        private ImageCycler leftWeapon;
        private ImageCycler upWeapon;
        private ImageCycler downWeapon;

        //public Emotionalbar emotionalbar;

        private void Start()
        {
            Transform hp = transform.Find("FirstUI/״̬��/HP");
             hpControl = hp.GetComponent<StatuValueControl>();
            Debug.Log("�ҵ�HP���ƣ�");

            Transform mp = transform.Find("FirstUI/״̬��/MP");
             mpControl = mp.GetComponent<StatuValueControl>();
            Debug.Log("�ҵ�MP���ƣ�");

            Transform boss = transform.Find("FirstUI/BossHp");
            bossControl = boss.GetComponent<StatuValueControl>();
            bossFadeUI = boss.GetComponent<FadeUI>();
            Debug.Log("�ҵ�BossѪ�����ƺ��������ƣ�");

            Transform em = transform.Find("FirstUI/�����");
            emotionalbar = em.GetComponent<Emotionalbar>();
            Debug.Log("�ҵ����ֵ���ƣ�");

            Transform rw = transform.Find("FirstUI/װ����/��������");
            rightWeapon = em.GetComponent<ImageCycler>();
            Debug.Log("�ҵ�����������");

            Transform lw = transform.Find("FirstUI/װ����/��������");
            leftWeapon = em.GetComponent<ImageCycler>();
            Debug.Log("�ҵ�����������");

            Transform uw = transform.Find("FirstUI/װ����/����");
            upWeapon = em.GetComponent<ImageCycler>();
            Debug.Log("�ҵ����ܣ�");

            Transform dw = transform.Find("FirstUI/װ����/����");
            downWeapon = em.GetComponent<ImageCycler>();
            Debug.Log("�ҵ����ߣ�");


        }

        #region ���HP����
        /// <summary>
        /// �������Ѫ��
        /// </summary>
        /// <param name="amount"></param>
        public void IncreasePlayerHp(float amount)
        {
            hpControl.Increse(amount);
        }

        /// <summary>
        /// �������Ѫ��
        /// </summary>
        /// <param name="amount"></param>
        public void DecreasePlayerHp(float amount)
        {
            hpControl.Decrese(amount);
        }

        /// <summary>
        /// �������Ѫ��
        /// </summary>
        /// <param name="amount"></param>
        public void SetPlayerHp(float amount)
        {
            hpControl.SetValue(amount);
        }

        /// <summary>
        /// ����Ѫ������
        /// </summary>
        /// <param name="amount"></param>
        public void IncreasePlayerMaxHp(float amount)
        {
            hpControl.IncreseMaxOnly(amount);
        }

        /// <summary>
        /// �������Ѫ������
        /// </summary>
        /// <param name="amount"></param>
        public void DcreasePlyaerMaxHp(float amount)
        {
            hpControl.DecreseMax(amount);
        }

        /// <summary>
        /// �������Ѫ�����޲�����Ѫ
        /// </summary>
        /// <param name="amount"></param>
        public void IncresePlyerMaxHpToFull(float amount)
        {
            hpControl.IncreseMax_Full(amount);
        }

        #endregion

        #region ���MP����

        /// <summary>
        /// �����������
        /// </summary>
        public void IncreasePlayerMp(float amount)
        {
            mpControl.Increse(amount);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="amount"></param>
        public void DecreasePlayerMp(float amount)
        {
            mpControl.Decrese(amount);
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="amount"></param>
        public void IncreasePlayerMaxMp(float amount)
        {
            mpControl.IncreseMaxOnly(amount);
        }

        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="amount"></param>
        public void DcreasePlyaerMaxMp(float amount)
        {
            mpControl.DecreseMax(amount);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="amount"></param>
        public void SetPlayerMp(float amount)
        {
            mpControl.SetValue(amount);
        }

        /// <summary>
        /// ��������������޲�����Ѫ
        /// </summary>
        /// <param name="amount"></param>
        public void IncresePlyerMaxMpToFull(float amount)
        {
            mpControl.IncreseMax_Full(amount);
        }

        #endregion

        #region ���Ѫƿ��ƿ����
        #endregion

        #region ���BUFF����
        #endregion

        #region ������������
        /// <summary>
        /// ���ӵ�ǰ���ֵ
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseEmotion(float amount)
        {
            emotionalbar.IncreaseValue(amount);
        }

        /// <summary>
        /// ���ٵ�ǰ���ֵ
        /// </summary>
        /// <param name="amount"></param>
        public void DecreaseEmotion(float amount)
        {
            emotionalbar.DecreaseValue(amount);
        }

        /// <summary>
        /// ���ӵ�ǰ���ֵ�ٷֱ�����
        /// </summary>
        /// <param name="amount"></param>
        public void IncreseFullEmotion(float amount)
        {
            emotionalbar.IncreseMax(amount);
        }
        
        /// <summary>
        /// ���ٵ�ǰ���ֵ���ްٷֱ�
        /// </summary>
        /// <param name="amount"></param>
        public void DecreaseFullEmotion(float amount)
        {
            emotionalbar.DecreaseMax(amount);
        }

        /// <summary>
        /// �����������������ֵ����������
        /// </summary>
        /// <param name=""></param>
        public void SetCurrentEmotion(float newlow, float newhigh, float fullamount)
        {
            emotionalbar.ResetEmotionalBar(newlow, newhigh, fullamount);
        }
        #endregion

        #region ����л�����

        /// <summary>
        /// װ����������
        /// </summary>
        public void AddRightWeapon()
        {

        }

        /// <summary>
        /// �л���������
        /// </summary>
        public void SwitchRightWeapon()
        {
            rightWeapon.CycleImage();
        }

        /// <summary>
        /// װ����������
        /// </summary>
        public void AddLeftWeapon()
        {

        }

        /// <summary>
        /// �л���������
        /// </summary>
        public void SwitchLefttWeapon()
        {
            leftWeapon.CycleImage();
        }

        /// <summary>
        /// װ������
        /// </summary>
        public void AddUpWeapon()
        {

        }

        /// <summary>
        /// �л�����
        /// </summary>
        public void SwitchUpWeapon()
        {
            upWeapon.CycleImage();
        }

        /// <summary>
        /// װ������
        /// </summary>
        public void AddDownWeapon()
        {

        }

        /// <summary>
        /// �л�����
        /// </summary>
        public void SwitchDownWeapon()
        {
            downWeapon.CycleImage();
        }
        #endregion

        #region ��һ��ҷ���
        #endregion

        #region Boss����
        /// <summary>
        /// ����BOSSѪ����BOSS��Ѫʱ
        /// </summary>
        /// <param name="amount"></param>
        public void IncreseBossHp(float amount)
        {
            bossControl.IncreseBossHp(amount);
        }

        /// <summary>
        /// ����BOSSѪ����BOSS�ܻ�
        /// </summary>
        /// <param name="amount"></param>
        public void DecreseBossHp(float amount)
        {
            bossControl.DecreseBossHP(amount);
        }

        /// <summary>
        /// ����BOSS��ѪѪ��������BOSSսʱ����
        /// </summary>
        /// <param name="amount"></param>
        public void SetBossHp(float amount)
        {
            bossControl.SetBossHP(amount);
        }

        /// <summary>
        /// BOSSѪ�����֣�����BOSSս�����
        /// </summary>
        public void BossHp_On()
        {
            bossFadeUI.FadeIn();
        }

        /// <summary>
        /// BOSSѪ����ʧ���뿪BOSSս�����
        /// </summary>
        public void BossHp_Off()
        {
            bossFadeUI.FadeOut();
        }
        #endregion
    }
}
