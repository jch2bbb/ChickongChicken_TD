using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Image[] heartImages;

    public void UpdateHearts(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = heartSprite;
            }
            else
            {
                heartImages[i].sprite = emptySprite;
            }
        }
    }
}