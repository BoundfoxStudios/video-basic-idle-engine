using System;
using System.Linq;
using IdleEngine.Sessions;
using UnityEngine;

namespace IdleEngine.Generators
{
  [CreateAssetMenu(fileName = "Generator", menuName = "Game/Generator")]
  public class Generator : ScriptableObject, ISerializationCallbackReceiver
  {
    private double _multiplier;
    public int Owned;
    public double BaseCost;
    public double BaseRevenue;
    public float BaseProductionTimeInSeconds;
    public double CostFactor;
    public Multiplier[] Multipliers;
    public float ProductionCycleInSeconds;
    public string Name;
    public Sprite Image;

    // 0..1
    public float ProductionCycleNormalized => ProductionCycleInSeconds / ProductionTimeInSeconds;

    [NonSerialized]
    public float ProductionTimeInSeconds;

    [NonSerialized]
    public double NextBuildingCostsForOne;

    [NonSerialized]
    public double MoneyPerMinute;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
      Precalculate();
    }

    public bool CanBeBuild(Session session)
    {
      return session.Money >= NextBuildingCostsForOne;
    }

    public void Build(Session session)
    {
      if (!CanBeBuild(session))
      {
        return;
      }

      Owned++;
      session.Money -= NextBuildingCostsForOne;
      Precalculate();
    }

    public double Produce(float deltaTimeInSeconds)
    {
      return Produce(deltaTimeInSeconds, ref ProductionCycleInSeconds);
    }

    private double Produce(float deltaTimeInSeconds, ref float productionCycleInSeconds)
    {
      if (Owned == 0)
      {
        return 0;
      }

      productionCycleInSeconds += deltaTimeInSeconds;

      double calculatedSum = 0;

      while (productionCycleInSeconds >= ProductionTimeInSeconds)
      {
        calculatedSum += BaseRevenue * Owned * _multiplier;
        productionCycleInSeconds -= ProductionTimeInSeconds;
      }

      return calculatedSum;
    }

    private void Precalculate()
    {
      UpdateModifiers();
      UpdateMultiplier();
      UpdateNextBuildingCosts();
      UpdateMoneyPerMinute();
    }

    private void UpdateMoneyPerMinute()
    {
      var productionCycleInSeconds = 0f;
      MoneyPerMinute = Produce(60, ref productionCycleInSeconds);
    }

    private void UpdateNextBuildingCosts()
    {
      var kOverR = Math.Pow(CostFactor, Owned);
      var kPlusNOverR = Math.Pow(CostFactor, Owned + 1);

      NextBuildingCostsForOne = BaseCost *
                                (
                                  (kOverR - kPlusNOverR)
                                  /
                                  (1 - CostFactor)
                                );
    }

    private void UpdateModifiers()
    {
      ProductionTimeInSeconds = BaseProductionTimeInSeconds;

      if (Owned > 10)
      {
        ProductionTimeInSeconds /= 2;
      }
    }

    private void UpdateMultiplier()
    {
      if (Multipliers == null)
      {
        _multiplier = 1;
        return;
      }

      _multiplier = Multipliers.Aggregate(1d, (acc, multiplier) => acc * (multiplier.Level <= Owned ? multiplier.Value : 1));
    }

    private void OnValidate()
    {
      Precalculate();
    }
  }
}
