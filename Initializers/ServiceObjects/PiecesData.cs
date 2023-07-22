using System.Collections.Generic;
using ServiceObjects;
using UnityEngine;
using Views;

namespace Initializers.ServiceObjects
{
    [CreateAssetMenu(fileName = "PiecesData", menuName = "PiecesData", order = 0)]
    public class PiecesData : ScriptableObject
    {
        [HideInInspector]public List<PieceView> whitePieces3D;
        [HideInInspector]public List<PieceView> blackPieces3D;
        [HideInInspector] public List<PieceView> blackPieces2D;
        [HideInInspector] public List<PieceView> whitePieces2D;
        private void OnEnable()
        {
            var otherData = Resources.LoadAll<PiecesData>("test");
            if(otherData.Length > 1)
                Debug.LogError($"More than 1 asset of type {typeof(PiecesData)} founded");
        }
        public Dictionary<PieceType, PieceView> GetData(PieceColor color, Dimensions dimensions)
        {
            List<PieceView> pieces = null;
            if (dimensions == Dimensions.ThreeDimensional)
                pieces = color == PieceColor.Black ? blackPieces3D : whitePieces3D;
            else
                pieces = color == PieceColor.Black ? blackPieces2D : whitePieces2D;
            Dictionary<PieceType, PieceView> result = new Dictionary<PieceType, PieceView>();
            foreach (var piece in pieces)
            {
                result.Add(piece.Info.Type, piece);
            }
            return result; 
        }
    }
}