using HuskHornheadState;
using System;
using UnityEngine;

public class HuskHornhead : Monster
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rushSpeed;
    [SerializeField] public Vector2 attackRange;
    [NonSerialized] public Animator animator;
    [NonSerialized] public SpriteRenderer render;

    private LayerMask groundLayer;
    private LayerMask playerLayer;
    private StateBase[] states;
    private StateHuskHornhead curState;

    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        states = new StateBase[(int)StateHuskHornhead.Size];
        states[(int)StateHuskHornhead.Walk] = new WalkState(this);
        states[(int)StateHuskHornhead.Idle] = new IdleState(this);
        states[(int)StateHuskHornhead.Attack] = new AttackState(this);
        states[(int)StateHuskHornhead.Die] = new DieState(this);
    }
    #region State
    /// <summary>
    /// FSM 기본 설정
    /// </summary>
    private void Start()
    {
        curState = StateHuskHornhead.Walk;
    }

    private void Update()
    {
        if (curHp <= 0 && alive)
        {
            ChangeState(StateHuskHornhead.Die);
            alive = false;
        }

        states[(int)curState].Update();
    }

    public void ChangeState(StateHuskHornhead state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }
    #endregion

    #region WalkCheck
    /// <summary>
    /// 진행 방향에 벽, 혹은 낭떠러지를 확인, WalkState에서 사용
    /// </summary>
    public bool WalkCheck()
    {
        if (WallCheck() || !OneStepAHeadCheck())
            return true;
        else
            return false;
    }

    private bool WallCheck()
    {
        RaycastHit2D hit;

        if (render.flipX)
            hit = Physics2D.Raycast(transform.position, Vector2.left, 4, groundLayer);
        else
            hit = Physics2D.Raycast(transform.position, Vector2.right, 4, groundLayer);

        Debug.DrawRay(transform.position, Vector2.left * 4, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * 4, Color.red);

        if (hit.collider != null)
            return true;
        else
            return false;
    }

    private bool OneStepAHeadCheck()
    {
        RaycastHit2D hit;

        if (render.flipX)
            hit = Physics2D.Raycast(new Vector2 (transform.position.x + 3, transform.position.y), 
                Vector2.down, 8, groundLayer);
        else
            hit = Physics2D.Raycast(new Vector2(transform.position.x - 3, transform.position.y),
                Vector2.down, 8, groundLayer);

        Debug.DrawRay((new Vector2(transform.position.x + 3, transform.position.y)), 
            Vector2.down * 8, Color.white);
        Debug.DrawRay((new Vector2(transform.position.x - 3, transform.position.y)), 
            Vector2.down * 8, Color.white);

        if (hit.collider != null)
            return true;
        else
            return false;
    }
    #endregion

    #region AttackRange
    /// <summary>
    /// Player 감지되면 AttackState 실행, Player 방향으로 Filp.X 조정
    /// </summary>
    /// <return> 범위 내 Player가 감지되면 Ture, 안되면 False </return>
    public bool PlayerDetection()
    {
        if (curState == StateHuskHornhead.Attack)
            return false;

        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, attackRange, 0, playerLayer);

        foreach (Collider2D collider in cols)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Vector2 dir = (transform.position - collider.transform.position).normalized;

                if (dir.x > 0)
                    render.flipX = false;
                else
                    render.flipX = true;

                return true;
            }
        }
        
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, attackRange);
    }
    #endregion
}

namespace HuskHornheadState
{
    public enum StateHuskHornhead { Walk, Idle, Attack, Die, Size }

    public class WalkState : StateBase
    {
        private HuskHornhead huskHornhead;
        private Animator Animator { get { return huskHornhead.animator; } }
        private SpriteRenderer Render { get { return huskHornhead.render; } }
        private float MoveSpeed { get { return huskHornhead.moveSpeed; } }

        public WalkState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {
            Animator.SetBool("IsWalk", true);
        }

        public override void Update()
        {
            if (huskHornhead.PlayerDetection())
                huskHornhead.ChangeState(StateHuskHornhead.Attack);

            if (!huskHornhead.WalkCheck())
            {
                if (Render.flipX)
                    huskHornhead.transform.Translate(new Vector2(MoveSpeed * Time.deltaTime, 0));
                else
                    huskHornhead.transform.Translate(new Vector2(-MoveSpeed * Time.deltaTime, 0));
            }
            else
                huskHornhead.ChangeState(StateHuskHornhead.Idle);
        }

        public override void Exit()
        {
            Animator.SetBool("IsWalk", false);
        }
    }

    public class IdleState : StateBase
    {
        private HuskHornhead huskHornhead;
        private Animator Animator { get { return huskHornhead.animator;  } }
        private SpriteRenderer Render { get { return huskHornhead.render; } }

        public IdleState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            if (huskHornhead.PlayerDetection())
                huskHornhead.ChangeState(StateHuskHornhead.Attack);

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("TurnBack")
                && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                if (Render.flipX)
                    Render.flipX = false;
                else
                    Render.flipX = true;

                huskHornhead.ChangeState(StateHuskHornhead.Walk);
            }
        }

        public override void Exit()
        {

        }
    }

    public class AttackState : StateBase
    {
        private HuskHornhead huskHornhead;
        private Animator Animator { get { return huskHornhead.animator; } }
        private SpriteRenderer Render { get { return huskHornhead.render; } }
        private float RushSpeed { get { return huskHornhead.rushSpeed; } }
        private bool startRush;

        public AttackState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {
            startRush = false;
            Animator.SetTrigger("Attack");
        }

        public override void Update()
        {
            if (!startRush && Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                startRush = true;

            if (!huskHornhead.WalkCheck())
            {
                if (startRush)
                {
                    if (Render.flipX)
                        huskHornhead.transform.Translate(new Vector2(RushSpeed * Time.deltaTime, 0));
                    else
                        huskHornhead.transform.Translate(new Vector2(-RushSpeed * Time.deltaTime, 0));
                }
            }
            else
                huskHornhead.ChangeState(StateHuskHornhead.Idle);
        }

        public override void Exit()
        {
            Animator.SetTrigger("AttackEnd");
        }
    }

    public class DieState : StateBase
    {
        private HuskHornhead huskHornhead;
        private Animator Animator { get { return huskHornhead.animator; } }

        public DieState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {
            Animator.SetTrigger("Die");
            huskHornhead.gameObject.layer = 1 >> huskHornhead.gameObject.layer;
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            
        }
    }
}
