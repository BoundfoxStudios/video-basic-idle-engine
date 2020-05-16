using System;
using IdleEngine.Sessions;
using UnityEngine;

namespace IdleEngine
{
  public class IdleEngine : MonoBehaviour
  {
    public Session Session;

    private void Update()
    {
      if (!Session)
      {
        return;
      }

      Session.Tick(Time.deltaTime);
    }

    private void OnEnable()
    {
      if (!Session)
      {
        return;
      }

      Session.CalculateOfflineProgression();
    }

    private void OnDisable()
    {
      if (!Session)
      {
        return;
      }

      Session.LastTicks = DateTime.UtcNow.Ticks;
    }
  }
}
