using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradePriceText;
    [SerializeField] private TextMeshProUGUI cantAffordUpgradeText;

    private bool mouse_over = false;
    private Tree treeRef;
    private TreeSlowmo treeSlowmoRef;

    private void Awake()
    {
        treeRef = GetComponentInParent<Tree>();
        treeSlowmoRef = GetComponentInParent<TreeSlowmo>();
    }

    private void Start()
    {
        if (cantAffordUpgradeText != null)
            cantAffordUpgradeText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UpdatePriceText();

        if (cantAffordUpgradeText != null)
            cantAffordUpgradeText.gameObject.SetActive(false);
    }

    private void UpdatePriceText()
    {
        if (upgradePriceText == null) return;

        if (treeRef != null)
        {
            upgradePriceText.text = "Cost: " + treeRef.GetUpgradeCost().ToString();
        }
        else if (treeSlowmoRef != null)
        {
            upgradePriceText.text = "Cost: " + treeSlowmoRef.GetUpgradeCost().ToString();
        }
    }

    public void ShowCantAffordUpgrade()
    {
        if (cantAffordUpgradeText != null)
        {
            cantAffordUpgradeText.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(HideCantAffordAfterDelay());
        }
    }

    private IEnumerator HideCantAffordAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (cantAffordUpgradeText != null)
            cantAffordUpgradeText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }
}