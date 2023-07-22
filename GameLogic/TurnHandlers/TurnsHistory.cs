using System;
using System.Collections.Generic;
using GameLogic;
using ModestTree;
using Zenject;

namespace WithMVCS.TurnsMngr
{
    public class TurnsHistory : IInitializable
    {
        private Stack<Turn> _turns;
        private SignalBus _signalBus;
        public TurnsHistory(SignalBus signalBus)
        {
            _turns = new Stack<Turn>();
            _signalBus = signalBus;
        }
        public void Initialize()
        {
        }
        public void WriteTurn(Turn turn)
        {
            _turns.Push(turn);
        }
        private async void UndoTurn(/*IUndoSignal signal*/)
        {
            /*_signalBus.Fire<PauseInputSignal>();
            for (int i = 0; i < signal.TurnsCount && !_turns.IsEmpty(); i++)
            {
                var turnController = _turns.Pop();
                var capturedPieceCell = turnController.PiecePlacedCell;
                await turnController.SelectedPiece.Replace(turnController.InitialCell, capturedPieceCell);
                if(turnController.CapturedPiece is not null)
                    await _captureBoardController.TakePiece(capturedPieceCell,turnController.CapturedPiece.Color);
                PiecesRepalced?.Invoke(turnController);
                turnController.Dispose();
            }
            _signalBus.Fire<PauseInputSignal>();*/
        }
        public Turn GetTurn()
        {
            return _turns.Peek();
        }
        
    }
}