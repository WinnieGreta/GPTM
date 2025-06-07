using Signals;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerManaComponent : ITickable, IInitializable
    {
        [Inject] private PlayerHealthManaSettings _playerSettings;
        [Inject] private PlayerStatusComponent _statusComponent;
        [Inject] private SignalBus _signalBus;

        private float _maxMana;
        private float _manaRegainSpeed;
        
        public void Initialize()
        {
            _maxMana = _playerSettings.ManaMax;
            _statusComponent.Mana = _maxMana;
            _signalBus.Fire(new OnManaSetupSignal { Mana = _statusComponent.Mana });
            _manaRegainSpeed = _playerSettings.ManaRegainSpeed;
        }
        
        public void Tick()
        {
            if (_statusComponent.Mana == _maxMana)
            {
                return;
            }
            
            if (_statusComponent.Mana + _manaRegainSpeed < _maxMana)
            {
                _statusComponent.Mana += _manaRegainSpeed;
            }
            else
            {
                _statusComponent.Mana = _maxMana;
            }
            
            _signalBus.Fire(new OnManaUpdateSignal { Mana = _statusComponent.Mana});
        }

    }
}