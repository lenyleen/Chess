using System.Collections.Generic;
using Zenject;

namespace InputHandler
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        private List<IState> _states;
        [Inject]
        public void Construct(PieceSelectionState pieceSelection, PieceSelectedState pieceSelected, OpponentTurnState opponentTurn, PauseState pause)
        {
            _states = new List<IState>(){pieceSelection,pieceSelected,opponentTurn,pause};
            var currentEnumState = InputState.PieceSelection;
            CurrentState = _states[(int)currentEnumState];
        }
        public void ChangeState(InputState newState)
        {
            CurrentState.Exit();
            CurrentState = _states[(int)newState];
            CurrentState.Enter();
        }
    }
}