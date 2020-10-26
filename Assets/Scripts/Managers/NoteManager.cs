using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] private Transform noteAppearTrans = null;
    public int bpm = 0;

    private double currentTime = 0d;

    private TimingManager timingManager = null;
    private EffectManager effectManager = null;
    private ComboManager comboManager = null;

    private void Start()
    {
        timingManager = GetComponent<TimingManager>();
        effectManager = FindObjectOfType<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    private void Update()
    {
        if (!GameManager.instance.isStartGame)
            return;

        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)   // 60s / BPM = 1Beat 시간 (60 / 120 = 1beat 당 0.5초)
        {
            GameObject note = ObjectPool.instance.noteQueue.Dequeue();
            note.transform.position = noteAppearTrans.position;
            note.SetActive(true);

            timingManager.boxNoteList.Add(note);

            // 0으로 초기화하면 안됨(currentTime = 0.51005551... -> 0.01005551의 시간만큼 오차가 손실됨)
            // 0으로 초기화시 다음 노트는 오차값만큼 빨리 나옴
            currentTime -= 60d / bpm;
            
        }
    }

    /// <summary>
    /// 노트가 화면 밖으로 벗어난 경우
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(CollectionTag.NoteTag))    // 노트 태그 확인
        {
            if (collision.GetComponent<Note>().GetNoteFlag())   // 노트 이미지의 활성화 확인(비활성화인 경우 => 흘린 노트가 아니다)
            {
                timingManager.MissRecord();
                comboManager.ResetCombo();
                effectManager.JudgementEffect((int)Judgement.Miss);
            }

            timingManager.boxNoteList.Remove(collision.gameObject);

            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    public void RemoveNote()
    {
        GameManager.instance.isStartGame = false;

        for (int i = 0; i < timingManager.boxNoteList.Count; i++)
        {
            timingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(timingManager.boxNoteList[i]);
        }

        timingManager.boxNoteList.Clear();
    }
}