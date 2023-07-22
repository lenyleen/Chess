using System;
using Chess_AI;
using DefaultNamespace;
using GameLogic.Signals;
using GameLogic.TurnHandlers;
using InputHandler;
using ServiceObjects;
using UnityEngine;
using Views;
using Zenject;
namespace Initializers
{
  public class GameInitializer : MonoInstaller
  {
    [Inject] private LvlData _lvlData; 
    [SerializeField] private Settings _settings;
    [SerializeField] private ChessBoardInitializer.Settings _chessBoardSettings;
    public override void InstallBindings()
    {
      SignalBusInstaller.Install(Container);
      AIInstaller.Install(Container);
      Container.DeclareSignal<TurnEndedSignal>();
      InputInstaller.Install(Container,_settings.Dimensions);
      Container.Bind<Dimensions>().FromInstance(_settings.Dimensions).AsSingle();
      TurnHandlersInstaller.Install(Container,1);
      Container.BindFactory<PieceView, Position, PieceView, PieceView.Factory>().FromFactory<PieceVIewCustomFactory>().NonLazy();
      Container.Bind<PieceSpawner>().AsSingle();
      ChessBoardInitializer.Install(Container,_chessBoardSettings);
    }
    [Serializable]
    public class Settings
    {
      public CellView cellPrefab;
      public int length;
      public Dimensions Dimensions;
    }
  }
}
