using System.Collections.Generic;
using Initializers.ServiceObjects;
using ServiceObjects;
using Views;
using Zenject;
namespace Initializers
{
  public class PieceSpawner
  {
    private Dictionary<PieceType, List<(int, int)>> _whitePiecesPositions;
    private Dictionary<PieceType, List<(int, int)>> _blackPiecesPositions;
    private PiecesData _piecesData;
    private PieceView.Factory _pieceFactory;
    private Dimensions _dimensions;
    public PieceSpawner(LvlData lvlData, PiecesData data, PieceView.Factory factory, Dimensions dimensions)
    {
      _whitePiecesPositions = lvlData.whiteFiguresPositions;
      _blackPiecesPositions = lvlData.blackFiguresPositions;
      _piecesData = data;
      _pieceFactory = factory;
      _dimensions = dimensions;
    }
    public PieceView[][] SpawnPieces(InjectContext context)
    {
      var pieces = new PieceView[8][]{new PieceView[8],new PieceView[8],new PieceView[8],new PieceView[8],new PieceView[8],new PieceView[8],new PieceView[8],new PieceView[8]};
      var cells = context.Container.Resolve<CellPlaceholder[][]>();
      SpawnPieces(cells, _whitePiecesPositions, PieceColor.White, _dimensions, pieces);
      SpawnPieces(cells, _blackPiecesPositions, PieceColor.Black, _dimensions, pieces);
      return pieces;
    }

    private void SpawnPieces(CellPlaceholder[][] cells, Dictionary<PieceType, List<(int, int)>> data, PieceColor color,Dimensions dimensions,PieceView[][] views)
    {
      var prefabsData = _piecesData.GetData(color,dimensions);
      foreach (var piece in data )
      {
        
        foreach (var position in piece.Value)
        {
           views[position.Item2][position.Item1]= _pieceFactory.Create(prefabsData[piece.Key],cells[position.Item2][position.Item1].Position);
           cells[position.Item2][position.Item1].PieceInfo = views[position.Item2][position.Item1].Info;
        }
      }
    }
  }
}
