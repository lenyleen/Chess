using UnityEngine;
using Zenject;
using System;
using ServiceObjects;

namespace InputHandler
{
    public class InputInstaller : Installer<Dimensions,InputInstaller>
    {
        private Settings _settings;
        private Dimensions _dimensions;
        [Inject]
        private void Constuct(InputInstaller.Settings settings,Dimensions dimensions)
        {
            _settings = settings;
            _dimensions = dimensions;
        }
        public override void InstallBindings()
        {
            var cameraInstance = Camera.main;
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle().NonLazy();
            BindSelection();
            Container.Bind<PieceSelectionState>().AsSingle().WithArguments(cameraInstance,_settings.piecesLayer);
            Container.Bind<PieceSelectedState>().AsSingle().WithArguments(cameraInstance,_settings.cellsLayer);
            Container.Bind<OpponentTurnState>().AsSingle();
            Container.Bind<PauseState>().AsSingle();
            Container.Bind<StateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
        }
        private void BindSelection()
        {
            if (_dimensions == Dimensions.ThreeDimensional)
            {
                Container.BindInterfacesAndSelfTo<Selection3D>().AsSingle();
                return;
            }
            Container.BindInterfacesAndSelfTo<Selection2D>().AsSingle();
        }
        [Serializable]
        public class Settings
        {
            public LayerMask piecesLayer;
            public LayerMask cellsLayer;
        }
    }
}