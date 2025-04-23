using Gameplay.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    public override void InstallBindings()
    {
        Container.BindInstance(_playerInput).AsSingle();
        Container.BindInstance(_playerRigidbody).AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMovementComponent>().AsSingle();
    }
}