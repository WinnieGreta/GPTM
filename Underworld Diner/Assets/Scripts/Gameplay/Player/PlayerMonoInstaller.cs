using Gameplay.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private Animator _animator;
    public override void InstallBindings()
    {
        Container.BindInstance(_playerInput).AsSingle();
        Container.BindInstance(_playerRigidbody).AsSingle();
        Container.BindInstance(_animator).AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMovementComponent>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAnimatorComponent>().AsSingle();
    }
}