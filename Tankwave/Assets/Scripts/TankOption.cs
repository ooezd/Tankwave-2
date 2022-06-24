using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TankOption : MonoBehaviour
{
    [SerializeField] private Image tankImage;
    [SerializeField] private TankCard tankCard;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI bulletSpeedText;
    [SerializeField] private TextMeshProUGUI fireRateText;

    private void OnEnable()
    {
        Initialize();
    }
    public void Initialize()
    {
        tankImage.sprite = Container.instance.tankImages[tankCard.tankId];

        speedText.text = tankCard.speed.ToString();
        bulletSpeedText.text = tankCard.bulletSpeed.ToString();
        fireRateText.text = tankCard.fireRate.ToString();
    }
    public void OnClick()
    {
        CSVReader.instance.selectedTank = tankCard;
        MenuUIManager.instance.TankSelected();
    }

}
