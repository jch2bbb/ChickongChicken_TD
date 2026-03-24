using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    private Tree tree;
    private TreeSlowmo treeSlowmo;
    private Color startColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startColor = sr.color;
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        bool isHoveringThisPlot = IsMouseOverThisPlot(mousePos);

        if (isHoveringThisPlot)
        {
            sr.color = hoverColor;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                BuildTower();
            }
        }
        else
        {
            sr.color = startColor;
        }
    }

    private bool IsMouseOverThisPlot(Vector2 mousePos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void BuildTower()
    {
        if (UIManager.main.IsHoveringUI()) return;

        if (towerObj != null)
        {
            if (tree != null)
            {
                tree.OpenUpgradeUI();
            }
            else if (treeSlowmo != null)
            {
                treeSlowmo.OpenUpgradeUI();
            }
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null) return;

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Menu.main.ShowCantAfford();
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);

        tree = towerObj.GetComponentInChildren<Tree>();
        treeSlowmo = towerObj.GetComponentInChildren<TreeSlowmo>();

        if (tree == null && treeSlowmo == null)
        {
            UnityEngine.Debug.LogWarning("No Tree or TreeSlowmo found on: " + towerObj.name);
        }
    }
}