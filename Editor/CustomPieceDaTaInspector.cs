using System.Collections.Generic;
using System.Linq;
using Initializers.ServiceObjects;
using ServiceObjects;
using UnityEditor;
using UnityEngine;
using Views;


namespace Editor
{
    [CustomEditor(typeof(PiecesData))]
    public class CustomPieceDataInspector : UnityEditor.Editor
    {
        private bool _showBlackPieces = false;
        private bool _showWhitePieces = false;
        private PiecesData _data;
    
        private void OnEnable()
        {
            _data = (PiecesData) target;
            LoadPieces(ref _data.blackPieces3D,PieceColor.Black,3);
            LoadPieces(ref _data.whitePieces3D,PieceColor.White,3); 
            LoadPieces(ref  _data.blackPieces2D,PieceColor.Black,2);
            LoadPieces(ref _data.whitePieces2D,PieceColor.White,2);
        } 

        private void LoadPieces(ref List<PieceView> pieces, PieceColor color,int dimension)
        {
            pieces ??= Resources.LoadAll<PieceView>($"Prefabs/{dimension}D/{color}").ToList(); 
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("GO Pieces Data");
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("3D objects");
            ShowPiecesFoldOut(ref _showBlackPieces,_data.blackPieces3D,PieceColor.Black);
            EditorGUILayout.EndFoldoutHeaderGroup();
            ShowPiecesFoldOut(ref _showWhitePieces,_data.whitePieces3D, PieceColor.White);
            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.Space(50);
            EditorGUILayout.LabelField("2D objects");
            ShowPiecesFoldOut(ref _showBlackPieces,_data.blackPieces2D,PieceColor.Black);
            EditorGUILayout.EndFoldoutHeaderGroup();
            ShowPiecesFoldOut(ref _showWhitePieces,_data.whitePieces2D, PieceColor.White);
            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        private void ShowPiecesFoldOut(ref bool show, List<PieceView> pieces, PieceColor color)
        {
            show = EditorGUILayout.BeginFoldoutHeaderGroup(show, $"{color} Pieces");
            if(!show)return;
            foreach (var piece in pieces)
            {
                EditorGUILayout.LabelField($"{piece.Info.Type}",$"{piece.name}");
            }
        }
    }
}