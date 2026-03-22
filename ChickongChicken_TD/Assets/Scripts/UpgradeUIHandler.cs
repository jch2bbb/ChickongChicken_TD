using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradePriceText;

    private bool mouse_over = false;
    private Tree treeRef;

    private void Awake()
    {
        // Get reference in Awake instead of Start
        // so it's ready before OnEnable fires
        treeRef = GetComponentInParent<Tree>();
    }

    private void OnEnable()
    {
        UpdatePriceText();
    }

    private void UpdatePriceText()
    {
        if (treeRef != null && upgradePriceText != null)
        {
            upgradePriceText.text = "Cost: " + treeRef.GetUpgradeCost().ToString();
        }
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