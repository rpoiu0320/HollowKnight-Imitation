using HuskHornheadState;
using System;
using System.Collections;
using UnityEngine;

public class HuskHornhead : Monster
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rushSpeed;
    [SerializeField] public Vector2 attackRange;
    [NonSerialized] public Animator animator;
    [NonSerialized] public SpriteRenderer render;
    [NonSerialized] public Collider2D col;

    private LayerMask groundLayer;
    private LayerMask playerLayer;
    private StateBase[] states;
    private StateHuskHornhead curState;

    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
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
        Debug.Log(curState.ToString());

        if (curHp <= 0 && alive)
        {
            ChangeState(StateHuskHornhead.Die);
            alive = false;
        }

        states[(int)curState].Update();

        if (curState != StateHuskHornhead.Attack && PlayerDetection())
            curState = StateHuskHornhead.Attack; 
    }

    public void ChangeState(StateHuskHornhead state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();
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
            hit = Physics2D.Raycast(transform.position, Vector2.left, 8, groundLayer);
        else
            hit = Physics2D.Raycast(transform.position, Vector2.right, 8, groundLayer);

        Debug.DrawRay(transform.position, Vector2.left * 8, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * 8, Color.red);

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

    #region TurnBack
    /// <summary>
    /// TrunBack을 Coroutine화 해서 IdleState에서 사용
    /// </summary>
    public void TurnBack()
    {
        turnBackRoutine = StartCoroutine(TurnBackRoutine());
    }

    Coroutine turnBackRoutine;
    IEnumerator TurnBackRoutine()
    {
        animator.SetTrigger("TurnBack");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9);

        if (render.flipX)
            render.flipX = false;
        else
            render.flipX = true;

        ChangeState(StateHuskHornhead.Walk);
    }
    #endregion

    #region AttackRange
    /// <summary>
    /// Player 감지되면 AttackState 실행, Player 방향으로 Filp.X 조정
    /// </summary>
    /// <return> 범위 내 Player가 감지되면 Ture, 안되면 False </return>
    private bool PlayerDetection()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, attackRange, 0, playerLayer);

        foreach (Collider2D collider in cols)
        {
            if (collider.gameObject.layer == playerLayer)
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
        private float idleTime;

        public IdleState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {
            idleTime = 0;
        }

        public override void Update()
        {
            if (idleTime > 3f)
                huskHornhead.TurnBack();

            idleTime += Time.deltaTime;
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

        public AttackState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {
            Animator.SetTrigger("Attack");
        }

        public override void Update()
        {
            if (!huskHornhead.WalkCheck())
            {
                if (Render.flipX)
                    huskHornhead.transform.Translate(new Vector2(RushSpeed * Time.deltaTime, 0));
                else
                    huskHornhead.transform.Translate(new Vector2(-RushSpeed * Time.deltaTime, 0));
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
        private Collider2D Col { get { return huskHornhead.col; } }

        public DieState(HuskHornhead huskHornhead)
        {
            this.huskHornhead = huskHornhead;
        }

        public override void Enter()
        {
            Animator.SetBool("IsDie", true);
            Col.enabled = false;
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            
        }
    }
}
