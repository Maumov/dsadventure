using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public int Range = 100;
        public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

        CrossPlatformInputManager.VirtualAxis HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis VerticalVirtualAxis; // Reference to the joystick in the cross platform input

        public RectTransform JoystickCanvas;
        RectTransform rTransform;
        Vector2 startPos;
        Vector2 converter;
        Vector2 deltaPos;

        void OnEnable()
        {
            CreateVirtualAxes();
        }

        void Start()
        {
            rTransform = GetComponent<RectTransform>();
            startPos = rTransform.anchoredPosition;
        }

        void UpdateVirtualAxes(Vector2 value)
        {
            deltaPos = startPos - value;
            deltaPos.y = -deltaPos.y;
            deltaPos /= Range;

            HorizontalVirtualAxis.Update(-deltaPos.x);
            VerticalVirtualAxis.Update(deltaPos.y);
        }

        void CreateVirtualAxes()
        {
            HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(HorizontalVirtualAxis);

            VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(VerticalVirtualAxis);
        }

        public void OnDrag(PointerEventData data)
        {
            converter.x = data.position.x / Screen.width * JoystickCanvas.sizeDelta.x;
            converter.y = data.position.y / Screen.height * JoystickCanvas.sizeDelta.y;

            converter.Set(Mathf.Clamp(converter.x, startPos.x - Range, startPos.x + Range), Mathf.Clamp(converter.y, startPos.y - Range, startPos.y + Range));
            rTransform.anchoredPosition = converter;

            UpdateVirtualAxes(converter);
        }


        public void OnPointerUp(PointerEventData data)
        {
            rTransform.anchoredPosition = startPos;
            UpdateVirtualAxes(startPos);
        }


        public void OnPointerDown(PointerEventData data)
        {
            OnDrag(data);
        }

        void OnDisable()
        {
            HorizontalVirtualAxis.Remove();
            VerticalVirtualAxis.Remove();
        }
    }
}