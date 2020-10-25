using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMenu : MonoBehaviour
{
    [SerializeField] private GameObject titleUI = null;

    public void BtnBack()
    {
        titleUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        GameManager.instance.GameStart();
        this.gameObject.SetActive(false);
    }
}