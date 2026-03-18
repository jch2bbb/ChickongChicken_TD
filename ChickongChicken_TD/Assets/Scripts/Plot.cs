// using System.Collections;
// using System.Collections.Generic;
// using System.Numerics;
// using UnityEngine;

// public class Plot : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private SpriteRenderer sr;
//     [SerializeField] private Color hoverColor;

//     private GameObject tower;
//     private Color startColor;

//     private void Awake()
//     {
//         sr = GetComponent<SpriteRenderer>();
//     }

//     private void Start()
//     {
//         startColor = sr.color;
//     }

//     private void OnMouseEnter()
//     {
//         sr.color = hoverColor;
//     }
 
//     private void OnMouseExit()
//     {
//         sr.color = startColor;
//     }

//     private void OnMouseDown()
//     {
//         if (tower == null) return;

//         GameObject towerToBuild = BuildManager.main.GetSelectedTower();
//         Instantiate(tower, transform.position, Quaternion.identity);
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
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
        Collider2D hit = Physics2D.OverlapPoint(mousePos);

        if (hit != null && hit.gameObject == gameObject)
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

    private void BuildTower()
    {
        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null) return;

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            UnityEngine.Debug.Log("You can't afford this tower");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}