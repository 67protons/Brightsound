using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    Player player;
    public RectTransform lightCooldown, soundCooldown;
    public List<GameObject> healthNotes = new List<GameObject>();
    int currentPlayerHealth;
    Vector2 lightCDDimensions, soundCDDimensions;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lightCDDimensions = lightCooldown.sizeDelta;
        soundCDDimensions = soundCooldown.sizeDelta;
    }

    void Start()
    {
        currentPlayerHealth = player.health;
    }

    void Update()
    {
        lightCooldown.sizeDelta = new Vector2(lightCDDimensions.x, lightCDDimensions.y * (player.lightTimer / player.lightCooldown));
        soundCooldown.sizeDelta = new Vector2(soundCDDimensions.x, soundCDDimensions.y * (player.soundTimer / player.soundCooldown));
        if (player.health < currentPlayerHealth)
        {
            for (int i = player.health; i < currentPlayerHealth; i++)
            {
                healthNotes[i].SetActive(false);
            }
            currentPlayerHealth = player.health;
        }
    }
}
