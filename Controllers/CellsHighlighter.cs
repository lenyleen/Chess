using System.Collections.Generic;
using ServiceObjects;
using Views;
namespace Controllers
{
  public class CellsHighlighter
  {
    private CellView.Pool _cellHighlightPool;
    public List<CellView> _activeCells { get; private set; }
    public CellsHighlighter(CellView.Pool cellHighlightPool)
    {
      _activeCells = new List<CellView>(16);
      _cellHighlightPool = cellHighlightPool;
    }
    public List<CellView> HighlightCells(List<CellPlaceholder> cellsToHighlight)
    {
      for (int i = 0; i < cellsToHighlight.Count; i++)
      {
        var position = cellsToHighlight[i].Position;
        var pieceOnCell = cellsToHighlight[i].PieceInfo != null;
        var cellView = _cellHighlightPool.Spawn(position.WorldPosition, pieceOnCell, 0.5f, position.MatrixPosition);
        _activeCells.Add(cellView);
      }
      return _activeCells;
    }
    public void DeactivateCells()
    {
      if(_activeCells.Count == 0) return;
      for (int i = 0; i < _activeCells.Count; i++)
      {
        _cellHighlightPool.Despawn(_activeCells[i]);
      }
      _activeCells.Clear();
    }
  }
}
