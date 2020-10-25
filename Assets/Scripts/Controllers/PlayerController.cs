using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_CanPressKey = true;

    // 이동
    [SerializeField] private float moveSpeed = 3f;
    private Vector3 dir = Vector3.zero;
    public Vector3 destPos = Vector3.zero;
    private Vector3 originPos = Vector3.zero;

    // 회전
    [SerializeField] private float spinSpeed = 270f;
    private Vector3 rotDir = Vector3.zero;
    private Quaternion destRot = Quaternion.identity;

    // 반동
    [SerializeField] private float recoilPosY = 0.25f;
    [SerializeField] private float recoilSpeed = 1.5f;

    private bool canMove = true;
    private bool isFalling = false;

    // 기타..
    [SerializeField] private Transform fakeCube = null;
    [SerializeField] private Transform realCube = null;

    private Transform myTrans = null;
    private Rigidbody myRigid = null;

    private TimingManager timingManager = null;
    private CameraController cameraController = null;
    private StatusManager statusManager = null;

    private void Start()
    {
        if (myTrans == null) myTrans = transform;
        if (myRigid == null) myRigid = GetComponentInChildren<Rigidbody>();
        if (timingManager == null) timingManager = FindObjectOfType<TimingManager>();
        if (cameraController == null) cameraController = FindObjectOfType<CameraController>();
        if (statusManager == null) statusManager = FindObjectOfType<StatusManager>();

        originPos = myTrans.position;
    }

    private void Update()
    {
        if (!GameManager.instance.isStartGame)
            return;

        CheckFalling();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (canMove && s_CanPressKey && !isFalling)
            {
                Calc();

                if (timingManager.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }

    private void Calc()
    {
        // 방향 계산
        dir.Set(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));

        // 이동 목표값 계산
        destPos = myTrans.position + new Vector3(-dir.x, 0f, dir.z);

        // 회전 목표값 계산
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        fakeCube.RotateAround(myTrans.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }

    private void StartAction()
    {
        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(cameraController.ZoomCam());
    }

    private IEnumerator MoveCo()
    {
        canMove = false;

        while(Vector3.SqrMagnitude(myTrans.position - destPos) >= 0.001f)
        {
            myTrans.position = Vector3.MoveTowards(myTrans.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        myTrans.position = destPos;
        canMove = true;
    }

    private IEnumerator SpinCo()
    {
        while(Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = destRot;
    }

    private IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0f, recoilSpeed * Time.deltaTime, 0f);
            yield return null;
        }

        while(realCube.position.y > 0f)
        {
            realCube.position -= new Vector3(0f, recoilSpeed * Time.deltaTime, 0f);
            yield return null;
        }

        realCube.localPosition = Vector3.zero;
    }

    private void CheckFalling()
    {
        if (isFalling && canMove)
            return;

        if (!Physics.Raycast(myTrans.position, Vector3.down, 1.1f))
        {
            Falling();
        }
    }

    private void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false;
    }

    public void ResetFalling()
    {
        statusManager.DecreaseHp(1);
        AudioManager.instance.PlaySFX(SFXName.SFX_Falling);

        if (!statusManager.IsDead)
        {
            isFalling = false;
            myRigid.useGravity = false;
            myRigid.isKinematic = true;
            myTrans.position = originPos;
            realCube.localPosition = Vector3.zero;
        }
    }
}