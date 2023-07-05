using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GruzMotherState;
using System;

public class GruzMother : Monster
{
    [SerializeField] LayerMask groundLayer;
    private Animator animator;
    private SpriteRenderer render;
    private Collider2D col;
    private StateBase[] states;
    private StateGruzMother curState;

    private bool isGround;

    [NonSerialized] public Transform playerTransform;

    private int curHp;
    //GruzMother gruzMother = new GruzMother();

    public GruzMother(int curHp) : base()
    {
        //this.curHp = data.Monsters[(int)name].maxHp;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        curState = StateGruzMother.Sleep;
       // Debug.Log(data.Monsters[(int)MonsterData.monsterName.GruzMother].maxHp);
       //Debug.Log(data.Monsters[(int)MonsterData.monsterName.GruzMother].name);
       // Debug.Log(data.Monsters[(int)MonsterData.monsterName.GruzMother].haveGeo);
    }

    private void Update()
    {
        //states[(int)curState].Update();
        //Debug.Log(curHp);
        //Debug.Log(name);
    }

    public void ChangeState(StateGruzMother state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();

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

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }

    public class IdleState : StateBase
    {
        private GruzMother gruzMother;

        public IdleState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {

        }

        public override void Setup()
        {

        }

        public override void Transition()
        {

        }

        public override void Update()
        {

        }

        public class RushState : StateBase
        {
            private GruzMother gruzMother;

            public RushState(GruzMother gruzMother)
            {
                this.gruzMother = gruzMother;
            }

            public override void Enter()
            {

            }

            public override void Exit()
            {

            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {

            }
        }

        public class WildSlamState : StateBase
        {
            private GruzMother gruzMother;

            public WildSlamState(GruzMother gruzMother)
            {
                this.gruzMother = gruzMother;
            }

            public override void Enter()
            {

            }

            public override void Exit()
            {

            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {

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

            public override void Exit()
            {

            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {

            }
        }
    }
}
