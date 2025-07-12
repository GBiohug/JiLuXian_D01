using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace GOAP.WorldKeys
{
// 是否能看到玩家
    public class CanSeePlayer : WorldKeyBase{}
    

// 玩家怀疑度等级 (0-1)
    public class PlayerSuspicionLevel : WorldKeyBase{}
   

// 玩家是否被完全发现
    public class PlayerFullyDetected : WorldKeyBase{}


// 最后已知的玩家位置
    public class LastKnownPlayerPosition : WorldKeyBase{}
    

}