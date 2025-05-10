using Gameplay.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public override void InstallBindings()
    {
        Container.BindInstance(_playerInput).AsSingle();
        Container.BindInstance(_playerRigidbody).AsSingle();
        Container.BindInstance(_animator).AsSingle();
        Container.BindInstance(_transform).AsSingle();
        Container.BindInstance(_navMeshAgent).AsSingle();
        Container.BindInstance(_spriteRenderer).AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMovementComponent>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAnimatorComponent>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerNavigationComponent>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStatusComponent>().AsSingle();
    }
}