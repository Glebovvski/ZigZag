using UnityEngine;
using Zenject;

public class PlayerInputInstaller : MonoInstaller
{
    [SerializeField] private PlayerInput _playerInput;
    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
    }
}