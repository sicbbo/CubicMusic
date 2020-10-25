using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Animator noteHitAnimator = null;
    private int hitTrigger = 0;

    [SerializeField] private Animator judgementAnimator = null;
    [SerializeField] private Image judgementImage = null;
    [SerializeField] private Sprite[] judgementSprite = null;

    private void Start()
    {
        hitTrigger = Animator.StringToHash("Hit");
    }

    public void JudgementEffect(int num)
    {
        judgementImage.sprite = judgementSprite[num];
        judgementAnimator.SetTrigger(hitTrigger);
    }

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hitTrigger);
    }
}