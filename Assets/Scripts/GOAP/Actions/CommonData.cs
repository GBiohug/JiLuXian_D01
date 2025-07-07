using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;

using UnityEngine;

namespace GOAP.Actions
{
    public class CommonData : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}