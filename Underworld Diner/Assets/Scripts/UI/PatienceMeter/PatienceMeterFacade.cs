using Interfaces;
using Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PatienceMeter
{
    public class PatienceMeterFacade : MonoBehaviour, IPatienceMeter, IPoolable<Transform, int>, IDespawnable
    {
        [Inject] private PatienceMeterFactoryInstaller.PatienceMeterPool _patienceMeterPool;
        [Inject] private RectTransform _containerTransform;
        [Inject] private Image _mask;

        [SerializeField] private Vector3 _patienceMeterOffset;

        private float _patienceMax;
        private float _patience;

        private const int UNITS_PER_HEART = 24;

        public void OnSpawned(Transform anchor, int hearts)
        {
            _containerTransform.position = anchor.position + _patienceMeterOffset;
            _patienceMax = hearts;
            _patience = _patienceMax;
            _mask.fillAmount = 1;
            _containerTransform.sizeDelta = new Vector2(UNITS_PER_HEART * hearts, _containerTransform.sizeDelta.y);
        }
        public void Despawn()
        {
            _patienceMeterPool.Despawn(this);
        }

        public void OnDespawned()
        {

        }

        public void UpdatePatienceMeter(float patience, Transform anchor)
        {
            _containerTransform.position = anchor.position + _patienceMeterOffset;
            if (_patience != patience)
            {
                _mask.fillAmount = patience / _patienceMax;
                _patience = patience;
            }
        }

    }
}