using System;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace InputHandler
{
    public class InputHandler : ITickable,IInitializable,IDisposable
    {
        private readonly StateMachine _stateMachine;
        private readonly SignalBus _signalBus;
        public InputHandler(StateMachine stateMachine,SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
        }
        public void Initialize()
        {
            /*_signalBus.Subscribe<PauseInputSignal>(PauseInput);*/
        }
        
        public void Tick()
        {
            _stateMachine.CurrentState.LogicUpdate();
        }

        private void PauseInput()
        {
            var state = _stateMachine.CurrentState is PauseState ? InputState.PieceSelection : InputState.None;
            _stateMachine.ChangeState(state);
        }

        public void Dispose()
        {
            /*_signalBus.Unsubscribe<PauseInputSignal>(PauseInput);*/
        }
    }

    public enum InputState
    {
        PieceSelection,
        PieceSelected,
        OpponentsTurn,
        None
    }
}
