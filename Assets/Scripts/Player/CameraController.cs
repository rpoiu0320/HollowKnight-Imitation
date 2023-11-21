using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    private Collider2D mapBoundary;
    private CinemachineVirtualCamera cmVC;
    private CinemachineFramingTransposer cmFT;
    private CinemachineConfiner2D cmConfiner2D;
    private CinemachineBasicMultiChannelPerlin cmBMCP;

    private void Awake()
    {
        mapBoundary = GameObject.FindWithTag("MapBoundary").GetComponent<Collider2D>();
        cmVC = GetComponent<CinemachineVirtualCamera>();
        cmFT = cmVC.GetCinemachineComponent<CinemachineFramingTransposer>();
        cmBMCP = cmVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmConfiner2D = cmVC.GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        cmVC.Follow = player.transform;
        cmConfiner2D.m_BoundingShape2D = mapBoundary;
    }

    private void FixedUpdate()
    {
        RenewalHorizon(player.inputDir.x);
    }

    #region Moving Camera
    /// <summary>
    /// 캐릭터가 바라보는 가로 방향으로 Camera 이동
    /// </summary>
    /// <param name="dirX"></param>
    private void RenewalHorizon(float dirX)
    {
        if (dirX > 0)
            cmFT.m_TrackedObjectOffset.x = 10f;
        else if (dirX < 0)
            cmFT.m_TrackedObjectOffset.x = -10f;
    }

    /// <summary>
    /// Camera 세로축 초기화
    /// </summary>
    public void ResetVertical()
    {
        cmFT.m_TrackedObjectOffset.y = 0f;
    }

    /// <summary>
    /// Camera 세로축으로 일정거리 이동
    /// </summary>
    /// <param name="dirY"></param>
    public void UpDownVertical(float dirY)
    {
        if (dirY > 0)
            cmFT.m_TrackedObjectOffset.y = 15f;
        else if (dirY < 0)
            cmFT.m_TrackedObjectOffset.y = -15f;
    }
    #endregion

    #region CameraNoise
    /// <summary>
    /// NoiseRoutine을 실행시켜줌
    /// </summary>
    public void CameraNoise()
    {
        noiseRoutine = StartCoroutine(NoiseRoutine());
    }

    Coroutine noiseRoutine;
    /// <summary>
    /// 일정 시간동안 화면 진동
    /// </summary>
    /// <returns></returns>
    IEnumerator NoiseRoutine()
    {
        cmBMCP.m_AmplitudeGain = 5f;

        yield return new WaitForSeconds(0.3f);

        cmBMCP.m_AmplitudeGain = 0f;
    }
    #endregion
}
