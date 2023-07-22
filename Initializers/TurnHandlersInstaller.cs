using System.Collections.Generic;
using GameLogic.Moves;
using ServiceObjects;
using Zenject;
namespace GameLogic.TurnHandlers
{ 
  public class TurnHandlersInstaller : Installer<int,TurnHandlersInstaller>
  {
    private int _gameMode;
    public TurnHandlersInstaller(int gameMode)
    {
      _gameMode = gameMode;
    }
    public override void InstallBindings()
    {
      Container.Bind<Dictionary<PieceType, IMove>>().FromMethod(InitializeMoves);
      Container.BindFactory<PlayerType, Turn, Turn.Factory>().AsSingle();
      if (_gameMode == 1)
      {
        Container.BindInterfacesAndSelfTo<PlayerTurnHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<AIOpponent>().AsSingle();
      }
      Container.BindInterfacesAndSelfTo<TurnsController>().AsSingle();
      /*Container.BindInterfacesAndSelfTo<PlayerTurnsMngr>().AsSingle();*/
      /*Container.BindInterfacesAndSelfTo<COMTurnsMngr>().AsSingle();*/
    }
    private Dictionary<PieceType, IMove> InitializeMoves()
    {
      var moves =  new Dictionary<PieceType, IMove>()
      {
        {PieceType.Pawn,new PawnMoves( new List<(int, int)>() {(0, 1)},new List<(int, int)>() {(1, 1), (1, -1)}) },
        {PieceType.Rook, new RookMoves(  new List<(int, int)>() {(1, 0), (-1, 0), (0, 1), (0, -1)})},
        {PieceType.Bishop, new BishopMoves( new List<(int, int)>() {(1, 1), (-1, 1), (-1, -1), (1, -1)})},
        {PieceType.Knight, new KingMoves( new List<(int, int)>() {(2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (-1, 2), (-1, -2), (1, -2)})},
        {PieceType.King, new KingMoves(new List<(int, int)>() {(-1, 1), (1, -1), (-1, -1), (1, 1), (1, 0), (-1, 0), (0, 1), (0, -1)})},
      };
      moves.Add(PieceType.Queen,new QueenMoves(null,moves[PieceType.Rook],moves[PieceType.Bishop]));
      return moves;
    }
  }
}
