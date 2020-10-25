using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [NonSerialized] public bool isStartGame = false;

    [SerializeField] private GameObject[] gameUI = null;
    [SerializeField] private GameObject titleUI = null;

    private void Start()
    {
        if (instance == null) instance = this;
    }

    public void GameStart()
    {
        for (int i = 0; i < gameUI.Length; i++)
        {
            gameUI[i].SetActive(true);
        }

        isStartGame = true;
    }

    public void MainMenu()
    {
        for (int i = 0; i < gameUI.Length; i++)
        {
            gameUI[i].SetActive(false);
        }

        titleUI.SetActive(true);
    }
}