using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GruzMotherState;
using System;

public class GruzMother : Monster
{
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float rushSpeed;
    [SerializeField] public float wildSlamSpeed;
    [SerializeField] public float vierticalSpeed;
    [SerializeField] public float horizonSpeed;
    private StateBase[] states;
    private StateGruzMother curState;
    private bool isGround;

    [NonSerialized] public SpriteRenderer render;
    [NonSerialized] public ContactFilter2D contactFilter;
    [NonSerialized] public Collider2D col;
    [NonSerialized] public Flying flying;
    [NonSerialized] public Animator animator;
    [NonSerialized] public Transform playerTransform;
    [NonSerialized] public bool isFly;
    [NonSerialized] public bool isSleep;


    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        flying = GetComponent<Flying>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        states = new StateBase[(int)StateGruzMother.Size];
        states[(int)StateGruzMother.Sleep] =    new SleepState(this);
        states[(int)StateGruzMother.Idle] =     new IdleState(this);
        states[(int)StateGruzMother.Rush] =     new RushState(this);
        states[(int)StateGruzMother.WildSlam] = new WildSlamState(this);
        states[(int)StateGruzMother.Die] =      new DieState(this);
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        flying.IsFly(isFly = false);
        curState = StateGruzMother.Sleep;
        isSleep = true;
    }

    private void Update()
    {
        if (curHp <= 0)
            ChangeState(StateGruzMother.Die);

        states[(int)curState].Update();
    }

    public void ChangeState(StateGruzMother state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + 0.5f), 4f);
    }
}

namespace GruzMotherState
{
    public enum StateGruzMother { Sleep, Idle, Rush, WildSlam, Die, Size }

    public class SleepState : StateBase
    {
        private GruzMother gruzMother;

        public SleepState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            if (gruzMother.curHp < gruzMother.data.Monsters[(int)gruzMother.monsterName].maxHp)
            {
                gruzMother.ChangeState(StateGruzMother.Idle);
                awakeRoutine = gruzMother.StartCoroutine(AwakeRoutine());
            }
        }

        public override void Exit()
        {

        }

