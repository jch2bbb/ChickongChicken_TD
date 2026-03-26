using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public static Menu main;

    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI selectedTowerText;
    [SerializeField] TextMeshProUGUI cantAffordText;
    [SerializeField] Image selectedTowerImage;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        if (cantAffordText != null)
            cantAffordText.gameObject.SetActive(false);

        if (selectedTowerText != null)
            selectedTowerText.text = "";

        UpdateSelectedTowerUI();
    }

    private void Update()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }

    public void UpdateSelectedTowerUI()
    {
        Tower selected = BuildManager.main.GetSelectedTower();
        if (selected == null) return;

        if (selectedTowerText != null)
            selectedTowerText.text = selected.name;

        if (selectedTowerImage != null)
        {
            if (selected.sprite != null)
            {
                selectedTowerImage.sprite = selected.sprite;
                selectedTowerImage.enabled = true;
                selectedTowerImage.color = Color.white;
            }
            else
            {
                selectedTowerImage.enabled = false;
                UnityEngine.Debug.Log("No sprite assigned for: " + selected.name);
            }
        }
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