using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Chess_AI
{
    public class MinMaxSystem
    {
        private const int MaxScore = 99999;
        private const int MinScore = -99999;
        private AITurn.Pool _turnsPool;
        public MinMaxSystem(AITurn.Pool pool)
        {
            _turnsPool = pool;
        }
        public AITurn MinMaxRoot(int depth, AIChessBoard board, bool isMinMaxingPlayer)
        {
            var sw = new Stopwatch();
            sw.Start();
            var turns = board.GetAllPossibleTurns(!isMinMaxingPlayer);
            float bestScore = MinScore;
            AITurn bestTurn = default;
            for (var i = 0; i < turns.Count; i++)
            {
                var turn = turns[i];
                board.SetTurn(turn);
                var value = MinMax(depth - 1, board, MinScore, MaxScore, !isMinMaxingPlayer);
                board.Undo(turn);
                if (!(value >= bestScore))
                    continue;
                bestScore = value;
                bestTurn = turn;
                turns.RemoveAt(i);
            }
            _turnsPool.DespawnAll(turns);
            sw.Stop(); 
            UnityEngine.Debug.Log(sw.Elapsed);
            return bestTurn;
        }

        private float MinMax(int depth, AIChessBoard board, float alpha, float beta, bool isMinMaxingPlayer)
        {
            if (depth == 0)
                return -board.EvaluateBoard();
            var turns = board.GetAllPossibleTurns(!isMinMaxingPlayer);
            float bestScore = isMinMaxingPlayer ? MinScore : MaxScore;
            foreach (var turn in turns)
            {
                board.SetTurn(turn);
                bestScore = isMinMaxingPlayer 
                    ? Math.Max(bestScore, MinMax(depth - 1, board, alpha, beta, !isMinMaxingPlayer))
                    : Math.Min(bestScore, MinMax(depth - 1, board, alpha, beta, !isMinMaxingPlayer)); 
                board.Undo(turn);
                if (isMinMaxingPlayer)
                {
                    alpha = Math.Max(alpha, bestScore);
                    if (beta <= alpha)
                        break;
                }
                else
                {
                    beta = Math.Min(beta, bestScore);
                    if (beta <= alpha)
                        break;
                }
            }
            _turnsPool.DespawnAll(turns);
            return bestScore;
        }
        /*private float MinMax(int depth, AIChessBoard board, float alpha, float beta, bool isMinMaxingPlayer)
        {
            
            if (depth == 0)
                return -board.EvaluateBoard();
            var turns = board.GetAllPossibleTurns(!isMinMaxingPlayer);
            if (isMinMaxingPlayer)
            {
                float bestScore = MinScore;
                foreach (var turn in turns)
                {
                    board.SetTurn(turn);
                    bestScore = Math.Max(bestScore, MinMax(depth - 1, board, alpha, beta, !isMinMaxingPlayer)); 
                    board.Undo(turn);
                    alpha = Math.Max(alpha, bestScore);
                    if (!(beta <= alpha)) continue;
                    _turnsPool.DespawnAll(turns);
                    return bestScore; 
                }
                _turnsPool.DespawnAll(turns);
                return bestScore;
            }
            else
            {
                float bestScore = MaxScore;
                foreach (var turn in turns)
                {
                    board.SetTurn(turn);
                    bestScore = Math.Min(bestScore, MinMax(depth - 1, board, alpha, beta, !isMinMaxingPlayer)); 
                    board.Undo(turn);
                    alpha = Math.Min(beta, bestScore);
                    if (!(beta <= alpha)) continue;
                    _turnsPool.DespawnAll(turns);
                    return bestScore;
                }
                _turnsPool.DespawnAll(turns);
                return bestScore;
            }
        }*/
    }
}