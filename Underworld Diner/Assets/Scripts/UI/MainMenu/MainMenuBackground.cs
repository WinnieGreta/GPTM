using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.MainMenu
{
    public class MainMenuBackground : MonoBehaviour
    {
        [Serializable]
        public class Layer
        {
            public Transform layer;
            public float magnitude;
            public Vector3 StartLocalPos { get; set; }
        }

        [Header("Layers")] 
        [SerializeField] private Layer[] _layers;

        [Header("Motion settings")] 
        [SerializeField] private float _maxOffset;
        [SerializeField] private float _smoothSpeed;
        [SerializeField] private Vector2 _deadZone;

        [Header("UI Input Action Map")]
        [SerializeField] private InputActionReference _pointAction;
        
        private Vector2 _current;

        public void Awake()
        {
            for (int i = 0; i < _layers.Length; i++)
            {
                _layers[i].StartLocalPos = _layers[i].layer.localPosition;
            }
        }

        public void OnEnable()
        {
            _pointAction.action.Enable();
        }

        public void Update()
        {
            MoveBackgroundLayers();
        }

        private void MoveBackgroundLayers()
        {
            Vector2 position = _pointAction.action.ReadValue<Vector2>();
            
            if (!Application.isFocused)
            {
                SnapToCenter();
                return;
            }

            Vector2 target = new();

            if (!IsOnScreen(position))
            {
                target = Vector2.zero;
            }
            else
            {
                Vector2 normalized = new Vector2((position.x / Screen.width) * 2 - 1,
                    (position.y / Screen.height) * 2 - 1);

                target = new Vector2(DeadzoneAdjustments(normalized.x, _deadZone.x),
                    DeadzoneAdjustments(normalized.y, _deadZone.y)) * _maxOffset;
            }

            _current = Vector2.Lerp(_current, target, Time.deltaTime * _smoothSpeed);

            ApplyOffset();
            
        }

        private bool IsOnScreen(Vector2 position)
        {
            return !(position.x < 0 || position.x > Screen.width || position.y < 0 || position.y > Screen.height);
        }

        private float DeadzoneAdjustments(float vectorComponent, float deadzoneComponent)
        {
            float absVectorComponent = Mathf.Abs(vectorComponent);
            if (absVectorComponent > deadzoneComponent)
            {
                return (absVectorComponent - deadzoneComponent) / (1 - deadzoneComponent) * Mathf.Sign(vectorComponent);
            }

            return 0;
        }

        private void ApplyOffset()
        {
            foreach (var l in _layers)
            {
                l.layer.localPosition = l.StartLocalPos + (Vector3)_current * l.magnitude;
            }
        }

        private void SnapToCenter()
        {
            _current = Vector2.zero;
            ApplyOffset();
        }
    }
}