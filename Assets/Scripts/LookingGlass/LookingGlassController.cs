using UnityEngine;

namespace LookingGlass
{
    public class LookingGlassController: MonoBehaviour
    {
        private MeshRenderer m_MeshRenderer;
        
        
        
        public void enableLookingGlass()
        {
            m_MeshRenderer.material.SetTexture("_ScreenTex", LookingGlassManager.GetScreenRT());
        }
    }

}