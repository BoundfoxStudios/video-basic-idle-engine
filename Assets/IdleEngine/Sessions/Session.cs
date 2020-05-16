using System;
using System.Linq;
using IdleEngine.Generators;
using UnityEngine;

namespace IdleEngine.Sessions
{
  [CreateAssetMenu(fileName = "Session", menuName = "Game/Session")]
  public class Session : ScriptableObject
  {
    public Generator[] Generators;
    public double Money;
    public long LastTicks;

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

    public void SaveTicks()
    {
      LastTicks = DateTime.UtcNow.Ticks;
    }
  }
}
