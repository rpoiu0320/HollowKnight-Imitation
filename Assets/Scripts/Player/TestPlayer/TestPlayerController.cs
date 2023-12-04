using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;
    [Header("Speed")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpSpeed;

    private bool pressJumpKey;
    private bool isLook = false;
    private float lookTime = 0;

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    #region Move and Look
    /// <summary>
    /// player.inputDir을 기반으로 Character이동
    /// </summary>
    private void Move()
    {
        if (player.actionLimite)
            return;

        if (player.inputDir.x > 0)
        {
            lookTime = 0;
            player.animator.SetBool("IsLook", isLook = false);
            player.cameraController.ResetVertical();
            player.render.flipX = false;
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
        }
        else if (player.inputDir.x < 0)
        {
            lookTime = 0;
            player.animator.SetBool("IsLook", isLook = false);
            player.cameraController.ResetVertical();
            player.render.flipX = true;
            transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, 0));
        }
        else if (player.inputDir.y != 0)
        {
            player.animator.SetFloat("LookUpDown", player.inputDir.y);
            lookTime += Time.deltaTime;

            if (lookTime > 1.5f && !isLook)
                lookRoutine = StartCoroutine(LookRoutine(player.inputDir.y));
        }
        else if (player.inputDir.y == 0 || player.actionLimite)
        {
            player.animator.SetFloat("LookUpDown", 0);
            lookTime = 0;
        }
    }

    Coroutine lookRoutine;
    /// <summary>
    /// 위, 아래를 일정시간 바라보면 해당 방향으로 카메라 이동
    /// </summary>
    /// <param name="dirY"></param>
    /// <returns></returns>
    IEnumerator LookRoutine(float dirY)
    {
        player.animator.SetBool("IsLook", isLook = true);
        player.cameraController.UpDownVertical(dirY);

        while (true)
        {
            if (player.inputDir.y == 0 || player.actionLimite)
            {
                player.animator.SetBool("IsLook", isLook = false);

                break;
            }

            yield return null;
        }

        player.cameraController.ResetVertical();
    }
    #endregion

    #region Dash
    private void OnDash(InputValue value)
    {
        if (player.actionLimite)
            return;

        dashRoutine = StartCoroutine(DashRoutine());
    }

    Coroutine dashRoutine;
    /// <summary>
    /// Rigidboty.velocity를 사용한 대시,
    /// 대시 중 Layer 변경을 통한 무적
    /// </summary>
    /// <returns></returns>
    IEnumerator DashRoutine()
    {
        float dashTime = 0;
        player.actionLimite = true;
        gameObject.layer = 1 >> gameObject.layer;                               // Layer 변경으로 대시 중 무적 구현
        player.animator.SetTrigger("Dash");

        while (dashTime < 0.5f)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0);                // 대시 중 하강 방지

            if (player.render.flipX == false)                                  // 바라보는 방향으로 대시
                player.rb.velocity = new Vector2(dashSpeed, 0);
            else
                player.rb.velocity = new Vector2(-dashSpeed, 0);

            dashTime += Time.deltaTime;

            yield return null;
        }

        player.rb.velocity = new Vector2(0, 0);
        gameObject.layer = 3 << gameObject.layer;
        player.actionLimite = false;
    }
    #endregion

    #region jump
    private void OnJump(InputValue value)
    {
        pressJumpKey = value.isPressed;

        if (pressJumpKey && !player.actionLimite && player.isGround)
            jumpRoutine = StartCoroutine(JumpRoutine());
    }

    Coroutine jumpRoutine;
    /// <summary>
    /// Jump Key 누른 시간에 따른 점프 높이 변동
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpRoutine()
    {
        float jumpTime = 0;

        while (pressJumpKey && !player.actionLimite)
        {
            if (jumpTime > 0.4f)
                break;
            else
                player.rb.velocity = new Vector2(player.rb.velocity.x, jumpSpeed);        // 일정한 점프속도 보장
            
            jumpTime += Time.deltaTime;
            lookTime = 0; 
            player.animator.SetBool("IsLook", isLook = false);
            player.cameraController.ResetVertical();

            yield return null;
        }
    }
    #endregion

    #region GroundCheck
    /// <summary>
    /// Raycast를 사용해 Player 아래 Ground를 체크
    /// </summary>
    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 4f, player.groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 4f, Color.red);

        if (hit.collider != null)
            player.animator.SetBool("IsGround", player.isGround = true);
        else
            player.animator.SetBool("IsGround", player.isGround = false);
    }
    #endregion
}
