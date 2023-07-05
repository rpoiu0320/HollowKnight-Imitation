using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GruzMotherState;
using System;
using System.Linq;
using UnityEditor;

public class GruzMother : Monster
{
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float rushSpeed;
    [SerializeField] public float wildSlamSpeed;
    [SerializeField] public float vierticalSpeed;
    [SerializeField] public float horizonSpeed;
    private SpriteRenderer render;
    private StateBase[] states;
    private StateGruzMother curState;
    private bool isGround;

    [NonSerialized] public ContactFilter2D contactFilter;
    [NonSerialized] public Collider2D col;
    [NonSerialized] public Flying flying;
    [NonSerialized] public Animator animator;
    [NonSerialized] public Transform playerTransform;
    [NonSerialized] public bool isFly;


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
            gruzMother.flying.IsFly(gruzMother.isFly = true);
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;
            random = UnityEngine.Random.Range(2, 4);

            if (idleTime > 3f)
            {
                //gruzMother.ChangeState((StateGruzMother)random);
                gruzMother.ChangeState(StateGruzMother.WildSlam);
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
            gruzMother.animator.SetTrigger("EndRush");
        }

        public Coroutine rushRoutine;
        IEnumerator RushRoutine()
        {
            gruzMother.animator.SetTrigger("ReadyAttack");
            gruzMother.animator.SetTrigger("IsRush");

            yield return new WaitForSeconds(0.5f);

            Vector2 dir = (gruzMother.playerTransform.position - gruzMother.transform.position).normalized;

            while (rushTime < 1f)
            {
                gruzMother.transform.Translate(dir * gruzMother.rushSpeed * Time.deltaTime);

                if (Physics2D.OverlapCircle(new Vector2(gruzMother.transform.position.x, gruzMother.transform.position.y + 0.5f), 4f, gruzMother.groundLayer))
                    break;

                yield return null;
            }

            gruzMother.ChangeState(StateGruzMother.Idle);

            yield break;
        }
    }
    
    public class WildSlamState : StateBase
    {
        private GruzMother gruzMother;
        private enum Dir { Up, Down, Left, Right }
        private Dir horizonCheck;
        private Dir verticalCheck;
        private float wildSlamTime;

        public WildSlamState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }
    
        public override void Enter()
        {
            wildSlamRoutine = gruzMother.StartCoroutine(WildSlamRoutine());
            wildSlamTime = 0;
            horizonCheck = Dir.Left;
            verticalCheck = Dir.Down;
        }
    
        public override void Update()
        {
            wildSlamTime += Time.deltaTime;
        }
    
        public override void Exit()
        {
            
        }

        private void VerticalCheck()
        {
            RaycastHit2D hitDown = Physics2D.Raycast(gruzMother.transform.position, Vector2.down, 4f, gruzMother.groundLayer);
            RaycastHit2D hitUp = Physics2D.Raycast(gruzMother.transform.position, Vector2.up, 5f, gruzMother.groundLayer);
            Debug.DrawRay(gruzMother.transform.position, Vector2.down * 4f, Color.red);
            Debug.DrawRay(gruzMother.transform.position, Vector2.up * 5f, Color.red);

            if (hitDown.collider != null && hitDown.collider.gameObject != gruzMother.gameObject)
            {
                gruzMother.animator.SetTrigger("BumpGround");
                verticalCheck = Dir.Up;
            }
            else if (hitUp.collider != null && hitUp.collider.gameObject != gruzMother.gameObject)
            {
                gruzMother.animator.SetTrigger("BumpCeiling");
                verticalCheck = Dir.Down;
            }
        }

        private void HorizonCheck()
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(gruzMother.transform.position, Vector2.left, 5.5f, gruzMother.groundLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(gruzMother.transform.position, Vector2.right, 5.5f, gruzMother.groundLayer);
            Debug.DrawRay(gruzMother.transform.position, Vector2.left * 5.5f, Color.red);
            Debug.DrawRay(gruzMother.transform.position, Vector2.right * 5.5f, Color.red);

            if (hitLeft.collider != null && hitLeft.collider.gameObject != gruzMother.gameObject)
            {
                horizonCheck = Dir.Right;
            }
            else if (hitRight.collider != null && hitRight.collider.gameObject != gruzMother.gameObject)
            {
                horizonCheck = Dir.Left;
            }
        }

        Coroutine wildSlamRoutine;
        IEnumerator WildSlamRoutine()
        {
            while(wildSlamTime < 10f)
            {
                switch (verticalCheck)
                {
                    case Dir.Up :
                        if (horizonCheck == Dir.Left)
                            gruzMother.transform.Translate(new Vector3(-gruzMother.horizonSpeed * Time.deltaTime, gruzMother.vierticalSpeed * Time.deltaTime));
                        else if (horizonCheck == Dir.Right)
                            gruzMother.transform.Translate(new Vector3(gruzMother.horizonSpeed * Time.deltaTime, gruzMother.vierticalSpeed * Time.deltaTime));
                        break;

                    case Dir.Down :
                        if (horizonCheck == Dir.Left)
                            gruzMother.transform.Translate(new Vector3(-gruzMother.horizonSpeed * Time.deltaTime, -gruzMother.vierticalSpeed * Time.deltaTime));
                        else if (horizonCheck == Dir.Right)
                            gruzMother.transform.Translate(new Vector3(gruzMother.horizonSpeed * Time.deltaTime, -gruzMother.vierticalSpeed * Time.deltaTime));
                        break;

                    default :
                        break;
                }
                HorizonCheck();
                VerticalCheck();

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
    
        }
    
        public override void Update()
        {
    
        }
    
        public override void Exit()
        {
    
        }
    }
}
