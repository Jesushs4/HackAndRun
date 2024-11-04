using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    // Level timer
    private int minutes = 0;
    private int seconds = 0;
    [SerializeField] private TextMeshProUGUI timeText;

    // Level health
    private int health;
    [SerializeField] private Sprite emptyHealthImage;
    [SerializeField] private GameObject healthObject;
    private Image[] healthBar;

    
    private void Awake()
    {
        healthBar = healthObject.GetComponentsInChildren<Image>();
    }

    void Update()
    {
        
        UpdateHealthBar();

        GameManager.Instance.Timer += Time.deltaTime;

        seconds = (int)GameManager.Instance.Timer % 60;
        minutes = (int)GameManager.Instance.Timer / 60;
    }

    private void OnGUI()
    {
        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    /// <summary>
    /// Updates the health bar sprites
    /// </summary>
    private void UpdateHealthBar()
    {
        health = GameManager.Instance.Health;
        for (int i = 0; i < healthBar.Length; i++)
        {
            if (i >= health)
            {
                healthBar[i].sprite = emptyHealthImage;
            }

        }
    }
}

