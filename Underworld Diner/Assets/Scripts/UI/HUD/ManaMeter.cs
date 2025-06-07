using Interfaces;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD
{
    public class ManaMeter : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Image _mask;

        private float _manaMax;

        public void Awake()
        {
            _signalBus.Subscribe<OnManaSetupSignal>(SetUpManaMeter);
            _signalBus.Subscribe<OnManaUpdateSignal>(UpdateManaMeter);
        }

        private void SetUpManaMeter(OnManaSetupSignal setupSignal)
        {
            _manaMax = setupSignal.Mana;
            _mask.fillAmount = 1;
        }

        private void UpdateManaMeter(OnManaUpdateSignal updateSignal)
        {
            _mask.fillAmount = updateSignal.Mana / _manaMax;
        }
    }
}