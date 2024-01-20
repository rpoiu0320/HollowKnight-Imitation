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

    private bool ActionLimite { get { return player.actionLimite; } set { player.actionLimite = value; } }
    private Animator Animator { get { return player.animator; } }
    private Rigidbody2D Rb { get { return player.rb; } }
    private SpriteRenderer Render { get { return player.render; } }
    private CameraController CameraController { get { return player.cameraController; } }
    private Vector2 InputDIr { get { return player.inputDir; } }
    private bool IsGround { get { return player.isGround; } }

    private bool pressJumpKey;
    private bool isLook = false;
    private float lookTime = 0;

    private void Update()
    {
        Move();
        Animator.SetFloat("Move", Mathf.Abs(InputDIr.x));
    }

    #region Move and Look
    /// <summary>
    /// player.inputDir�� ������� Character�̵�
    /// </summary>
    private void Move()
    {
        if (ActionLimite)
            return;

        if (InputDIr.x > 0)
        {
            lookTime = 0;
            Animator.SetBool("IsLook", isLook = false);
            CameraController.ResetVertical();
            Render.flipX = false;
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
        }
        else if (InputDIr.x < 0)
        {
            lookTime = 0;
            Animator.SetBool("IsLook", isLook = false);
            CameraController.ResetVertical();
            Render.flipX = true;
            transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, 0));
        }
        else if (InputDIr.y != 0)
        {
            Animator.SetFloat("LookUpDown", InputDIr.y);

            if (!IsGround)
                return;

            lookTime += Time.deltaTime;

            if (lookTime > 1.5f && !isLook)
                lookRoutine = StartCoroutine(LookRoutine(InputDIr.y));
        }
        else if (InputDIr.y == 0 || ActionLimite)
        {
            Animator.SetFloat("LookUpDown", 0);
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
        Animator.SetBool("IsLook", isLook = true);
        CameraController.UpDownVertical(dirY);

        while (true)
        {
            if (InputDIr.y == 0 || ActionLimite)
            {
                Animator.SetBool("IsLook", isLook = false);

                break;
            }

            yield return null;
        }

        CameraController.ResetVertical();
    }
    #endregion

    #region Dash
    private void OnDash(InputValue value)
    {
        if (ActionLimite)
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
        ActionLimite = true;
        gameObject.layer = 1 >> gameObject.layer;                               // Layer �������� ��� �� ���� ����
        Animator.SetTrigger("Dash");

        while (dashTime < 0.5f)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, 0);                // ��� �� �ϰ� ����

            if (Render.flipX == false)                                  // �ٶ󺸴� �������� ���
                Rb.velocity = new Vector2(dashSpeed, 0);
            else
                Rb.velocity = new Vector2(-dashSpeed, 0);

            dashTime += Time.deltaTime;

            yield return null;
        }

        Rb.velocity = new Vector2(0, 0);
        gameObject.layer = 3 << gameObject.layer;
        ActionLimite = false;
    }
    #endregion

    #region jump
    private void OnJump(InputValue value)
    {
        pressJumpKey = value.isPressed;

        if (pressJumpKey && !ActionLimite && IsGround)
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

        while (pressJumpKey && !ActionLimite)
        {
            if (jumpTime > 0.4f)
                break;
            else
                Rb.velocity = new Vector2(Rb.velocity.x, jumpSpeed);        // ������ �����ӵ� ����
            
            jumpTime += Time.deltaTime;
            lookTime = 0; 
            Animator.SetBool("IsLook", isLook = false);
            CameraController.ResetVertical();

            yield return null;
        }
    }
    #endregion
}
