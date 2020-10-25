using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    [SerializeField] private Transform centerTrans = null;  // 센터 트랜스폼
    [SerializeField] private RectTransform[] timingRect = null; // 판정 범위(Perfect, Cool, Good, Bad)

    public List<GameObject> boxNoteList = new List<GameObject>();

    private int[] judgementRecord = new int[5];
    public int[] JudgementRecord => judgementRecord;

    private Vector2[] timingBoxs = null;  // 판정 범위의 최소값(x), 최대값(y)

    private EffectManager effectManager = null;
    private ScoreManager scoreManager = null;
    private ComboManager comboManager = null;
    private StageManager stageManager = null;
    private PlayerController playerController = null;
    private StatusManager statusManager = null;
    private AudioManager audioManager = null;

    private void Start()
    {
        audioManager = AudioManager.instance;
        effectManager = FindObjectOfType<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        stageManager = FindObjectOfType<StageManager>();
        playerController = FindObjectOfType<PlayerController>();
        statusManager = FindObjectOfType<StatusManager>();

        // 타이밍 박스 설정
        timingBoxs = new Vector2[timingRect.Length];
        for (int i = 0; i < timingRect.Length; i++)
        {
            // 각각의 판정 범위 => 최소값 = 중심 - (이미지의 너비 / 2)
            //                     최대값 = 중심 + (이미지의 너비 / 2)
            timingBoxs[i].Set(centerTrans.localPosition.x - timingRect[i].rect.width / 2,
                              centerTrans.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming()
    {
        // 리스트에 있는 노트들을 확인해서 판정 박스에 있는 노트를 찾아야함
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            // 판정범위 최소값 <= 노트의 x값 <= 판정범위 최대값
            float notePosX = boxNoteList[i].transform.localPosition.x;

            // 판정 범위만큼 반복 => 어느 판정 범위에 있는지 확인
            for (int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= notePosX && notePosX <= timingBoxs[x].y)
                {
                    // 인덱스 0부터 확인하므로 판정순서도 Perfect -> Cool -> Good -> Bad
                    // 노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // UI 이펙트 연출
                    if (x < timingBoxs.Length - 1)  // Bad 판정시 연출하지 않도록
                        effectManager.NoteHitEffect();
                    

                    if (CheckCanNextPlate())
                    {
                        // 점수 증가
                        scoreManager.IncreaseScore((Judgement)x);
                        // 다음 플레이트 생성
                        stageManager.ShowNextPlate();
                        // 판정 연출
                        effectManager.JudgementEffect(x);
                        // 판정 기록
                        judgementRecord[x]++;
                        // 쉴드 체크
                        statusManager.CheckShield();
                    }
                    else
                    {
                        effectManager.JudgementEffect((int)Judgement.Normal);
                    }

                    audioManager.PlaySFX(SFXName.SFX_Clap);

                    return true;
                }
            }
        }

        comboManager.ResetCombo();
        effectManager.JudgementEffect((int)Judgement.Miss); // 미스 판정 연출
        MissRecord();
        return false;
    }

    private bool CheckCanNextPlate()
    {
        if (Physics.Raycast(playerController.destPos, Vector2.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag(CollectionTag.PlateTag))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }

        return false;
    }

    public void MissRecord()
    {
        judgementRecord[(int)Judgement.Miss]++; // 미스 판정 기록
        statusManager.ResetShieldCombo();
    }
}