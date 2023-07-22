using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Editor.TaskLoader;
using NUnit.Framework;
using ServiceObjects;
using UnityEditor;
using UnityEngine;
using WithMVCS;
namespace Editor
{
      public class LvlEditor : EditorWindow
{
        private Dictionary<string, Texture> _pieces;
        private Texture[,] _textures;
        private Texture _defaultImage;
        private Dictionary<PieceType,List<(int,int)>>_whitePiecesPositions;
        private bool _showWhitePiecesDropdown = false;
        private bool _showBlackPiecesDropdown = false;
        private Dictionary<PieceType,List<(int,int)>> _blackPiecesPositions;
        private Dictionary<PieceType, List<(int, int)>> _selectedMapToAddPiece;
        private int[] _positionOptions;
        private string[] _positionOptionsText;
        private GUIStyle _headersStyle;
        private SaveManager _saveManager;
        private EditorTurnsIterator _editorTurnsIterator;
        //TasksLoader/Decoder
        private FromTextDataDecoder _decoder;
        private string _tasksPath = @"D:\ChessTasks\";
        private int _tasksFileNumber;
        private List<LvlTaskData> _tasksData;
        private int _taskIndex = 1;
        private bool _loadedDataSeted;
        //Player Turns
        private List<PreloadedTurn> _playerTurns;
        private (int, int) _pieceCellPosition;
        private (int, int) _pieceCellToPlace;
        //COM Turns
        private List<PreloadedTurn> _comTurns;
        private (int, int) _comPieceCellPosition;
        private (int, int) _comPieceCellToPlace;
        private Array _figuresNames;
        private string _lvlNumber;

        [MenuItem("Custom Editor/Game Editor")]
        public static void LvlEditorWindow()
        {
                GetWindowWithRect(typeof(LvlEditor), new Rect(new Vector2(0,0),new Vector2(700,700)), false, " Lvl Editor");   
        }
        void OnInspectorUpdate() { Repaint(); }

        private void OnEnable()
        {
                _positionOptions = new int[8] ;
                _positionOptionsText = new string[8];
                for (int i = 0; i < _positionOptions.Length; i++)
                {
                        _positionOptions[i] = i; 
                        _positionOptionsText[i] = i.ToString();
                }

                _figuresNames = Enum.GetValues(typeof(PieceType));
                
                _comTurns = new List<PreloadedTurn>();
                _playerTurns = new List<PreloadedTurn>();
                _whitePiecesPositions = InitializePiecesPositions();
                _blackPiecesPositions = InitializePiecesPositions();
                var font = Resources.Load("Fonts/Montserrat-Bold") as Font;
                _headersStyle = new GUIStyle() {font = font};
                _headersStyle.normal.textColor = Color.white;
                _defaultImage = Resources.Load("Editor/Pieces/square gray light _png_shadow_128px") as Texture;
                var piecesTextures = Resources.LoadAll<Texture>("Editor/Pieces");
                _pieces = new Dictionary<string, Texture>();
                for (int i = 0; i < piecesTextures.Length; i++)
                {
                        if(_pieces.ContainsKey(piecesTextures[i].name)) continue;
                        _pieces.Add(piecesTextures[i].name,piecesTextures[i]);
                }
                _textures = new Texture[8, 8];
                for (int i = 0; i < _textures.GetLength(0); i++)
                {
                        for (int j = 0; j < _textures.GetLength(1); j++)
                        {
                                _textures[i, j] = _defaultImage;
                        }
                }

                _decoder = new FromTextDataDecoder();
                _saveManager = new SaveManager();
        }

        private Dictionary<PieceType, List<(int,int)>> InitializePiecesPositions()
        {
                return new Dictionary<PieceType, List<(int, int)>>()
                {
                        {PieceType.Bishop, new List<(int, int)>(2){}},
                        {PieceType.King, new List<(int, int)>(1){}},
                        {PieceType.Knight, new List<(int, int)>(2){}},
                        {PieceType.Rook, new List<(int, int)>(2){}},
                        {PieceType.Queen, new List<(int, int)>(1){}},
                        {PieceType.Pawn, new List<(int, int)>(7){}}
                };
        }

        

