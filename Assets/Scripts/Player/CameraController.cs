using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vc;
    // [SerializeField] CinemachineComposer cm;

    CinemachineFramingTransposer aa;

    private void Awake()
    {
        aa = vc.GetCinemachineComponent<CinemachineFramingTransposer>();
        aa.m_TrackedObjectOffset.y = 0f;
    }

    private void Update()
    {

    }

    private void LookUpDown()
    {
        aa = vc.GetCinemachineComponent<CinemachineFramingTransposer>();
        aa.m_TrackedObjectOffset.y = 50f;
        //cm.m_TrackedObjectOffset.y = 50f;
    }

    // TODO : 쳐다보는 방향 = inputDir에 따라 body.x를 증감쇠 하여 바라보는 방향쪽이 더 많이 보이게끔, 위 아래 방향키를 누르면 1초정도 후에 body.y를 증감쇠하여 바라보게끔
    // 맵의 가장 맨 끝 Ground, Wall을 체크하여 맨 끝에 있는게 아니라면 Player가 화면 가운데에 위치, map크기에 맞는 polygon collier로 confiner2D를 설정하여 해결 가능 할 것 같음
    // 맵마다 맵의 크기에 맞는 polygon collier 설정 요망
    // 쳐다보는거는 0.3f, 카메라 무빙은 1f
}
