using System;
using System.Collections.Generic;
using GameLogic.Moves;
using GameLogic.Signals;
using ServiceObjects;
using Zenject;
namespace GameLogic.TurnHandlers
{
  public class TurnsController : IInitializable,IDisposable
  {
    private IPlayerTurnHandler _playerTurnHandler;
    private IPlayerTurnHandler _opponentTurnHandler;
    private IPlayerTurnHandler _currentTurnHandler;
    private Dictionary<PieceType, IMove> _moves;
    private SignalBus _signalBus;
    public TurnsController(Dictionary<PieceType, IMove> moves, SignalBus signalBus,PlayerTurnHandler playerTurnHandler,AIOpponent opponent)
    {
      _moves = moves;
      _signalBus = signalBus;
      _playerTurnHandler = playerTurnHandler;
      _opponentTurnHandler = opponent;
    }
    public void Initialize()
    {
      _currentTurnHandler = _playerTurnHandler;
      _currentTurnHandler.TurnEnded += TurnEnded;
    }
    public List<CellPlaceholder> PieceSelected(PieceInfo pieceInfo,CellPlaceholder[][] chessBoard)
    {
      _currentTurnHandler.PieceSelected(pieceInfo);
      return _moves[pieceInfo.Type].ShowPossibleMoves(pieceInfo.Color, pieceInfo.Position, chessBoard);
    }
    public void CellSelected(CellPlaceholder selectedCell)
    {
      _currentTurnHandler.CellSelected(selectedCell);
    }
    public void Dispose()
    {
      _currentTurnHandler.TurnEnded -= TurnEnded;
    }
    private void TurnEnded(Turn turn)
    {
      _signalBus.Fire(new TurnEndedSignal(turn));
    }
    public void ChangeTurnsHandler()
    {
      _currentTurnHandler.TurnEnded -= TurnEnded;
      _currentTurnHandler = _currentTurnHandler == _playerTurnHandler ? _opponentTurnHandler : _playerTurnHandler;
      _currentTurnHandler.TurnEnded += TurnEnded;
      _currentTurnHandler.StartTurn();
    }
  }
}