        Coroutine awakeRoutine;
        IEnumerator AwakeRoutine()
        {
            gruzMother.animator.SetBool("IsSleep", false);

            yield return new WaitForSeconds(1f);

            gruzMother.flying.IsFly(gruzMother.isFly = true);

            yield break;
        }
    }

    public class IdleState : StateBase
    {
        private GruzMother gruzMother;
        private float idleTime;
        private int random;

        public IdleState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }

        public override void Enter()
        {
            idleTime = 0;

            if (gruzMother.isSleep)     // ó�� ����� �� AwakeRoutine�� ����Ͽ� ����� �����ð��� ������Ű�� ����
            {
                gruzMother.isSleep = false;
                return;
            }

            gruzMother.flying.IsFly(gruzMother.isFly = true);
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;
            random = UnityEngine.Random.Range(2, 4);

            if (idleTime > 3f)
            {
                //gruzMother.ChangeState((StateGruzMother)random);    // Rush, WildSlam �� �ϳ�
                //gruzMother.ChangeState(StateGruzMother.WildSlam);
                gruzMother.ChangeState(StateGruzMother.Rush);
            }
        }

        public override void Exit()
        {
            gruzMother.flying.IsFly(gruzMother.isFly = false);
        }
    }

    public class RushState : StateBase
    {
        private GruzMother gruzMother;
        private float rushTime;
    
        public RushState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }
    
        public override void Enter()
        {
            rushRoutine = gruzMother.StartCoroutine(RushRoutine());
            rushTime = 0;
        }
        
        public override void Update()
        {
            rushTime += Time.deltaTime;
        }
    
        public override void Exit()
        {
            
        }

        public Coroutine rushRoutine;
        IEnumerator RushRoutine()
        {
            Vector2 dir = (gruzMother.playerTransform.position - gruzMother.transform.position).normalized;

            if (dir.x > 0)
                gruzMother.render.flipX = true;
            if (dir.x < 0)
                gruzMother.render.flipX = false;

            gruzMother.animator.SetTrigger("ReadyAttack");
            gruzMother.animator.SetTrigger("IsRush");

            yield return new WaitForSeconds(0.5f);

            while (rushTime < 1f)
            {
                gruzMother.transform.Translate(dir * gruzMother.rushSpeed * Time.deltaTime);

                if (Physics2D.OverlapCircle(new Vector2(gruzMother.transform.position.x, gruzMother.transform.position.y + 0.5f), 4f, gruzMother.groundLayer))
                    break;

                yield return null;
            }

            gruzMother.animator.SetTrigger("EndRush");

            yield return new WaitForSeconds(0.35f);

            gruzMother.ChangeState(StateGruzMother.Idle);

            yield break;
        }
    }
    
    public class WildSlamState : StateBase
    {
        private GruzMother gruzMother;
        private enum MoveDir { Up, Down, Left, Right, Null }
        private MoveDir horizonCheck;
        private MoveDir verticalCheck;
        private float wildSlamTime;
        private float random;
        private bool waitTiming;

        public WildSlamState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }
    
        public override void Enter()
        {
            wildSlamRoutine = gruzMother.StartCoroutine(WildSlamRoutine());
            wildSlamTime = 0;
            random = UnityEngine.Random.Range((float)MoveDir.Up, (float)MoveDir.Left);
            verticalCheck = (MoveDir)random;
            random = UnityEngine.Random.Range((float)MoveDir.Left, (float)MoveDir.Null);
            horizonCheck = (MoveDir)random;
        }
    
        public override void Update()
        {
            wildSlamTime += Time.deltaTime;
        }
    
        public override void Exit()
        {
            
        }

        private void VerticalCheck(MoveDir verticalCheck)
        {
            if (verticalCheck == MoveDir.Up)    // ���ϸ� ���̱� ���� �ʿ��� ��Ȳ������ �ش� �������� Ray�߻�, �󸶳� ���� �ɸ������� ��
            {                                   // and �и������� ���� rb.Constraints�� ��� ture�� �ϴ� �׻� Ray�߻� �� �������� ������ ����, �̸� ���� Ray���� �߻�
                RaycastHit2D hitUp = Physics2D.Raycast(gruzMother.transform.position, Vector2.up, 5f, gruzMother.groundLayer);

                if (hitUp.collider != null && hitUp.collider.gameObject != gruzMother.gameObject)
                {
                    gruzMother.animator.SetTrigger("BumpCeiling");
                    this.verticalCheck = MoveDir.Down;
                    waitTiming = true;
                }
            }
            else if (verticalCheck == MoveDir.Down)
            {
                RaycastHit2D hitDown = Physics2D.Raycast(gruzMother.transform.position, Vector2.down, 4f, gruzMother.groundLayer);

                if (hitDown.collider != null && hitDown.collider.gameObject != gruzMother.gameObject)
                {
                    gruzMother.animator.SetTrigger("BumpGround");
                    this.verticalCheck = MoveDir.Up;
                    waitTiming = true;
                }
            }

            Debug.DrawRay(gruzMother.transform.position, Vector2.down * 4f, Color.red);
            Debug.DrawRay(gruzMother.transform.position, Vector2.up * 5f, Color.red);
        }

        private void HorizonCheck(MoveDir horizonCheck)
        {
            if (horizonCheck == MoveDir.Left)
            {
                RaycastHit2D hitLeft = Physics2D.Raycast(gruzMother.transform.position, Vector2.left, 5.5f, gruzMother.groundLayer);

                if (hitLeft.collider != null && hitLeft.collider.gameObject != gruzMother.gameObject)
                    this.horizonCheck = MoveDir.Right;
            }
            else if (horizonCheck == MoveDir.Right)
            {
                RaycastHit2D hitRight = Physics2D.Raycast(gruzMother.transform.position, Vector2.right, 5.5f, gruzMother.groundLayer);

                if (hitRight.collider != null && hitRight.collider.gameObject != gruzMother.gameObject)
                    horizonCheck = MoveDir.Left;
            }
            
            Debug.DrawRay(gruzMother.transform.position, Vector2.left * 5.5f, Color.red);
            Debug.DrawRay(gruzMother.transform.position, Vector2.right * 5.5f, Color.red);
        }

        Coroutine wildSlamRoutine;
        IEnumerator WildSlamRoutine()
        {
            gruzMother.animator.SetTrigger("ReadyAttack");
            
            yield return new WaitForSeconds(0.3f);          // �ڿ������� �ִϸ��̼��� ����

            gruzMother.animator.SetTrigger("IsWildSlam");

            yield return new WaitForSeconds(0.3f);

            while (wildSlamTime < 8f)
            {
                switch (verticalCheck)
                {
                    case MoveDir.Up :
                        if (horizonCheck == MoveDir.Left)
                        {
                            gruzMother.transform.Translate(new Vector3(-gruzMother.horizonSpeed * Time.deltaTime, gruzMother.vierticalSpeed * Time.deltaTime));
                            gruzMother.render.flipX = false;
                        }
                        else if (horizonCheck == MoveDir.Right)
                        {
                            gruzMother.transform.Translate(new Vector3(gruzMother.horizonSpeed * Time.deltaTime, gruzMother.vierticalSpeed * Time.deltaTime));
                            gruzMother.render.flipX = true;
                        }
                        break;

                    case MoveDir.Down :
                        if (horizonCheck == MoveDir.Left)
                        {
                            gruzMother.transform.Translate(new Vector3(-gruzMother.horizonSpeed * Time.deltaTime, -gruzMother.vierticalSpeed * Time.deltaTime));
                            gruzMother.render.flipX = false;
                        }
                        else if (horizonCheck == MoveDir.Right)
                        {
                            gruzMother.transform.Translate(new Vector3(gruzMother.horizonSpeed * Time.deltaTime, -gruzMother.vierticalSpeed * Time.deltaTime));
                            gruzMother.render.flipX = true;
                        }
                        break;

                    default :
                        break;
                }

                HorizonCheck(horizonCheck);
                VerticalCheck(verticalCheck);

                if (waitTiming)     // SetTrigger Bump�� ���� ������ �Ͻ�����
                {
                    yield return new WaitForSeconds(0.25f);
                    waitTiming = false;
                }

                yield return null;
            }

            gruzMother.ChangeState(StateGruzMother.Idle);

            yield break;
        }
    }
    
    public class DieState : StateBase
    {
        private GruzMother gruzMother;
    
        public DieState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }
    
        public override void Enter()
        {
            gruzMother.animator.SetTrigger("IsDIe");
        }
    
        public override void Update()
        {
    
        }
    
        public override void Exit()
        {
    
        }
    }
}
