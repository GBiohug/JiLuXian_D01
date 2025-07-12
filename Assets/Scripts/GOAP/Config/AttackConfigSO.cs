using UnityEngine;

namespace GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/Attack Config", fileName = "Attack Config", order = 2)]
    public class AttackConfigSO:ScriptableObject
    {
        public float MeleeAttackRadius = 5;
        [Tooltip("攻击冷却时间（秒）")]
        public float MeleeAttackCooldown = 6f;
    }
}