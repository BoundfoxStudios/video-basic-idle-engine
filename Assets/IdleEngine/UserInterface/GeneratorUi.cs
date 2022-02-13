using IdleEngine.Generators;
using IdleEngine.Sessions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
  public class GeneratorUi : MonoBehaviour
  {
    public TextMeshProUGUI NextCostText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI PirateNameText;
    public TextMeshProUGUI IncomePerMinuteText;
    public Image ProgressionImage;
    public Image PirateImage;
    public Button BuyButton;
    
    private Generator _generator;
    private Session _session;

    public void SetGenerator(Generator generator, Session session)
    {
      _session = session;
      _generator = generator;
      
      PirateImage.sprite = generator.Image;
      PirateNameText.text = generator.Name;
    }

    public void Buy()
    {
      _generator.Build(_session);
    }

    private void LateUpdate()
    {
      UpdateUi();
    }

    private void UpdateUi()
    {
      if (!_generator)
      {
        return;
      }

      NextCostText.text = _generator.NextBuildingCostsForOne.InScientificNotation();
      LevelText.text = _generator.Owned.ToString();
      ProgressionImage.fillAmount = _generator.ProductionCycleNormalized;
      BuyButton.interactable = _generator.CanBeBuild(_session);
      IncomePerMinuteText.text = $"{_generator.MoneyPerMinute.InScientificNotation()}/m";
    }
  }
}
