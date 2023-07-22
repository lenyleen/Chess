using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.TaskLoader.CorrectMoveCheckers;
using NUnit.Framework;
using ServiceObjects;
using UnityEngine;
using WithMVCS;
namespace Editor.TaskLoader
{
     public class FromTextDataDecoder
{
        private Dictionary<char, PieceType> figures = new Dictionary<char, PieceType>()
        {
                {'P', PieceType.Pawn},
                {'N', PieceType.Knight},
                {'Q', PieceType.Queen},
                {'B', PieceType.Bishop},
                {'K', PieceType.King},
                {'R', PieceType.Rook}
        };

        private Dictionary<PieceType, IMoveCorrectnessChecker> movesCheckers =
                new Dictionary<PieceType, IMoveCorrectnessChecker>()
                {
                        {PieceType.Pawn, new PawnChecker()},
                        {PieceType.Knight, new KnightChecker()},
                        {PieceType.Queen, new QueenChecker()},
                        {PieceType.King, new KingChecker()},
                        {PieceType.Rook, new RookChecker()},
                        {PieceType.Bishop,new BishopChecker()}
                };

        private PieceColor[] _playerColor;
        /// <summary>
        /// Loads tasks from .txt file from disk and marks count of loaded tasks
        /// </summary>
        /// <param name="path"> Path to file with tasks </param>
        /// <param name="tasksCount"> Number of tasks to load and mark</param>
        /// <returns></returns>
        public List<LvlTaskData> ReadLvlTasksWithMark(string path, int tasksCount)
        {
                path = @"D:\ChessTasks\1.txt";
                List<string[]> data = new List<string[]>();
                
                using (StreamReader sr = new StreamReader(path))
                {
                        var loadedTasksCount = sr.ReadLine();
                        var loadedTasksCountNum = 0;
                        if(!string.IsNullOrEmpty(loadedTasksCount)) 
                                loadedTasksCountNum = int.Parse(loadedTasksCount!);
                        var i = 0;
                        while (i < loadedTasksCountNum * 3)
                        {
                                var line = sr.ReadLine();
                                if(line == "") continue;
                                i++;
                        }
                        i = 0;
                        while (data.Count < tasksCount)
                        {
                                var line = sr.ReadLine();
                                if(line == "") continue;
                                data.Add(new string[3]);
                                data[i][0] = line;
                                for (int j = 1; j < 3; j++)
                                {
                                        line = sr.ReadLine();
                                        data[i][j] = line;
                                }
                                i++;
                        }
                }

                string text = null;
                var currenNumberOfLoadedTasks = 0;
                using (StreamReader sm = new StreamReader(path))
                {
                        var num = sm.ReadLine();
                        try
                        {
                                currenNumberOfLoadedTasks = int.Parse(num);
                        }
                        catch (Exception e)
                        {
                                Debug.LogWarning($"Cant read number of current loaded tasks, check file: {path}");
                                throw;
                        }

                        currenNumberOfLoadedTasks += tasksCount;
                        text = sm.ReadToEnd();
                }
                using (StreamWriter sw  = new StreamWriter(path,false))
                {
                        var updatedText = $"{currenNumberOfLoadedTasks}\n" + text; 
                        sw.Write(updatedText);  
                }
                
                _playerColor = new PieceColor[tasksCount];
                var result = new List<LvlTaskData>();
                var positionsData = SetPositionsData(data);
                var turnsData = SetTurnsData(data, positionsData);
                for (int i = 0; i < data.Count; i++)
                {
                        var taskNumber = (currenNumberOfLoadedTasks + 1) - (data.Count - i);
                        var newTaskData = new LvlTaskData(data[i][0], taskNumber, positionsData[i], turnsData[i],_playerColor[i]);
                        result.Add(newTaskData);
                }

                return result;
        }
        private List<Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>>> SetPositionsData(List<string[]> data)
        {
                var positionsData = new List<Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>>>();
                for (int i = 0; i < data.Count; i++)
                {
                        if(data[i][1] == null) break;
                        var positions = data[i][1];
                        positionsData.Add(DecryptPiecePositions(positions));
                        _playerColor[i] = SetPlayerColor(data[i][1]);
                }
                return positionsData;
        }

        private PieceColor SetPlayerColor(string data)
        {
                for (int i = 0; i < data.Length; i++)
                {
                        if (char.IsWhiteSpace(data[i]))
                                return data[i + 1] == 'w' ? PieceColor.White : PieceColor.Black;
                }
                return PieceColor.White;
        }

        private Dictionary<PieceColor,Dictionary<PieceType,List<(int,int)>>> DecryptPiecePositions(string positions)
        {
                var result = new Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>>
                {
                        {PieceColor.Black, new Dictionary<PieceType, List<(int, int)>>()},
                        {PieceColor.White, new Dictionary<PieceType, List<(int, int)>>()}
                };
                var verticalPosition = 0;
                var horizontalPosition = 0; 
                for (int j = 0; j < positions.Length; j++, horizontalPosition++)
                {
                        if(positions[j] == '/')
                        {
                                verticalPosition++;
                                horizontalPosition = -1;
                                continue;
                        }
                        if (char.IsNumber(positions[j]))
                        {
                                var num = positions[j] - '1';
                                horizontalPosition += num;
                                continue;
                        }
                        var color = char.IsLower(positions[j]) ? PieceColor.Black : PieceColor.White;
                        var upperCh = char.ToUpper(positions[j]);
                        if(!figures.ContainsKey(upperCh)) break;
                        var figureName = figures[upperCh];
                        if(!result[color].ContainsKey(figureName))
                                result[color].Add(figureName,new List<(int, int)>());
                        result[color][figureName].Add((horizontalPosition,verticalPosition));
                }
                return result;
        }

