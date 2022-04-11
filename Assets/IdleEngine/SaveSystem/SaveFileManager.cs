using System.IO;
using UnityEngine;

namespace IdleEngine.SaveSystem
{
  public static class SaveFileManager
  {
    public static readonly string SavePath = Path.Combine(Application.persistentDataPath, "Saves");

    private static void EnsureSaveFolder()
    {
      if (!Directory.Exists(SavePath))
      {
        Directory.CreateDirectory(SavePath);
      }
    }

    public static void Write(string filename, string content)
    {
      EnsureSaveFolder();

      var path = Path.Combine(SavePath, filename);
      File.WriteAllText(path, content);
    }

    public static bool TryLoad(string filename, out string content)
    {
      content = string.Empty;

      var path = Path.Combine(SavePath, filename);

      if (!File.Exists(path))
      {
        return false;
      }

      content = File.ReadAllText(path);
      return true;
    }

    public static void DeleteSaveFiles()
    {
      Directory.Delete(SavePath, true);
    }
  }
}
