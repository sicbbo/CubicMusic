using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private int increaseScore = 10;
    [SerializeField] private float[] weight = null;
    [SerializeField] private int comboBonusScore = 10;

    private int currentScore = 0;
    public int CurrentScore => currentScore;
    private Animator myAnimator = null;
    private int scoreUpTrigger = 0;

    private ComboManager comboManager = null;

    private void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
        myAnimator = GetComponent<Animator>();
        scoreUpTrigger = Animator.StringToHash("ScoreUp");

        currentScore = 0;
        scoreText.text = "0";
    }

    public void IncreaseScore(Judgement p_JudgementState)
    {
        // 콤보 증가
        comboManager.IncreaseCombo();

        // 콤보 보너스 점수 계산
        int t_currentCombo = comboManager.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        // 가중치 계산
        int t_increaseScore = increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * weight[(int)p_JudgementState]);

        // 점수 반영
        currentScore += t_increaseScore;
        scoreText.text = string.Format("{0:#,##0}", currentScore);

        // 점수 연출 실행
        myAnimator.SetTrigger(scoreUpTrigger);
    }
}