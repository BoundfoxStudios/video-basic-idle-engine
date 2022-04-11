using System;
using IdleEngine.SaveSystem;
using IdleEngine.Sessions;
using IdleEngine.UserInterface;
using TMPro;
using UnityEngine;

namespace IdleEngine
{
  public class IdleEngine : MonoBehaviour
  {
    public Session Session;
    public GeneratorUi GeneratorUiPrefab;
    public Transform GeneratorContainer;
    public TextMeshProUGUI CoinsText;

    private void Awake()
    {
      ClearGenerators();
      CreateGeneratorUis();
    }

    private void ClearGenerators()
    {
      for (var i = GeneratorContainer.childCount - 1; i >= 0; i--)
      {
        Destroy(GeneratorContainer.GetChild(i).gameObject);
      }
    }

    private void CreateGeneratorUis()
    {
      if (!Session)
      {
        return;
      }

      foreach (var generator in Session.Generators)
      {
        var generatorUi = Instantiate(GeneratorUiPrefab, GeneratorContainer, false);
        generatorUi.SetGenerator(generator, Session);
      }
    }

    private void Update()
    {
      if (!Session)
      {
        return;
      }

      Session.Tick(Time.deltaTime);
    }

    private void LateUpdate()
    {
      if (!Session)
      {
        return;
      }

      CoinsText.text = Session.Money.InScientificNotation();
    }

    private void OnEnable()
    {
      if (!Session)
      {
        return;
      }

      Load();

      Session.CalculateOfflineProgression();
    }

    private void OnDisable()
    {
      if (!Session)
      {
        return;
      }

      Session.LastTicks = DateTime.UtcNow.Ticks;
      
      Save();
    }

    private void Save()
    {
      var data = Session.GetRestorableData();
      var json = JsonUtility.ToJson(data);
      
      SaveFileManager.Write(Session.name, json);
    }

    private void Load()
    {
      if (!SaveFileManager.TryLoad(Session.name, out var content))
      {
        // Neues Spiel wurde angefangen
        Session.Money = Session.Generators[0].NextBuildingCostsForOne;
        return;
      }
      
      // Spiel laden
      var data = JsonUtility.FromJson<Session.SaveData>(content);
      Session.SetRestorableData(data);
    }
  }
}
