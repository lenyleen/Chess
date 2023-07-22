using System;
using System.Collections.Generic;
using Chess_AI.AIPiecesMoves;
using GameLogic;
using GameLogic.Signals;
using ServiceObjects;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Chess_AI
{
    public class AIChessBoard : IInitializable,IDisposable
    {
        private AIPiece[][] _piecesBoard;
        private List<AIPiece> _movableAIPieces;
        private List<AIPiece> _movablePlayerPieces;
        private PieceColor _aiPiecesColor;
        private MinMaxSystem _minMaxSystem;
        private AITurn.Pool _turnsPool;
        private AIPiece.Factory _pieceFactory;
        private SignalBus _signalBus;
        private readonly Dictionary<PieceColor, Dictionary<PieceType, float[][]>> _piecesEvaluation = new Dictionary<PieceColor, Dictionary<PieceType, float[][]>>()
        {
            {PieceColor.White,new Dictionary<PieceType, float[][]>()
                {
                    {PieceType.Pawn,WhitePiecesBoardEvaluation.PawnEval},
                    {PieceType.Knight,WhitePiecesBoardEvaluation.KnightEval},
                    {PieceType.Bishop,WhitePiecesBoardEvaluation.BishopEval},
                    {PieceType.Rook,WhitePiecesBoardEvaluation.RookEval},
                    {PieceType.Queen,WhitePiecesBoardEvaluation.QueenEval},
                    {PieceType.King,WhitePiecesBoardEvaluation.KingEval}
                }
            },
            {PieceColor.Black,new Dictionary<PieceType, float[][]>()
            {
                {PieceType.Pawn,BlackPiecesBoardEvaluation.PawnEval},
                {PieceType.Knight,BlackPiecesBoardEvaluation.KnightEval},
                {PieceType.Rook,BlackPiecesBoardEvaluation.RookEval},
                {PieceType.Bishop,BlackPiecesBoardEvaluation.BishopEval},
                {PieceType.Queen,BlackPiecesBoardEvaluation.QueenEval},
                {PieceType.King,BlackPiecesBoardEvaluation.KingEval}
            }}
        };
        private Dictionary<PieceType, IAIMove> _aiPiecesMoves = new Dictionary<PieceType, IAIMove>()
        {
            {PieceType.Pawn, new PawnMoves()},
            {PieceType.Rook, new RookMoves()},
            {PieceType.Knight, new KnightMoves()},
            {PieceType.Bishop, new BishopMoves()},
            {PieceType.Queen, new QueenMoves()},
            {PieceType.King, new KingMoves()}
        };
        Dictionary<PieceType, int> _values = new Dictionary<PieceType, int>()
        {
            {PieceType.Pawn, 10},
            {PieceType.Rook, 50},
            {PieceType.Knight, 30},
            {PieceType.Bishop, 30},
            {PieceType.Queen, 90},
            {PieceType.King, 900}
        };

        public AIChessBoard(LvlData data, PieceColor aiPiecesColor, AIPiece[][] pieces, MinMaxSystem minMaxSystem,
            AITurn.Pool turnsPool, AIPiece.Factory pieceFactory, SignalBus signalBus)
        {
            _minMaxSystem = minMaxSystem;
            _piecesBoard = pieces;
            _aiPiecesColor = aiPiecesColor;
            _turnsPool = turnsPool;
            _movableAIPieces = new List<AIPiece>();
            _movablePlayerPieces = new List<AIPiece>();
            _pieceFactory = pieceFactory;
            SetBoard(data.whiteFiguresPositions, PieceColor.White,_values);
            SetBoard(data.blackFiguresPositions, PieceColor.Black,_values);
            _signalBus = signalBus;
        }
        public void Initialize()
        {
            _signalBus.Subscribe<TurnEndedSignal>(SetTurn);
        }

        public AITurn GetBestTurn(int depth)
        {
            var turn = _minMaxSystem.MinMaxRoot(depth, this, _aiPiecesColor == PieceColor.Black);
            Debug.Log(_turnsPool.NumTotal);
            return turn;
        }

        private void SetBoard(Dictionary<PieceType, List<(int, int)>> positionByName, PieceColor color, Dictionary<PieceType, int> values)
        {
            foreach (var piece in positionByName)
            {
                foreach (var position in piece.Value)
                {
                    var newPiece = _pieceFactory.Create(piece.Key, values[piece.Key], color,(position.Item2,position.Item1));
                    _piecesBoard[position.Item2][position.Item1] = newPiece;
                    if (color != _aiPiecesColor)
                        _movablePlayerPieces.Add(newPiece);
                    else
                        _movableAIPieces.Add(newPiece);
                }
            }
        }

        public List<AITurn> GetAllPossibleTurns(bool isPlayerMoves)
        {
            var possibleTurns = new List<AITurn>(72);
            var movablePieces = isPlayerMoves ? _movablePlayerPieces : _movableAIPieces;
            foreach (var piece in movablePieces)
            {
                if(piece.Captured)
                    continue;
                var newTurns = _aiPiecesMoves[piece.PieceType].GetPossibleMoves(_piecesBoard,piece.Color,piece.Position,_turnsPool);
                if(newTurns.Count == 0) continue;
                possibleTurns.AddRange(newTurns);
            }
            return possibleTurns;
        }
        public void SetTurn(AITurn move)
        {
            if(move.Name == PieceType.None) return;
            var piece = _piecesBoard[move.Position.Item1][move.Position.Item2];
            piece.Position = move.ToPlacePosition;
            if (move.CapturedPiece is not null)
                move.CapturedPiece.Captured = true;
            (_piecesBoard[move.Position.Item1][move.Position.Item2],
                    _piecesBoard[move.ToPlacePosition.Item1][move.ToPlacePosition.Item2])
                = (null, _piecesBoard[move.Position.Item1][move.Position.Item2]);
        }

        public void SetTurn(TurnEndedSignal signal)
        {
            var position = signal.TurnInfo.InitialCellPosition;
            var toPlacePosition = signal.TurnInfo.SelectedCellPosition;
            var piece = _piecesBoard[position.Item1][position.Item2];
            piece.Position = toPlacePosition;
            var capturedPiece = _piecesBoard[toPlacePosition.Item1][toPlacePosition.Item2];
            if (capturedPiece is not null)
                capturedPiece.Captured = true;
            (_piecesBoard[position.Item1][position.Item2],
                    _piecesBoard[toPlacePosition.Item1][toPlacePosition.Item2])
                = (null, _piecesBoard[position.Item1][position.Item2]);
        }
        public void ReturnCapturedPiece(PieceType pieceName, PieceColor color, (int, int) position)
        {
            var piece = _pieceFactory.Create(pieceName, _values[pieceName], color, position);
            _piecesBoard[position.Item1][position.Item2] = piece;
        }
        private float GetPieceValue(int row, int column)
        {
            var piece = _piecesBoard[row][column];
            if (piece is null)
                return 0.0f;
            var value = piece.Value + _piecesEvaluation[piece.Color][piece.PieceType][row][column];
            return piece.Color == PieceColor.White ? value : -value;
        }
        public float EvaluateBoard()
        {
            var totalScore = 0f;
            var length = _piecesBoard.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    totalScore += GetPieceValue(i, j);
                }
            }
            return totalScore;
        }

        public void Undo(AITurn move)
        {
            var piece = _piecesBoard[move.ToPlacePosition.Item1][move.ToPlacePosition.Item2];
            piece.Position = move.Position;
            if (move.CapturedPiece is not null)
                move.CapturedPiece.Captured = false;
            (_piecesBoard[move.Position.Item1][move.Position.Item2],
                    _piecesBoard[move.ToPlacePosition.Item1][move.ToPlacePosition.Item2])
                = (_piecesBoard[move.ToPlacePosition.Item1][move.ToPlacePosition.Item2],move.CapturedPiece);
        }
        public void Dispose()
        {
            _signalBus.Unsubscribe<TurnEndedSignal>(SetTurn);
        }
    }
}