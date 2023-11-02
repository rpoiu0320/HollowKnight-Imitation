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
    private CinemachineBasicMultiChannelPerlin cmBMCP;
    private Player player;

    private void Awake()
    {
        mapBoundary = GameObject.FindWithTag("MapBoundary").GetComponent<Collider2D>();
        cmVC = GameObject.FindWithTag("CMcamera").GetComponent<CinemachineVirtualCamera>();
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

    private void RenewalHorizon(float dirX)
    {
        if (dirX > 0)
            cmFT.m_TrackedObjectOffset.x = 10f;
        else if (dirX < 0)
            cmFT.m_TrackedObjectOffset.x = -10f;
    }

    /// <summary>
    /// <para>
    /// Camera 세로축 초기화
    /// </summary>
    public void ResetVertical()
    {
        cmFT.m_TrackedObjectOffset.y = 0f;
    }

    public void UpDownVertical(float dirY)
    {
        if (dirY > 0)
            cmFT.m_TrackedObjectOffset.y = 15f;
        else if (dirY < 0)
            cmFT.m_TrackedObjectOffset.y = -15f;
    }

    #region CameraNoise
    public void CameraNoise()
    {
        noiseRoutine = StartCoroutine(NoiseRoutine());
    }

    Coroutine noiseRoutine;
    IEnumerator NoiseRoutine()
    {
        cmBMCP.m_AmplitudeGain = 5f;

        yield return new WaitForSeconds(0.3f);

        cmBMCP.m_AmplitudeGain = 0f;
    }
    #endregion
}
