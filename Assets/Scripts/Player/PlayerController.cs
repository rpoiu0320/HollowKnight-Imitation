using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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

    #region Move and Look
    /// <summary>
    /// player.inputDir�� ������� Character�̵�
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

            if (!player.isGround)
                return;

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
    /// ��, �Ʒ��� �����ð� �ٶ󺸸� �ش� �������� ī�޶� �̵�
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
    /// Rigidboty.velocity�� ����� ���,
    /// ��� �� Layer ������ ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator DashRoutine()
    {
        float dashTime = 0;
        player.actionLimite = true;
        gameObject.layer = 1 >> gameObject.layer;                               // Layer �������� ��� �� ���� ����
        player.animator.SetTrigger("Dash");

        while (dashTime < 0.5f)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0);                // ��� �� �ϰ� ����

            if (player.render.flipX == false)                                  // �ٶ󺸴� �������� ���
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
    /// Jump Key ���� �ð��� ���� ���� ���� ����
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
                player.rb.velocity = new Vector2(player.rb.velocity.x, jumpSpeed);        // ������ �����ӵ� ����
            
            jumpTime += Time.deltaTime;
            lookTime = 0; 
            player.animator.SetBool("IsLook", isLook = false);
            player.cameraController.ResetVertical();

            yield return null;
        }
    }
    #endregion
}
