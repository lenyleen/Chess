using System.Collections.Generic;
using ServiceObjects;
using UnityEngine;
using Views;
namespace Controllers
{
  public class CapturedPiecesPool
  {
    private Dictionary<PieceColor, Transform> _poolsTransforms;
    private Dictionary<PieceColor, Stack<PieceView>> _capturedPieces;
    public CapturedPiecesPool(Transform whitePiecesPool, Transform blackPiecesPool)
    {
      _poolsTransforms = new Dictionary<PieceColor, Transform>()
      {
        {PieceColor.White, whitePiecesPool},
        {PieceColor.Black, blackPiecesPool}
      };
      _capturedPieces = new Dictionary<PieceColor, Stack<PieceView>>()
      {
        {PieceColor.White, new Stack<PieceView>()},
        {PieceColor.Black, new Stack<PieceView>()}
      };
    }
    public void CapturePiece(PieceView view)
    { 
       var curPoolTransform = _poolsTransforms[view.Info.Color];
      _capturedPieces[view.Info.Color].Push(view);
      view.transform.SetParent(curPoolTransform);
      view.Captured(curPoolTransform.position);
    }
    public PieceView GetCapturedPiece(PieceColor color)
    {
      var view = _capturedPieces[color].Pop();
      view.transform.SetParent(null);
      return view;
    }
  }
}
