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

    private ComboManager comboManager = null;
    private ScoreManager scoreManager = null;
    private TimingManager timingManager = null;
    private StatusManager statusManager = null;
    private PlayerController playerController = null;
    private StageManager stageManager = null;
    private NoteManager noteManager = null;
    private Result result = null;

    [SerializeField] private CenterFlame music = null;

    private void Start()
    {
        if (instance == null) instance = this;
        if (comboManager == null) comboManager = FindObjectOfType<ComboManager>();
        if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
        if (timingManager == null) timingManager = FindObjectOfType<TimingManager>();
        if (statusManager == null) statusManager = FindObjectOfType<StatusManager>();
        if (playerController == null) playerController = FindObjectOfType<PlayerController>();
        if (stageManager == null) stageManager = FindObjectOfType<StageManager>();
        if (noteManager == null) noteManager = FindObjectOfType<NoteManager>();
        if (result == null) result = FindObjectOfType<Result>();
    }

    public void GameStart(int p_SongNum, int p_BPM)
    {
        for (int i = 0; i < gameUI.Length; i++)
        {
            gameUI[i].SetActive(true);
        }

        music.bgmName = $"BGM{p_SongNum}";
        noteManager.bpm = p_BPM;
        stageManager.RemoveStage();
        stageManager.SettingStage(p_SongNum);
        comboManager.ResetCombo();
        scoreManager.Initialized();
        timingManager.Initialized();
        statusManager.Initialized();
        playerController.Initialized();
        result.SetCurrentSong = p_SongNum;

        AudioManager.instance.StopBGM();

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