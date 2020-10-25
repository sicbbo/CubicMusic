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

    private ScoreManager scoreManager = null;
    private ComboManager comboManager = null;
    private TimingManager timingManager = null;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        timingManager = FindObjectOfType<TimingManager>();
    }

    public void ShowResult()
    {
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
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        comboManager.ResetCombo();
    }
}