        private List<Dictionary<PieceColor, List<PreloadedTurn>>> SetTurnsData(List<string[]> data, 
                List<Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>>> positionsData)
        {
                var turnsData = new List<Dictionary<PieceColor, List<PreloadedTurn>>>();
                for (int i = 0; i < data.Count(); i++)
                {
                        var turns = data[i][2].Split(' ');
                        turnsData.Add(TurnsDecryption(turns, i,positionsData,_playerColor[i])); 
                }

                return turnsData;
        }
        private Dictionary<PieceColor, List<PreloadedTurn>> TurnsDecryption(string[] turns, int turnIndex,
                List<Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>>> positionsData,PieceColor color)
        {
                Dictionary<PieceColor, List<PreloadedTurn>> turnsData =
                        new Dictionary<PieceColor, List<PreloadedTurn>>();
                Dictionary<PieceColor, List<(int, int)>> piecesChangedPositions =
                        new Dictionary<PieceColor, List<(int, int)>>()
                        {
                                {PieceColor.White,new List<(int, int)>()}, 
                                {PieceColor.Black,new List<(int, int)>()}
                        };
                PieceType pieceName = default;
                var horizontalPos = 0; 
                var verticalPos = 0;
                List<(int, int)> piecesPositions = new List<(int, int)>();
                for (int i = 0; i < turns.Length; i++) 
                {
                        if(turns[i] == "") continue;
                        turns[i] = CheckForWithoutWhiteSpaceTurn(turns[i]);
                        if(char.IsNumber(turns[i][0]))continue;
                        var dataIndex = 0;
                        turns[i] = CheckForImpostorPawn(turns[i]);
                        for (int j = 0; j < turns[i].Length; j++) 
                        {
                                if(turns[i][j] == 'x')
                                {
                                        continue;
                                }
                                switch (dataIndex)
                                {
                                        case 0:
                                                var figureCh = !char.IsLower(turns[i][j])
                                                        ? turns[i][j]
                                                        : 'P';
                                                pieceName = figures[figureCh];
                                                piecesPositions = positionsData[turnIndex][color][pieceName];
                                                dataIndex++;
                                                break;
                                        case 1 :
                                                horizontalPos =  turns[i][j] - 'a';
                                                dataIndex++;
                                                break;
                                        case 2 :
                                                verticalPos = '8' - turns[i][j]; 
                                                dataIndex++;
                                                break;
                                }
                        }
                        if(!turnsData.ContainsKey(color))
                                turnsData.Add(color, new List<PreloadedTurn>());
                        var piecePosition = CheckTurn((horizontalPos, verticalPos), piecesPositions,piecesChangedPositions[color],pieceName, color, new DebugData(pieceName,turnIndex));
                        PreloadedTurn newTurn = new PreloadedTurn(default, (horizontalPos,verticalPos),piecePosition,pieceName);
                        piecesChangedPositions[color].Add((horizontalPos,verticalPos));
                        turnsData[color].Add(newTurn);
                        color = color == PieceColor.Black ? PieceColor.White : PieceColor.Black;
                }

                return turnsData;
        }

        private string CheckForWithoutWhiteSpaceTurn(string turn)
        {
                if (char.IsLetter(turn[0])) return turn;
                var letterStartsIndex = 0;
                for (int i = 0; i < turn.Length; i++)
                {
                        if (!char.IsLetter(turn[i])) continue;
                        letterStartsIndex = i;
                        break;
                }

                return turn[letterStartsIndex..];
        }
        private string CheckForImpostorPawn(string turn)
        {
                if (char.IsLetter(turn[1])) return turn;
                return 'P' + turn;
        }
                
        private List<(int,int)> CheckTurn((int,int) selectedCell, List<(int,int)> piecesPositions, List<(int,int)> piecesChangedPositions
                , PieceType name, PieceColor color, DebugData debugData)
        {
                var result = new List<(int, int)>();
                var checker = movesCheckers[name];
                if (piecesPositions.Count == 1) return new List<(int,int)>(){piecesPositions[0]};
                foreach (var position in piecesPositions)
                { 
                        if (checker.CheckPieceToMove(selectedCell, position, color))
                                result.Add(position);
                }
                foreach (var position in piecesChangedPositions)
                {
                        if (checker.CheckPieceToMove(selectedCell, position, color))
                                result.Add(position);   
                }
                if (result.Count <= 0) 
                        throw new Exception($"Piece not found at any position, please, check the task: {debugData.name}, {debugData.turnIndex}"); 
                return result;
        }
        public class DebugData
        {
                public PieceType name;
                public int turnIndex;

                public DebugData(PieceType name, int turnIndex)
                {
                        this.name = name;
                        this.turnIndex = turnIndex;
                }
        }
        
}   
}

