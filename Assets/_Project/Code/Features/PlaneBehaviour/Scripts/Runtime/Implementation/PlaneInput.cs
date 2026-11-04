using System;
using UnityEngine;

namespace PlaneBehaviour
{
    public class PlaneInput
    {
        private readonly float _joystickRadius;

        private Vector2 _pressPosition;

        public PlaneInput(float joystickRadius)
        {
            _joystickRadius = joystickRadius;
        }

        public event Action<Vector2> StickPressed;
        public event Action<float, float> StickChanged;
        public event Action StickReleased;

        public bool IsPressed { get; private set; }
        
        public float PitchInput { get; private set; }
        public float TurnInput { get; private set; }
        
        public bool DragStarted { get; private set; }
        public bool DragEnded { get; private set; }
        
        public Vector2 PointerPosition { get; private set; }

        public void Tick()
        {
            DragStarted = Input.GetMouseButtonDown(0);
            DragEnded = Input.GetMouseButtonUp(0);
            
            PointerPosition = Input.mousePosition;
            
            TickStick();
        }

        private void TickStick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsPressed = true;
                _pressPosition = Input.mousePosition;
                StickPressed?.Invoke(_pressPosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                IsPressed = false;
                StickReleased?.Invoke();
            }

            if (IsPressed == false)
            {
                PitchInput = 0f;
                TurnInput = 0f;
                return;
            }

            float fullDeflection = _joystickRadius * Screen.height;
            Vector2 drag = (Vector2)Input.mousePosition - _pressPosition;
            
            PitchInput = Mathf.Clamp(-drag.y / fullDeflection, -1f, 1f);
            TurnInput = Mathf.Clamp(drag.x / fullDeflection, -1f, 1f);

            StickChanged?.Invoke(TurnInput, PitchInput);
        }
    }
}