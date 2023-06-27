using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cmVC;
    CinemachineFramingTransposer cmFT;
    PlayerMover playerMover;
    PlayerAttacker playerAttacker;
    PlayerMover.UpDown upDown;
    Vector2 curDir;


    private void Awake()
    {
        playerMover = GetComponent<PlayerMover>();
        playerAttacker = GetComponent<PlayerAttacker>();
        curDir = playerMover.LookDir();
        cmFT = cmVC.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void FixedUpdate()
    { 
        HorizonCameraMoving();
        VerticalCameraMoving();
    }

    private void VerticalCameraMoving()
    {
        if (playerAttacker.IsAttack())
        {
            cmFT.m_TrackedObjectOffset.y = 0f;
            return;
        }

        switch (playerMover.LookingUpDown())
        {
            case PlayerMover.UpDown.Up:
                cmFT.m_TrackedObjectOffset.y = 15f;
                break;
            case PlayerMover.UpDown.Down:
                cmFT.m_TrackedObjectOffset.y = -15f;
                break;
            case PlayerMover.UpDown.None:
                cmFT.m_TrackedObjectOffset.y = 0f;
                break;
        }
    }

    private void HorizonCameraMoving()
    {
        if(playerMover.LookDir().x > 0)
        {
            cmFT.m_TrackedObjectOffset.x = 10f;
        }
        else if(playerMover.LookDir().x < 0)
        {
            cmFT.m_TrackedObjectOffset.x = -10f;
        }
    }

    // TODO : 쳐다보는 방향 = inputDir에 따라 body.x를 증감쇠 하여 바라보는 방향쪽이 더 많이 보이게끔, 위 아래 방향키를 누르면 1초정도 후에 body.y를 증감쇠하여 바라보게끔
    // 맵의 가장 맨 끝 Ground, Wall을 체크하여 맨 끝에 있는게 아니라면 Player가 화면 가운데에 위치, map크기에 맞는 polygon collier로 confiner2D를 설정하여 해결 가능 할 것 같음
    // 맵마다 맵의 크기에 맞는 polygon collier 설정 요망
    // 쳐다보는거는 시간 0.3f, 카메라 무빙은 1f
}
