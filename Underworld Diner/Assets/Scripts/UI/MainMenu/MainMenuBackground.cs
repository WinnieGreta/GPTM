using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace UI.MainMenu
{
    public class MainMenuBackground : MonoBehaviour
    {
        [Serializable]
        public struct Layer
        {
            public Transform layer;
            public float magnitude;
            public Vector3 startLocalPos;
        }

        [Header("Layers")] 
        [SerializeField] private Layer[] _layers;

        [Header("Motion settings")] 
        [SerializeField] private float _maxOffset = 0.5f;
        [SerializeField] private float _smoothSpeed = 10f;
        [SerializeField] private Vector2 _deadZone;

        [Header("UI Input Action Map")]
        [SerializeField] private InputActionReference _pointAction;
        
        private Vector2 _current;

        public void Awake()
        {
            for (int i = 0; i < _layers.Length; i++)
            {
                _layers[i].startLocalPos = _layers[i].layer.localPosition;
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
            Vector2 normalized = new Vector2((position.x / Screen.width) * 2 - 1, (position.y / Screen.height) * 2 - 1);

            Vector2 target = new Vector2(DeadzoneAdjustments(normalized.x, _deadZone.x), 
                DeadzoneAdjustments(normalized.y, _deadZone.y)) * _maxOffset;

            _current = Vector2.Lerp(_current, target, Time.deltaTime * _smoothSpeed);
            
            foreach (var l in _layers)
            {
                l.layer.localPosition = l.startLocalPos + (Vector3)_current * l.magnitude;
            }
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
    }
}