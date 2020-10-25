using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 0.1f;
    [SerializeField] private int blinkCount = 10;
    private int currentBlinkCount = 0;
    private bool isBlink = false;

    private bool isDead = false;
    public bool IsDead => isDead;

    private int maxHp = 3;
    private int currentHp = 3;

    private int maxShield = 3;
    private int currentShield = 0;

    [SerializeField] private GameObject[] hpObject = null;
    [SerializeField] private GameObject[] shieldObject = null;

    [SerializeField] private Image shieldGauge = null;
    [SerializeField] private int shieldIncreaseCombo = 5;
    private int currentShieldCombo = 0;

    private Result result = null;
    private NoteManager noteManager = null;
    [SerializeField] private MeshRenderer playerMesh = null;

    private void Start()
    {
        result = FindObjectOfType<Result>();
        noteManager = FindObjectOfType<NoteManager>();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if (currentShieldCombo >= shieldIncreaseCombo)
        {
            currentShieldCombo = 0;
            IncreaseShield();
        }

        shieldGauge.fillAmount = (float)(currentShieldCombo / shieldIncreaseCombo);
    }

    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGauge.fillAmount = (float)(currentShieldCombo / shieldIncreaseCombo);
    }

    public void IncreaseShield()
    {
        Mathf.Clamp(currentShield++, 0, maxShield);

        SettingShieldObject();
    }

    public void DecreaseShield(int p_num)
    {
        currentShield = Mathf.Max(currentShield -= p_num, 0);

        SettingShieldObject();
    }

    public void IncreaseHp(int p_num)
    {
        Mathf.Clamp(currentHp += p_num, 0, maxHp);

        SettingHpObject();
    }

    public void DecreaseHp(int p_num)
    {
        if (isBlink)
            return;

        if (currentShield > 0)
        {
            DecreaseShield(p_num);
        }
        else
        {
            currentHp -= p_num;
            if (currentHp <= 0)
            {
                isDead = true;
                result.ShowResult();
                noteManager.RemoveNote();
            }
            else
            {
                StartCoroutine(BlinkCo());
            }

            SettingHpObject();
        }
    }

    private void SettingHpObject()
    {
        for (int i = 0; i < hpObject.Length; i++)
        {
            if (i < currentHp)
            {
                hpObject[i].SetActive(true);
            }
            else
            {
                hpObject[i].SetActive(false);
            }
        }
    }

    private void SettingShieldObject()
    {
        for (int i = 0; i < shieldObject.Length; i++)
        {
            if (i < currentShield)
            {
                shieldObject[i].SetActive(true);
            }
            else
            {
                shieldObject[i].SetActive(false);
            }
        }
    }

    private IEnumerator BlinkCo()
    {
        isBlink = true;

        while (currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentBlinkCount++;
        }

        playerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}