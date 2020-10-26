using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;

}

public class StageMenu : MonoBehaviour
{
    [SerializeField] private Song[] songList = null;

    [SerializeField] private Text txtSongName = null;
    [SerializeField] private Text txtSongComposer = null;
    [SerializeField] private Text txtSongScore = null;
    [SerializeField] private Image imgDisk = null;

    [SerializeField] private GameObject titleUI = null;

    private int currentSong = 0;

    private DataBaseManager dataBaseManager = null;

    private void OnEnable()
    {
        if (dataBaseManager == null) dataBaseManager = FindObjectOfType<DataBaseManager>();
        
        SettingSong();
    }

    public void BtnNext()
    {
        AudioManager.instance.PlaySFX(SFXName.SFX_Touch);

        if (++currentSong > songList.Length - 1)
            currentSong = 0;

        SettingSong();
    }

    public void BtnPrior()
    {
        AudioManager.instance.PlaySFX(SFXName.SFX_Touch);

        if (--currentSong < 0)
            currentSong = songList.Length - 1;

        SettingSong();
    }

    private void SettingSong()
    {
        txtSongName.text = songList[currentSong].name;
        txtSongComposer.text = songList[currentSong].composer;
        txtSongScore.text = string.Format("{0:#,##0}", dataBaseManager.score[currentSong]);
        imgDisk.sprite = songList[currentSong].sprite;

        AudioManager.instance.PlayBGM($"BGM{currentSong}");
    }

    public void BtnBack()
    {
        titleUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int bpm = songList[currentSong].bpm;

        GameManager.instance.GameStart(currentSong, bpm);
        this.gameObject.SetActive(false);
    }
}