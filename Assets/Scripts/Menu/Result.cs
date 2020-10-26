using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private GameObject goUI = null;

    [SerializeField] private Text[] txtCount = null;
    [SerializeField] private Text txtCoin = null;
    [SerializeField] private Text txtScore = null;
    [SerializeField] private Text txtMaxCombo = null;

    private int currentSong = 0;
    public int SetCurrentSong { set { currentSong = value; } }

    private ScoreManager scoreManager = null;
    private ComboManager comboManager = null;
    private TimingManager timingManager = null;
    private DataBaseManager dataBaseManager = null;

    private void Start()
    {
        if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
        if (comboManager == null) comboManager = FindObjectOfType<ComboManager>();
        if (timingManager == null) timingManager = FindObjectOfType<TimingManager>();
        if (dataBaseManager == null) dataBaseManager = FindObjectOfType<DataBaseManager>();
    }

    public void ShowResult()
    {
        FindObjectOfType<CenterFlame>().ResetMusic();

        AudioManager.instance.StopBGM();

        goUI.SetActive(true);

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = "0";
        }

        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] judgement = timingManager.JudgementRecord;
        int currentScore = scoreManager.CurrentScore;
        int maxCombo = comboManager.MaxCombo;
        int coin = currentScore / 50;

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", judgement[i]);
        }

        txtScore.text = string.Format("{0:#,##0}", currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", coin);

        if (currentScore > dataBaseManager.score[this.currentSong])
        {
            dataBaseManager.score[this.currentSong] = currentScore;
            dataBaseManager.SaveScore();
        }
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        comboManager.ResetCombo();
    }
}