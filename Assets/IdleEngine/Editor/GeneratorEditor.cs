using IdleEngine.Generators;
using UnityEditor;

namespace IdleEngine.Editor
{
  [CustomEditor(typeof(Generator))]
  public class GeneratorEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      var generator = (Generator) target;
      
      EditorGUILayout.LabelField(nameof(Generator.NextBuildingCostsForOne), generator.NextBuildingCostsForOne.ToString());
      EditorGUILayout.LabelField(nameof(Generator.Owned), generator.Owned.ToString());
      EditorGUILayout.LabelField(nameof(Generator.ProductionCycleInSeconds), generator.ProductionCycleInSeconds.ToString());
    }
  }
}
