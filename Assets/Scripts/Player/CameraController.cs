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

        if(changeVertical != playerMover.IsCameraMove())    // �������� ī�޶� ����� ��
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

    // TODO : ���� ���� �� �� Ground, Wall�� üũ�Ͽ� �� ���� �ִ°� �ƴ϶�� Player�� ȭ�� ����� ��ġ, mapũ�⿡ �´� polygon collier�� confiner2D�� �����Ͽ� �ذ� ���� �� �� ����
    // �ʸ��� ���� ũ�⿡ �´� polygon collier ���� ���
}
