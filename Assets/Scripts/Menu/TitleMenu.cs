using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject stageUI = null;

    public void BtnPlay()
    {
        stageUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}