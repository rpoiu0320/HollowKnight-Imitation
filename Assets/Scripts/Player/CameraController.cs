using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cmVC;
    private CinemachineFramingTransposer cmFT;
    private PlayerMover playerMover;
    private PlayerAttacker playerAttacker;
    private bool changeVertical;

    private void Awake()
    {
        cmFT = cmVC.GetCinemachineComponent<CinemachineFramingTransposer>();
        playerMover = GetComponent<PlayerMover>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    private void FixedUpdate()
    {
        //if (playerMover.LimitMove())
        //    return;

        HorizonCameraMoving();

        if(changeVertical != playerMover.IsCameraMove())    // 수직으로 카메라가 변경될 때
        {
            changeVertical = playerMover.IsCameraMove();
            VerticalCameraMoving();
        }
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

    // TODO : 맵의 가장 맨 끝 Ground, Wall을 체크하여 맨 끝에 있는게 아니라면 Player가 화면 가운데에 위치, map크기에 맞는 polygon collier로 confiner2D를 설정하여 해결 가능 할 것 같음
    // 맵마다 맵의 크기에 맞는 polygon collier 설정 요망
}
