using System.Collections.Generic;
using ServiceObjects;
using WithMVCS;
namespace Editor
{
  public class LvlTaskData
  {
    public string TaskName { get; private set; }
    public int TaskNumber { get; private set; }
    public Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>> PositionsData { get; private set; }
    public Dictionary<PieceColor, List<PreloadedTurn>> TurnsData { get; private set; }
    public PieceColor PlayerColor;

    public LvlTaskData(string taskName,int taskNumber, 
      Dictionary<PieceColor, Dictionary<PieceType, List<(int, int)>>> positionsData, Dictionary<PieceColor, List<PreloadedTurn>> turnsData, PieceColor playerColor)
    {
      TaskName = taskName;
      TaskNumber = taskNumber;
      PositionsData = positionsData;
      TurnsData = turnsData;
      PlayerColor = playerColor;
    }
  }
}

