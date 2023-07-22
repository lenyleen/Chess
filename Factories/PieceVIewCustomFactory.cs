using ServiceObjects;
using Unity.Mathematics;
using UnityEngine;
using Views;
using Zenject;
namespace DefaultNamespace
{
  public class PieceVIewCustomFactory : IFactory<PieceView,Position,PieceView>
  {
    private DiContainer _container;
    public PieceVIewCustomFactory(DiContainer container)
    {
      _container = container;
    }
    public PieceView Create(PieceView prefab,Position position)
    {
      var pieceView = _container.InstantiatePrefabForComponent<PieceView>(prefab, position.WorldPosition, new Quaternion(-1,0,0,1), null);
      pieceView.Constuct(new PieceInfo(position,prefab.Info.Color,prefab.Info.Type));
      return pieceView;
    }
  }
}
