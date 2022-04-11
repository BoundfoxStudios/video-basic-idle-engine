using System.IO;
using IdleEngine.SaveSystem;
using UnityEditor;
using UnityEngine;

namespace IdleEngine.Editor
{
  public static class SaveFileManagerMenuItems
  {
    [MenuItem("Idle Game/Delete saves")]
    public static void DeleteSaves()
    {
      SaveFileManager.DeleteSaveFiles();
      Debug.Log("Save files deleted.");
    }

    [MenuItem("Idle Game/Show Save folder")]
    public static void ShowSaveFolder()
    {
      if (Directory.Exists(SaveFileManager.SavePath))
      {
        EditorUtility.RevealInFinder(SaveFileManager.SavePath);
        return;
      }
      
      EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
  }
}
