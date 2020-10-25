using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private GameObject goComboImage = null;
    [SerializeField] private Text txtCombo = null;

    private int currentCombo = 0;
    private int maxCombo = 0;
    public int MaxCombo => maxCombo;
    private Animator myAnimator = null;
    private int t_ComboUpTrigger = 0;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        t_ComboUpTrigger = Animator.StringToHash("ComboUp");

        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);

        if (maxCombo < currentCombo)
        {
            maxCombo = currentCombo;
        }

        if (currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);

            myAnimator.SetTrigger(t_ComboUpTrigger);
        }
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }
}