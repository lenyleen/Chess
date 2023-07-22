using Initializers.ServiceObjects;
using InputHandler;
using ServiceObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [FormerlySerializedAs("PiecesDada")]
    [SerializeField] private PiecesData piecesData;
    [SerializeField] private InputInstaller.Settings _inputSettings;
    public override void InstallBindings()
    {
        Container.BindInstance(piecesData).AsSingle();
        Container.Bind<LvlData>().FromMethod(LoadLvlData).AsSingle().NonLazy();
        Container.BindInstance(_inputSettings).AsSingle();
    }

    private LvlData LoadLvlData()
    {
        var loader = new SaveManager();
        var data = loader.Load("1");
        return data;
    }
}