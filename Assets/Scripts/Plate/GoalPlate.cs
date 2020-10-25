using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    private AudioSource audioSource = null;
    private NoteManager noteManager = null;

    private Result result = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        noteManager = FindObjectOfType<NoteManager>();
        result = FindObjectOfType<Result>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CollectionTag.PlayerTag))
        {
            audioSource.Play();
            PlayerController.s_CanPressKey = false;
            noteManager.RemoveNote();
            result.ShowResult();
        }
    }
}