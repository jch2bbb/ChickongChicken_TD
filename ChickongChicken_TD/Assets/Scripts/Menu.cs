using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI selectedTowerText;
    [SerializeField] TextMeshProUGUI cantAffordText;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    private void Start()
    {
        if (cantAffordText != null)
            cantAffordText.gameObject.SetActive(false);

        if (selectedTowerText != null)
            selectedTowerText.text = "";
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void Update()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected(int index)
    {
        BuildManager.main.SetSelectedTower(index);

        Tower selected = BuildManager.main.GetSelectedTower();
        if (selectedTowerText != null && selected != null)
        {
            selectedTowerText.text = "Selected: " + selected.name;
        }

        if (cantAffordText != null)
            cantAffordText.gameObject.SetActive(false);
    }

    public void ShowCantAfford()
    {
        if (cantAffordText != null)
        {
            cantAffordText.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(HideCantAffordAfterDelay());
        }
    }

    private IEnumerator HideCantAffordAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        cantAffordText.gameObject.SetActive(false);
    }
}