        private void OnGUI()
        {
                DrawLeftSide();
        }

        #region Left Side

        private void DrawLeftSide()
        {
                GUILayout.BeginHorizontal(GUILayout.Width(250));
                var a = 'a';
                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Chess Board",_headersStyle);
                for (int i = 0; i < 8; i++)
                {
                        
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"{8 - i}({i})",GUILayout.Width(25),GUILayout.Height(25));
                        for (int j = 0; j < 8; j++)
                        {
                                EditorGUILayout.LabelField(new GUIContent(_textures[i,j]),GUILayout.Width(64),GUILayout.Height(64));
                        }
                        
                        GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                GUILayout.Space(32);
                for (int j = 0; j < 8; j++)
                {

                        EditorGUILayout.LabelField($"{a}({j})", GUILayout.Width(64),GUILayout.Height(64));
                        a++; 
                }
                GUILayout.EndHorizontal();
                DrawTurnsControlButtons();
                GUILayout.EndVertical();
                RightSide();
        }

        private void DrawTurnsControlButtons()
        {
                GUILayout.BeginHorizontal(); 
                PreviousTurnButton();
                GUILayout.Space(200);
                NextTurnButton();
                GUILayout.EndHorizontal();
        }
        private void PreviousTurnButton()
        {
                if(!GUILayout.Button("Previous turn",GUILayout.Width(200))) return;
                _editorTurnsIterator.PreviousTurn(_textures);
        }
        private void NextTurnButton()
        {
                if(!GUILayout.Button("Next turn",GUILayout.Width(200))) return;
                _editorTurnsIterator.NextTurn(_defaultImage,_textures,_pieces); 
        }

        #endregion

