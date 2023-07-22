using System;
using UnityEngine;
namespace Views
{
  public class CellView3D : CellView
  {
    public override event Action<(int, int)> OnSelectedEvent;
    public override (int, int) MatrixPosition { get; protected set; }
    private Material _material;
    private BoxCollider _boxCollider;
    private void Awake()
    {
      _material = this.GetComponent<MeshRenderer>().material;
      _boxCollider = this.GetComponent<BoxCollider>();
    }
    public override void OnSelected()
    {
      OnSelectedEvent?.Invoke(MatrixPosition);
    }
    public override void HighLight(Color color, float alpha, bool enableCollision)
    {
      color.a = alpha;
      _material.color = color;
      _boxCollider.enabled = enableCollision;
    }
    public override void Reinitialize(Vector3 position, (int, int) matrixPosition)
    {
      this.transform.position = position;
      MatrixPosition = matrixPosition;
    }
  }
}
