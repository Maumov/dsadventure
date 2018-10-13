using UnityEngine;


namespace UnityStandardAssets.CrossPlatformInput
{
    public class MobileControlRig : MonoBehaviour
    {

        static MobileControlRig instance;
        Canvas canvas;

        private void Start()
        {
            instance = this;
            canvas = GetComponent<Canvas>();
            UnityEngine.EventSystems.EventSystem system = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

            if(system == null)
            {//the scene have no event system, spawn one
                GameObject o = new GameObject("EventSystem");

                o.AddComponent<UnityEngine.EventSystems.EventSystem>();
                o.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }

        public static void Show()
        {
            instance.canvas.enabled = true;
        }

        public static void Hide()
        {
            instance.canvas.enabled = false;
        }
    }
}