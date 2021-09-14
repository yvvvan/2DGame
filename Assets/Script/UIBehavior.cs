using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIBehavior : MonoBehaviour
{
    public TextMeshProUGUI gameTimeText;
    public GameState gameState;

    void Start() {
        Screen.SetResolution(1920, 1080, false);

    }

    void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameState.gameTime);
        gameTimeText.text = timeSpan.ToString("m':'ss");
    }
}
