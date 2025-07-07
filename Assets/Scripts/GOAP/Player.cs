using UnityEngine;

namespace GOAP
{
    public class Player:MonoBehaviour
    {
     
        public void Awake()
        {
          Debug.Log(gameObject.name + ":Awake");
          Debug.Log(gameObject.transform.name + ":Pos");
        }
    }
}