using System;
using System.Threading.Tasks;
using InputHandler;
using ServiceObjects;
using UnityEngine;
using Zenject;
namespace Views
{
  public abstract class PieceView : MonoBehaviour,ISelectable
  {
    [field: SerializeField]public PieceInfo Info { get; protected set; }
    public void Constuct(PieceInfo info)
    {
      Info = info;
    }
    public abstract Task Replace(Position position);
    public abstract Task Captured(Vector3 position);
    public abstract event Action<PieceView> OnSelectedEvent;
    public abstract void OnSelected();
    public abstract void Deselect();
    public class Factory : PlaceholderFactory<PieceView,Position,PieceView>
    {
      
    }
  }
}

