using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller
{
#pragma warning disable
    [SerializeField] private GameConfig _gameConfig;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.BindInstance(_gameConfig);
    }
}