using UnityEngine;
using Zenject;
namespace InputHandler
{
  public class Selection3D : ISelection
  {
    public ISelectable CheckSelection(LayerMask layerMask,Camera mainCamera)
    {
      var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
      if (!Physics.Raycast(ray, out RaycastHit raycastHit, 999f,layerMask)) 
        return null;
      return raycastHit.collider.TryGetComponent(out ISelectable selectable) ? selectable : null;
    }
  }
  public class Selection2D : ISelection
  {
    public ISelectable CheckSelection(LayerMask layerMask,Camera mainCamera)
    {
      var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
      var raycastHit = Physics2D.Raycast(ray.origin, ray.direction, 999f, layerMask);
      if (raycastHit.collider == null) 
        return null;
      return raycastHit.collider.TryGetComponent(out ISelectable selectable) ? selectable : null;
    }
  }
  public interface ISelection
  {
    public ISelectable CheckSelection(LayerMask layerMask,Camera mainCamera);
  }
}
