using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400f;

    private Transform rootTrans = null;
    private Image noteImage = null;

    private void Awake()
    {
        rootTrans = this.transform;

        noteImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        noteImage.enabled = true;
    }

    private void Update()
    {
        rootTrans.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}