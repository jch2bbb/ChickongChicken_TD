using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;

    private void Update()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected(int index)
    {
        BuildManager.main.SetSelectedTower(index);
    }
}