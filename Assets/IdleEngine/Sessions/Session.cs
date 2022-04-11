using System;
using System.Linq;
using IdleEngine.Generators;
using IdleEngine.SaveSystem;
using UnityEngine;

namespace IdleEngine.Sessions
{
  [CreateAssetMenu(fileName = "Session", menuName = "Game/Session")]
  public class Session : ScriptableObject, IRestorable<Session.SaveData>
  {
    [Serializable]
    public class RuntimeData
    {
      public double Money;
      public long LastTicks;
    }

    [Serializable]
    public class SaveData : RuntimeData
    {
      public Generator.RuntimeData[] Generators;
    }

    private RuntimeData _data = new();
    
    public Generator[] Generators;

    public double Money
    {
      get => _data.Money;
      set => _data.Money = value;
    }

    public long LastTicks
    {
      get => _data.LastTicks;
      set => _data.LastTicks = value;
    }

    private void OnEnable()
    {
      _data = new RuntimeData();
    }

    public void Tick(float deltaTimeInSeconds)
    {
      Money += CalculateProgress(deltaTimeInSeconds);
    }

    private double CalculateProgress(float deltaTimeInSeconds)
    {
      return Generators == null ? 0 : Generators.Sum(generator => generator.Produce(deltaTimeInSeconds));
    }

    public void CalculateOfflineProgression()
    {
      if (LastTicks <= 0)
      {
        return;
      }

      var deltaTime = (DateTime.UtcNow.Ticks - LastTicks) / TimeSpan.TicksPerSecond;

      var moneyBefore = Money;

      Tick(deltaTime);

      Debug.Log($"Calculated offline progression: {Money - moneyBefore}");
    }

    public SaveData GetRestorableData()
    {
      return new SaveData()
      {
        Money = Money,
        LastTicks = LastTicks,
        Generators = Generators.Select(generator => generator.GetRestorableData()).ToArray()
      };
    }

    public void SetRestorableData(SaveData data)
    {
      Money = data.Money;
      LastTicks = data.LastTicks;

      foreach (var generator in Generators)
      {
        var savedGenerator = data.Generators.SingleOrDefault(g => g.Id == generator.name);

        if (savedGenerator is null)
        {
          Debug.LogWarning($"Did not find generator {generator.name} in save game");
          continue;
        }
        
        generator.SetRestorableData(savedGenerator);
      }
    }
  }
}