        #region Right Side
        private void RightSide()
        {
                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Pieces Positions", _headersStyle);
                DrawPiecesPositionSubWindow(ref _showWhitePiecesDropdown,_whitePiecesPositions,PieceColor.White);
                EditorGUILayout.EndFoldoutHeaderGroup();
                DrawPiecesPositionSubWindow(ref _showBlackPiecesDropdown,_blackPiecesPositions,PieceColor.Black);
                EditorGUILayout.EndFoldoutHeaderGroup();
                DrawPlayersTurns(_playerTurns, "Player");
                /*if(_playerTurns.Count <= _comTurns.Count)
                        DrawPlayersTurnSelection(_playerTurns,ref _pieceCellPosition,ref _pieceCellToPlace, "Player");*/
                DrawPlayersTurns(_comTurns,"COM");
                /*if(_playerTurns.Count > _comTurns.Count)
                        DrawPlayersTurnSelection(_comTurns, ref _comPieceCellPosition,ref _comPieceCellToPlace, "COM");*/
                GUILayout.Space(200);
                DrawTasksLoader();
                if (_tasksData != null && _taskIndex < _tasksData.Count)
                {
                        if(!_loadedDataSeted)
                        {
                                DrawPiecesAfterLoad(_tasksData[_taskIndex].PositionsData, PieceColor.White,
                                        ref _whitePiecesPositions);
                                DrawPiecesAfterLoad(_tasksData[_taskIndex].PositionsData, PieceColor.Black,
                                        ref _blackPiecesPositions);
                                SetTurns(ref _playerTurns, _tasksData[_taskIndex].TurnsData, PieceColor.White);
                                SetTurns(ref _comTurns, _tasksData[_taskIndex].TurnsData, PieceColor.Black);
                                if (_tasksData[_taskIndex].PlayerColor == PieceColor.Black)
                                        (_playerTurns, _comTurns) = (_comTurns, _playerTurns);
                                InitializeTurnsIterator(_tasksData[_taskIndex].PlayerColor,_tasksData[_taskIndex].TaskName);
                                _loadedDataSeted = true;
                        }
                        DrawSaveSkipButtons();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                
        }
 
        private void DrawSaveSkipButtons()
        {
                if (GUILayout.Button("Save Lvl"))
                {
                        Save();
                        TaskChecked();
                }
                if (!GUILayout.Button("Skip Task")) return;
                TaskChecked();
        }

        private void TaskChecked()
        {
                _taskIndex++;
                _loadedDataSeted = false;
                ClearTextures();  
        }
        private void DrawPiecesAfterLoad(Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>> data, PieceColor color,
                ref Dictionary<PieceType,List<(int,int)>> piecesPositions)
        {
                foreach (var piece in data[color])
                {
                        foreach (var position in piece.Value)
                        {
                                _textures[position.Item2, position.Item1] = _pieces[$"{color}{piece.Key}"];
                        }
                }
                piecesPositions = data[color];
        }

        private void InitializeTurnsIterator(PieceColor playerColor, string taskName)
        {
                if (_editorTurnsIterator is not null)
                {
                    _editorTurnsIterator.ReInitialize(_playerTurns,_comTurns,playerColor,taskName);    
                    return;
                }
                _editorTurnsIterator = new EditorTurnsIterator(_playerTurns,_comTurns,playerColor,taskName);
        }
        private void SetTurns(ref List<PreloadedTurn> turns, Dictionary<PieceColor,List<PreloadedTurn>> data,PieceColor color)
        {
                turns = data[color];
        }
        private void DrawTasksLoader()
        {
                EditorGUILayout.LabelField("Tasks Loader/Decoder :", _headersStyle);
                EditorGUILayout.LabelField("Default path : D:\\ChessTasks\\");
                _tasksPath =EditorGUILayout.TextField("Tasks data path",_tasksPath);
                _tasksFileNumber = EditorGUILayout.IntField("Tasks file number :",_tasksFileNumber);
                if(!GUILayout.Button("Load Tasks")) return;
                var path = $"{_tasksPath + _tasksFileNumber}.txt";
                Debug.Log($"File Loaded by path : {_tasksPath + _tasksFileNumber}.txt");
                _tasksData = _decoder.ReadLvlTasksWithMark(path, 10);
                if (_tasksData is null)
                        throw new Exception("File not found");
                _taskIndex = 0;
        }
        private void PiecesPositionsDropdown(Rect rect)
        {
                var menu = new GenericMenu();
                foreach (PieceType val in _figuresNames)
                {
                        AddMenuItem(menu, $"AddPiece/{val}",val);
                }
                menu.DropDown(rect);
        }
        private void AddMenuItem(GenericMenu menu, string menuPath, PieceType piece)
        {
                menu.AddItem(new GUIContent(menuPath),false,ChangePiece,piece);
        }

        private void ChangePiece(object piece)
        {
                var pieceName = (PieceType) piece;
                if(_selectedMapToAddPiece.ContainsKey(pieceName))
                {
                        if (_selectedMapToAddPiece[pieceName].Count > _selectedMapToAddPiece[pieceName].Capacity - 1) return;
                        _selectedMapToAddPiece[pieceName].Add(default);
                        return;
                }
                _selectedMapToAddPiece.Add((PieceType)piece,new List<(int, int)>());
        }
        private void DrawPiecesPositionSubWindow(ref bool showPieceWindowPos, Dictionary<PieceType,List<(int,int)>> piecesPositions, PieceColor color)
        {
                showPieceWindowPos = EditorGUILayout.BeginFoldoutHeaderGroup(showPieceWindowPos, $"{color} Pieces Positions");
                if (!showPieceWindowPos) return;
                
                foreach (var figurePos in piecesPositions)
                {
                        for (int i = 0; i < figurePos.Value.Count; i++)
                        {
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(figurePos.Key.ToString(),GUILayout.Width(50));
                                var item1 = EditorGUILayout.IntPopup(figurePos.Value[i].Item1, _positionOptionsText, _positionOptions,GUILayout.Width(30));
                                var item2 = EditorGUILayout.IntPopup(figurePos.Value[i].Item2, _positionOptionsText, _positionOptions,GUILayout.Width(30));
                                figurePos.Value[i] = (item1, item2);
                                SetPiecePosition(figurePos.Key,figurePos.Value[i],color);
                                GUILayout.EndHorizontal();
                        }
                }
                if (!GUILayout.Button("Add Piece")) return;
                _selectedMapToAddPiece = piecesPositions;
                var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                PiecesPositionsDropdown(new Rect(mousePos.x - 150f, mousePos.y - 100, 200f, 24f));
        }

        private void DrawPlayersTurns(List<PreloadedTurn> turns, string name)
        {
                EditorGUILayout.LabelField($"{name} Turns", _headersStyle);
                GUILayout.BeginHorizontal();
                for (int i = 0; i < turns.Count; i++)
                {
                        if (i % 2 == 0)
                        {
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField($"Turn {i + 1}: {turns[i].piece}, C2: {turns[i]._selectedCellPosition} |", GUILayout.Width(180));
                                continue;
                        }
                        EditorGUILayout.LabelField($"Turn {i + 1}: C1 :{turns[i].piece}, C2: {turns[i]._selectedCellPosition} |", GUILayout.Width(180));
                }
                GUILayout.EndHorizontal(); 
        }
        
       
        private void SetPiecePosition(PieceType pieceName,(int,int) position, PieceColor color)
        {
                if(!GUILayout.Button("Set Position",GUILayout.Width(80)))return;
                var texture = _pieces[$"{color}{pieceName}"];
                _textures[position.Item1, position.Item2] = texture;
        }

        private void Save()
        {
                var existedLvl = Resources.Load($"Data/{_tasksData[_taskIndex].TaskNumber}"); 
                if (existedLvl != null)
                        if (!EditorUtility.DisplayDialog("Warning!", "The level you are trying to create already exists",
                                    "Create anyway", "Cancel"))return;
                LvlData lvlData = new LvlData(_playerTurns,_comTurns,$"{ _tasksData[_taskIndex].TaskNumber}",
                        _whitePiecesPositions,_blackPiecesPositions,_tasksData[_taskIndex].PlayerColor);
                _saveManager.Save(lvlData,$"/{_tasksData[_taskIndex].TaskNumber}.txt");
               AssetDatabase.Refresh();
        }

        private void ClearTextures()
        {
                for (int i = 0; i < _textures.GetLength(0); i++)
                {
                        for (int j = 0; j < _textures.GetLength(1); j++)
                        {
                                _textures[i, j] = _defaultImage;
                        }
                }
        }
        #endregion  
}

}

#region Trash

/*private void DrawPlayersTurnSelection(List<PreloadedTurn> playerTurns,ref (int,int) initCell, ref (int,int) selectedCell, string name)
        {
                EditorGUILayout.LabelField($"{name} Turn Selection",_headersStyle);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Piece Cell Position (x,y)", GUILayout.Width(150));
                GUILayout.BeginHorizontal();
                initCell.Item1 =
                        EditorGUILayout.IntPopup(initCell.Item1, _positionOptionsText, _positionOptions);
                initCell.Item2 =
                        EditorGUILayout.IntPopup(initCell.Item2, _positionOptionsText, _positionOptions); 
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.Space(20);
                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Cell position to place", GUILayout.Width(150));
                GUILayout.BeginHorizontal();
                selectedCell.Item1 =
                        EditorGUILayout.IntPopup(selectedCell.Item1, _positionOptionsText, _positionOptions);
                selectedCell.Item2 =
                        EditorGUILayout.IntPopup(selectedCell.Item2, _positionOptionsText, _positionOptions);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                if (initCell == default && selectedCell == default) return;
                if(!GUILayout.Button("Add Turn")) return;
                playerTurns.Add(new PreloadedTurn(initCell,selectedCell, null));
                initCell = default;
                selectedCell = default;
        }*/
/* private (FigureName,(int,int)) DrawAddPieceButton(KeyValuePair<FigureName,List<(int,int)>> figurePos)
        {
                if (GUILayout.Button("Add Piece"))
                        return (figurePos.Key, (1, 1));
                return default; 
                
        }*/

#endregion

