using UnityEngine;

using Zenject;

namespace InputHandler
{
    public class PieceSelectionState : IState
    {
        private StateMachine _stateMachine;
        private Camera _mainCamera;
        private LayerMask _layerMask;
        private ISelection _selection;
        public PieceSelectionState(StateMachine stateMachine, Camera mainCamera, LayerMask layerMask, ISelection selection)
        {
            _stateMachine = stateMachine;
            _mainCamera = mainCamera;
            _layerMask = layerMask;
            _selection = selection;
        }
        public void Enter()
        {
            Debug.Log("Piece selection state");
        }
        public  void LogicUpdate()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            var selectable = _selection.CheckSelection(_layerMask, _mainCamera);
            if(selectable == null)
                return;
            selectable.OnSelected();
            _stateMachine.ChangeState(InputState.PieceSelected);
        }
        public  void Exit()
        { }
    }
}