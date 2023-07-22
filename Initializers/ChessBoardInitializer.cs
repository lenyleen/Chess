using System;
using Controllers;
using GameLogic.TurnHandlers;
using Models;
using ServiceObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Views;
using Zenject;
namespace Initializers
{
  public class ChessBoardInitializer : Installer<ChessBoardInitializer.Settings,ChessBoardInitializer>
  {
    private Settings _settings;
    private Dimensions _dimensions;
    private PieceSpawner _pieceSpawner;
    [Inject]
    private void Construct(PieceSpawner pieceSpawner,Dimensions dimensions,Settings settings)
    {
      _pieceSpawner = pieceSpawner;
      _dimensions = dimensions;
      _settings = settings;
    }
    public override void InstallBindings()
    {
      Container.Bind<CellPlaceholder[][]>().FromMethod(SpawnCells).AsSingle();
      Container.Bind<PieceView[][]>().FromMethod(_pieceSpawner.SpawnPieces).AsSingle().NonLazy();
      Container.Bind<CellsHighlighter>().AsSingle();
      Container.Bind<CapturedPiecesPool>().AsSingle().WithArguments(_settings.WhitePiecesPool,_settings.BlackPiecesPool);
      Container.BindMemoryPool<CellView, CellView.Pool>().WithInitialSize(30).
        FromComponentInNewPrefab(_settings.cellView3DPrefab).UnderTransformGroup("Cell Highlights").NonLazy();
      Container.BindInterfacesAndSelfTo<ChessBoardModel>().AsSingle().NonLazy();
      Container.BindInterfacesAndSelfTo<ChessBoardController>().AsSingle().NonLazy();
    }
    private CellPlaceholder[][] SpawnCells()
    {
      var cells = new CellPlaceholder[_settings.Length][];
      var firstCellPos = _settings.PivotTransform.position;
      firstCellPos = _dimensions == Dimensions.ThreeDimensional ? firstCellPos : new Vector3(firstCellPos.x, firstCellPos.z, firstCellPos.y);
      var newY = firstCellPos.y;
      for (int row = 0; row < _settings.Length; row++)
      {
        var newZ =firstCellPos.z + (_settings.DistanceBetweenCellsCenters * row);
        cells[row] = new CellPlaceholder[_settings.Length];
        for (int column = 0; column < _settings.Length; column++)
        {
          var newX = firstCellPos.x - (_settings.DistanceBetweenCellsCenters * column);
          Position position = new Position(new Vector3(newX,newY,newZ),(row, column));
          cells[row][column] = new CellPlaceholder(null, position);
        }
      }
      return cells;
    }
    [Serializable]
    public class Settings
    {
      public int Length;
      public Transform BlackPiecesPool;
      public Transform WhitePiecesPool;
      [FormerlySerializedAs("CellViewPrefab")] public CellView3D cellView3DPrefab;
      public Transform PivotTransform;
      public float DistanceBetweenCellsCenters;
    }
  }
}
