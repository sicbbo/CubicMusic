using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTrans = null;
    [SerializeField] private float followSpeed = 15f;

    private Transform rootTrans = null;
    private Vector3 playerDistance = Vector3.zero;

    [SerializeField] private float zoomDistance = -1.25f;
    private float hitDistance = 0f;
    private WaitForSeconds waitFor = new WaitForSeconds(0.15f);

    private void Start()
    {
        rootTrans = transform;

        playerDistance = rootTrans.position - playerTrans.position;
    }

    private void Update()
    {
        Vector3 destPos = playerTrans.position + playerDistance + (rootTrans.forward * hitDistance);
        rootTrans.position = Vector3.Lerp(rootTrans.position, destPos, followSpeed * Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return waitFor;

        hitDistance = 0f;
    }
}