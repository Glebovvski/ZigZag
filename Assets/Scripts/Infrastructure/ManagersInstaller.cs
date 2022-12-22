using Managers;
using UnityEngine;
using Zenject;

public class ManagersInstaller : MonoInstaller
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private CollectibleManager collectibleManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ColorManager _colorManager;
    [SerializeField] private AudioManager _audioManager;
    public override void InstallBindings()
    {
        Container.Bind<ScoreManager>().FromInstance(_scoreManager).AsSingle();
        Container.Bind<CollectibleManager>().FromInstance(collectibleManager).AsSingle();
        Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<ColorManager>().FromInstance(_colorManager).AsSingle();
        Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle();
    }
}