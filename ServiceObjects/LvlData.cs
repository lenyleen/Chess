using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WithMVCS;
namespace ServiceObjects
{
  [System.Serializable]
  public class LvlData
  {
    public PieceColor playerType;
    public List<PreloadedTurn>  playerTurns;
    public List<PreloadedTurn> COMturns;
    public string name;
    public Dictionary<PieceType,List<(int,int)>> whiteFiguresPositions; 
    public Dictionary<PieceType,List<(int,int)>> blackFiguresPositions;
    public LvlData(List<PreloadedTurn> playerTurns, List<PreloadedTurn> COMturns, string name, Dictionary<PieceType,List<(int,int)>> whiteFiguresPositions, 
      Dictionary<PieceType,List<(int,int)>> blackFiguresPositions, PieceColor playerType)
    {
      this.playerTurns = playerTurns;
      this.COMturns = COMturns;
      this.name = name;
      this.whiteFiguresPositions = whiteFiguresPositions;
      this.blackFiguresPositions = blackFiguresPositions;
      this.playerType = playerType;
    }
  }
}
        
