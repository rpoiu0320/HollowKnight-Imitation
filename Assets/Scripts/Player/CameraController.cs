using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Collider2D mapBoundary;
    private CinemachineVirtualCamera cmVC;
    private CinemachineFramingTransposer cmFT;
    private CinemachineConfiner2D cmConfiner2D;
    private Player player;
    private PlayerMover playerMover;
    private PlayerAttacker playerAttacker;
    private PlayerSkiller playerSkiller;
    
    private bool changeVertical;

    private void Awake()
    {
        mapBoundary = GameObject.FindWithTag("MapBoundary").GetComponent<Collider2D>();
        cmVC = GetComponent<CinemachineVirtualCamera>();
        cmFT = cmVC.GetCinemachineComponent<CinemachineFramingTransposer>();
        cmConfiner2D = cmVC.GetComponent<CinemachineConfiner2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        playerMover = player.GetComponent<PlayerMover>();
        playerAttacker = player.GetComponent<PlayerAttacker>();
        playerSkiller = player.GetComponent<PlayerSkiller>();
    }

    private void Start()
    {
        cmVC.Follow = player.transform;
        cmConfiner2D.m_BoundingShape2D = mapBoundary;
    }

    private void FixedUpdate()
    {
        // TODO : LimitMove상태에서 카메라 움직임 제한, 아래의 코드를 그냥 쓰면 LimitMove상태에서 바라보는 반대의 입력을 했을 때 카메라와 플레이어가 바라보는 방향이 일치하지 않음
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

        if (playerSkiller.IsSkill())
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
