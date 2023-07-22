using System;
using System.Threading.Tasks;
using DG.Tweening;
using ServiceObjects;
using UnityEngine;
namespace Views
{
  [RequireComponent(typeof(BoxCollider))]
  public class PieceView3D : PieceView
  {
    private float _defaultYPosition;
    public override event Action<PieceView> OnSelectedEvent;
    private void Awake()
    {
      _defaultYPosition = this.transform.position.y;
    }
    public override void OnSelected()
    {
      var newYPosition = this.transform.position.y + 2;
      transform.DOMoveY(newYPosition, 0.2f).SetEase(Ease.OutBack).SetAutoKill(false);
      OnSelectedEvent?.Invoke(this);
    }
    public override async void Deselect()
    {
      await transform.DOMoveY(_defaultYPosition, 0.2f).SetEase(Ease.OutBack).SetAutoKill(false).AsyncWaitForCompletion();
    }
    public override async Task Replace(Position position)
    {
      await DOTween.Sequence().Append(transform.DOMove(new Vector3(position.WorldPosition.x,transform.position.y,position.WorldPosition.z), 0.2f).SetEase(Ease.OutCirc))
        .Append(transform.DOMoveY(_defaultYPosition, 0.2f)).SetAutoKill().AsyncWaitForCompletion();
      Info.Position = position;
    }
    public override async Task Captured(Vector3 position)
    {
      await transform.DOMove(position, 0.5f).SetEase(Ease.OutCirc).SetAutoKill().AsyncWaitForCompletion();
    }
  }
}
