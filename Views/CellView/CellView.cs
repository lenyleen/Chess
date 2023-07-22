using System;
using InputHandler;
using UnityEngine;
using Zenject;
namespace Views
{
  public abstract class CellView : MonoBehaviour, ISelectable
  {
    public abstract (int,int) MatrixPosition { get; protected set; }
    public abstract event Action<(int,int)> OnSelectedEvent;
    public abstract void OnSelected();
    public abstract void HighLight(Color color, float alpha, bool enableCollision);
    public abstract void Reinitialize(Vector3 position, (int,int) matrixPosition);


    public class Pool : MonoMemoryPool<Vector3,bool, float,(int,int), CellView>
    {
      protected override void Reinitialize(Vector3 position, bool pieceOn, float alpha,(int,int) matrixPosition, CellView view)
      {
        view.Reinitialize(position,matrixPosition);
        var color = pieceOn ? Color.red : Color.green;
        view.HighLight(color,alpha,true);
      }
      protected override void OnDespawned(CellView item)
      {
        item.transform.position = Vector3.zero;
        item.HighLight(Color.green, 0f, false);
        item.gameObject.SetActive(false);
      }
    }
  }
}
