using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    private bool musicStart = false;

    public string bgmName = string.Empty;

    public void ResetMusic()
    {
        musicStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (musicStart)
            return;

        if (collision.CompareTag(CollectionTag.NoteTag))
        {
            AudioManager.instance.PlayBGM(bgmName);
            musicStart = true;
        }
    }
}