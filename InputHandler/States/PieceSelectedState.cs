using UnityEngine;
using Zenject;

namespace InputHandler
{
    public class PieceSelectedState : IState
    {
        private StateMachine _stateMachine;
        private Camera _mainCamera;
        private LayerMask _layerMask;
        private ISelection _selection;
        private SignalBus _signalBus;
        public PieceSelectedState(StateMachine stateMachine,Camera mainCamera, LayerMask layerMask,ISelection selection,SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _mainCamera = mainCamera;
            _layerMask = layerMask;
            _selection = selection;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            Debug.Log("Cell selection state");
        }

        public  void LogicUpdate()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            var selectable = _selection.CheckSelection(_layerMask, _mainCamera);
            if (selectable == null)
            {
                return;
                /*_signalBus.Fire<CurrentTurnUndoSignal>();
                _stateMachine.ChangeState(InputState.PieceSelection);
                return;   */
            }
            selectable.OnSelected();
            _stateMachine.ChangeState(InputState.PieceSelection);
        }
        public  void Exit()
        { }
    }
}