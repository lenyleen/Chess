using System.IO;
using Newtonsoft.Json;
using Application = UnityEngine.Device.Application;
namespace ServiceObjects
{
  public class SaveManager
  {
    private readonly string dataPath;

    public SaveManager()
    {
      dataPath = Application.dataPath + "/Resources/Data"; 
    }

    public void Save(LvlData data, string name)
    {
      if (!Directory.Exists(dataPath))
      {
        Directory.CreateDirectory(dataPath);
      }
      var json = JsonConvert.SerializeObject(data);
      File.WriteAllText(dataPath + name, json);
    }

    public LvlData Load(string name)
    {
      var json = File.ReadAllText(dataPath + "/LvlData/"+name +".txt");
      var data = JsonConvert.DeserializeObject<LvlData>(json);
      return data;
    }
  }
}
