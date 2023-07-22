using System.Collections.Generic;
using ServiceObjects;
using WithMVCS;

namespace ServiceObjects
{
  [System.Serializable]
  public struct PreloadedTurn
  {
        
    public PieceType piece;
    public (int, int) _initialCellPosition;
    public (int, int) _selectedCellPosition;
    public List<(int, int)> initialCells;
    public PreloadedTurn((int, int) initialCellPosition, (int, int) selectedCellPosition, List<(int, int)> initialCells, PieceType piece)
    {
      this.piece = piece;
      _initialCellPosition = initialCellPosition;
      _selectedCellPosition = selectedCellPosition;
      this.initialCells = initialCells;
                
    }
  }     
}

